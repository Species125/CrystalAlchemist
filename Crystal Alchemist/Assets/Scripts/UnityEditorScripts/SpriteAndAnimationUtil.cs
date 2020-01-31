﻿using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;
using UnityEditor;
using System.IO;
using System.Linq;

[System.Serializable]
public struct FileList
{
    public GameObject spriteRendererObject;
    public string path;
}

[System.Serializable]
public struct AnimationKey
{
    public int startIndex;
    public string animTyp;
    public int intervall;
}

public class SpriteAndAnimationUtil : MonoBehaviour
{
    [FoldoutGroup("Animations", Expanded = false)]
    [SerializeField]
    private string assetPath = "Assets/Art/Graphics/Characters/Player Sprites/";

    [FoldoutGroup("Animations", Expanded = false)]
    [SerializeField]
    private string animationPath = "Animations/Player Animations/";

    [FoldoutGroup("Animations", Expanded = false)]
    [SerializeField]
    private List<AnimationKey> animationKeys = new List<AnimationKey>();

    [FoldoutGroup("Animations", Expanded = false)]
    [SerializeField]
    private Character character;

    private string[] animArray = new string[] { "Down", "Right", "Left", "Up" };

#if UNITY_EDITOR
    [Button]
    public void UpdateSpritesAndAnimations()
    {
        List<PlayerAnimationPart> childObjects = new List<PlayerAnimationPart>();
        GetChildObjects(this.transform, childObjects); //get all children from GameObject

        List<AnimationClip> clips = getAnimationClips(); //get all AnimationClips from Asset Folder

        foreach (PlayerAnimationPart part in childObjects)
        {
            SliceAndNameSprites(part); //Slice and Name Sprites
        }

        foreach (AnimationClip clip in clips)
        {      
            foreach (PlayerAnimationPart childObject in childObjects)
            {
                //set all animation clips foreach object
                SetAnimationClips(clip, childObject);
            }

            Debug.Log("<color=blue>Set Animation of: " + clip.name + " for " + childObjects.Count + " GameObjects.</color>");
        }
    }

#endif

    private string GetGameObjectPath(Transform transform)
    {
        string path = transform.name;
        while (transform.parent != null && transform.parent != this.transform)
        {
            transform = transform.parent;
            path = transform.name + "/" + path;
        }
        return path;
    }

    private void SetAnimationClips(AnimationClip clip, PlayerAnimationPart childObject)
    {
        if (clip != null)
        {
            string path = this.assetPath + "/" + childObject.path;
            GameObject spriteObject = childObject.gameObject;

            List<Object> sprite = AssetDatabase.LoadAllAssetsAtPath(path).ToList();
            List<Sprite> sprites = new List<Sprite>();

            foreach (Object obj in sprite)
            {
                if (obj is Sprite && obj.name.Contains(clip.name)) sprites.Add((Sprite)obj);
            }

            //sprites = sprites.OrderBy(c => int.Parse(c.name.Split('_').Last())).ToList();
            sprites = sprites.OrderBy(c => c.name).ToList();

            EditorCurveBinding spriteBinding = new EditorCurveBinding();
            spriteBinding.type = typeof(SpriteRenderer);
            spriteBinding.path = GetGameObjectPath(spriteObject.transform);
            spriteBinding.propertyName = "m_Sprite";

            if (sprites.Count > 0)
            {
                int intervall = getAnimationClipIntervall(sprites[0].name);
                int arrayLength = sprites.Count + 1;
                if (intervall <= 0) arrayLength = 1;

                ObjectReferenceKeyframe[] spriteKeyFrames = new ObjectReferenceKeyframe[arrayLength];
                float time = 0f;

                try
                {
                    for (int i = 0; i < spriteKeyFrames.Length; i++)
                    {
                        spriteKeyFrames[i] = new ObjectReferenceKeyframe();
                        spriteKeyFrames[i].time = time;

                        if (i == spriteKeyFrames.Length - 1) spriteKeyFrames[i].value = sprites[i - 1];
                        else spriteKeyFrames[i].value = sprites[i];

                        time += (float)(intervall / 60f);
                    }
                }
                catch
                {
                    Debug.Log("<color=red>Error: " + childObject.gameObject.name + " (" + childObject.path + ")</color>");
                }

                AnimationUtility.SetObjectReferenceCurve(clip, spriteBinding, spriteKeyFrames);
                //if(file.setSortOrder) clip.SetCurve("", typeof(SpriteRenderer), "m_SortOrder", AnimationCurve.Linear(0, file.sortOrder, 0, file.sortOrder));

                AssetDatabase.SaveAssets();
                AssetDatabase.Refresh();
            }
        }
        else
        {
            Debug.Log("<color=red>Error: Clip is null</color>");
        }
    }

