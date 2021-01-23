using Sirenix.OdinInspector;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEditor;
using UnityEngine;

namespace CrystalAlchemist
{
    [System.Serializable]
    public class AnimationKey
    {
        [HorizontalGroup("Column")]
        public int startIndex;
        [HorizontalGroup("Column")]
        public int endIndex;
        [HorizontalGroup("Row")]
        public int rowStartIndex;
        [HorizontalGroup("Row")]
        public int rowEndIndex;
        [HorizontalGroup("Anim")]
        public string animTyp;
        [HorizontalGroup("Anim")]
        public int intervall;
    }

    [CreateAssetMenu(menuName = "Game/Editor/Player Sprite Sheet")]
    public class PlayerSpriteTool : ScriptableObject
    {
        [BoxGroup]
        [SerializeField]
        [TextArea]
        private string assetPath = "Assets/Resources/Art/Characters/Player Sprites/";

        [BoxGroup]
        [TextArea]
        [SerializeField]
        private string animationPath = "Resources/Animations/Player Animations/";

        [BoxGroup]
        [SerializeField]
        private List<AnimationKey> animationKeys = new List<AnimationKey>();

        [BoxGroup]
        [SerializeField]
        private List<string> rows = new List<string>();

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

            List<string> spriteNames = new List<string>();

            for (int i = 0; i < myTexture.width; i += SliceWidth)
            {
                int rowIndex = 0;

                for (int j = myTexture.height; j > 0; j -= SliceHeight)
                {
                    AnimationKey column = getAnimationKey(columnIndex, rowIndex);
                    string row = getAnimationRowKey(rowIndex);

                    if (column != null)
                    {
                        string direction = row;                        
                        string animationName = column.animTyp;

                        SpriteMetaData smd = new SpriteMetaData();
                        smd.pivot = this.pivot;
                        smd.alignment = 9;

                        string spriteName = animationName + " " + direction;
                        if (column.endIndex - column.startIndex > 0) spriteName = animationName + " " + direction + " " + (columnIndex-column.startIndex);

                        smd.name = spriteName;
                        smd.rect = new Rect(i, j - SliceHeight, SliceWidth, SliceHeight);

                        newData.Add(smd);
                    }

                    rowIndex++;
                }

                columnIndex++;
            }

            ti.spritesheet = newData.ToArray();
            AssetDatabase.ImportAsset(path, ImportAssetOptions.ForceUpdate);

            AssetDatabase.LoadAssetAtPath<Texture2D>(path);

            Debug.Log("<color=blue>Sliced and Named: " + path + " into " + newData.Count + " sprites</color>");
        }


        private AnimationKey getAnimationKey(int columnIndex, int rowIndex)
        {
            foreach (AnimationKey column in this.animationKeys)
            {
                if (columnIndex >= column.startIndex
                    && columnIndex <= column.endIndex
                    && rowIndex >= column.rowStartIndex
                    && rowIndex <= column.rowEndIndex)
                {
                    return column;
                }
            }

            return null;
        }

        private string getAnimationRowKey(int rowIndex)
        {
            if (rowIndex < this.rows.Count) return this.rows[rowIndex];
            return null;
        }
#endif
    }
}