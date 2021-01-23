using Sirenix.OdinInspector;
using System.Collections.Generic;
using System.IO;
using UnityEditor;
using UnityEngine;

namespace CrystalAlchemist
{
    [CreateAssetMenu(menuName = "Game/Editor/Player Sprite Sheet")]
    public class PlayerSpriteTool : ScriptableObject
    {
        [BoxGroup]
        [SerializeField]
        private Vector2 pivot = new Vector2(0.5f, 0.02085f);

#if UNITY_EDITOR
        [Button]
        public void UpdateSpriteSheet()
        {
            if (EditorUtility.DisplayDialog("Update Animation", "Do you want to update all animations?\nThis could take a while... .", "Do it", "Nope")) UpdateSprites();
        }

        public void UpdateSprites()
        {
            DirectoryInfo currentDir = new DirectoryInfo(Application.dataPath + @"/Resources/Scriptable Objects/Character Creation/Properties");
            FileInfo[] files = currentDir.GetFiles("*.asset", SearchOption.AllDirectories);

            foreach (FileInfo file in files) SliceAndNameSprites(file.FullName);

            AssetDatabase.SaveAssets();
            AssetDatabase.Refresh();

            Debug.Log("<color=green>COMPLETED!</color>");
        }

        private void SliceAndNameSprites(string propertyPath)
        {
            string replace = (Application.dataPath + @"/Resources/").Replace("/", "\\");
            string _path = propertyPath.Replace(replace, "").Replace(".asset", "");

            CharacterCreatorProperty property = Resources.Load<CharacterCreatorProperty>(_path);

            if (property.dontUpdateSprites) return;

            Texture2D myTexture = property.texture;
            string path = AssetDatabase.GetAssetPath(myTexture);
            string metaPath = Application.dataPath.Replace("Assets", "") + path.Replace("png", "meta");

            //File.Delete(metaPath);

            TextureImporter ti = AssetImporter.GetAtPath(path) as TextureImporter;
            ti.isReadable = true;

            ti.spritePixelsPerUnit = 16;
            ti.spriteImportMode = SpriteImportMode.Multiple;
            ti.filterMode = FilterMode.Point;
            ti.textureCompression = TextureImporterCompression.Uncompressed;

            List<SpriteMetaData> newData = new List<SpriteMetaData>();

            int SliceWidth = property.GetSize().x;
            int SliceHeight = property.GetSize().y;
            int columnIndex = 0;
            int spriteIndex = 0;

            for (int i = 0; i < myTexture.width; i += SliceWidth)
            {
                int rowIndex = 0;

                for (int j = myTexture.height; j > 0; j -= SliceHeight)
                {
                    SpriteMetaData smd = new SpriteMetaData();
                    smd.pivot = this.pivot;
                    smd.alignment = 9;
                    smd.name = "" + spriteIndex;
                    smd.rect = new Rect(i, j - SliceHeight, SliceWidth, SliceHeight);

                    newData.Add(smd);

                    rowIndex++;
                    spriteIndex++;
                }

                columnIndex++;
            }

            ti.spritesheet = newData.ToArray();

            AssetDatabase.ImportAsset(path, ImportAssetOptions.ForceUpdate);
            AssetDatabase.LoadAssetAtPath<Texture2D>(path);

            Debug.Log("<color=blue>Sliced and Named: " + path + " into " + newData.Count + " sprites</color>");
        }

#endif
    }
}