    private void GetChildObjects(Transform transform, List<PlayerAnimationPart> childObjects)
    {
        foreach (Transform child in transform)
        {
            if (child.GetComponent<PlayerAnimationPart>() != null) childObjects.Add(child.GetComponent<PlayerAnimationPart>());

            GetChildObjects(child, childObjects);
        }
    }

    private List<AnimationClip> getAnimationClips()
    {
        List<string> files = Directory.GetFiles(Application.dataPath + "/" + this.animationPath, "*.anim").ToList();
        List<AnimationClip> clips = new List<AnimationClip>();

        foreach (string file in files)
        {
            string path = "Assets/"+file.Replace(Application.dataPath, "");
            clips.Add(AssetDatabase.LoadAssetAtPath(path, typeof(AnimationClip))as AnimationClip);
        }

        return clips;
    }

    private void SliceAndNameSprites(PlayerAnimationPart file)
    {
        string assetpath = this.assetPath + "/" + file.path;
        Texture2D myTexture = AssetDatabase.LoadAssetAtPath<Texture2D>(assetpath);

        File.Delete(Application.dataPath + "/" + this.assetPath.Replace("Assets", "") + "/" + file.path + ".meta");

        string path = AssetDatabase.GetAssetPath(myTexture);
        TextureImporter ti = AssetImporter.GetAtPath(path) as TextureImporter;
        ti.isReadable = true;

        ti.spritePixelsPerUnit = 16;
        ti.spriteImportMode = SpriteImportMode.Multiple;
        ti.filterMode = FilterMode.Point;
        ti.textureCompression = TextureImporterCompression.Uncompressed;

        List<SpriteMetaData> newData = new List<SpriteMetaData>();

        int SliceWidth = 32;
        int spriteIndex = 0;

        for (int i = 0; i < myTexture.width; i += SliceWidth)
        {
            int animDirection = 0;

            int SliceHeight = 48;
            Vector2 pivot = new Vector2(0.5f, 0.5f);

            string animTyp = getAnimationClipName(spriteIndex);

            for (int j = myTexture.height; j > 0 && animDirection < 4; j -= SliceHeight)
            {
                if (animDirection >= 3 && file.isTail)
                {
                    SliceHeight = 64;
                    pivot = new Vector2(0.5f, 0.625f);
                }

                SpriteMetaData smd = new SpriteMetaData();
                smd.pivot = pivot;
                smd.alignment = 9;

                smd.name = animTyp + " " + this.animArray[animDirection];
                if (spriteIndex > 0) smd.name = animTyp + " " + this.animArray[animDirection] + " " + spriteIndex;

                smd.rect = new Rect(i, j - SliceHeight, SliceWidth, SliceHeight);

                newData.Add(smd);

                animDirection++;
            }

            spriteIndex++;
        }

        ti.spritesheet = newData.ToArray();
        AssetDatabase.ImportAsset(path, ImportAssetOptions.ForceUpdate);
        Debug.Log("<color=blue>Sliced and Named: " + file.path + " into " + newData.Count + " sprites</color>");
    }

    private string getAnimationClipName(int spriteIndex)
    {
        string result = "Unbekannt";

        for (int i = this.animationKeys.Count - 1; i >= 0; i--)
        {
            if (spriteIndex >= this.animationKeys[i].startIndex)
            {
                result = this.animationKeys[i].animTyp;
                break;
            }
        }

        return result;
    }

    private int getAnimationClipIntervall(string name)
    {
        int result = 10;

        for (int i = this.animationKeys.Count - 1; i >= 0; i--)
        {
            if (name.ToUpper().Contains(this.animationKeys[i].animTyp.ToUpper()))
            {
                result = this.animationKeys[i].intervall;
                break;
            }
        }

        return result;
    }
}
