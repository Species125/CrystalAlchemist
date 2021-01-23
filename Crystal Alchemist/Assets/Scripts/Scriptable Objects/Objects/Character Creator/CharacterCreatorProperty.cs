using AssetIcons;
using Sirenix.OdinInspector;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using UnityEngine;

namespace CrystalAlchemist
{
    public enum EnableMode
    {
        always,
        name,
        race,
        nameAndRace
    }

    public enum RaceRestriction
    {
        include,
        exclude
    }

    public enum BodyPart
    {
        HeadGearBack,
        Face,
        FaceGear,
        Hairstyle,
        Ears,
        Horns,
        HeadGear,
        Tail,
        BackGear,
        Body,
        Legs,
        Decoration,
        Underwear,
        LowerGear,
        UpperGear,
        Empty
    }

    [System.Serializable]
    public struct ColorTable
    {
        public ColorGroup colorGroup;
        public Color highlight;
        public Color main;
        public Color shadows;
        public Color lines;
    }

    [System.Serializable]
    public class ColorEffect
    {
        public bool addGlow = false;

        [ShowIf("addGlow")]
        [ColorUsage(true, true)]
        public Color default_glow;
    }

    [CreateAssetMenu(menuName = "Game/CharacterCreation/Property")]
    public class CharacterCreatorProperty : NetworkScriptableObject
    {
        [BoxGroup("Start")]
        public BodyPart type;

        [BoxGroup("Start")]
        public bool dontUpdateSprites = false;

        [BoxGroup("Start")]
        [SerializeField]
        [HideIf("dontUpdateSprites")]
        private Vector2Int size = new Vector2Int(32, 48);

        [BoxGroup("Enable Info")]
        [SerializeField]
        private EnableMode enableMode = EnableMode.always;

        [HideIf("enableMode", EnableMode.always)]
        [HideIf("enableMode", EnableMode.name)]
        [BoxGroup("Enable Info")]
        [SerializeField]
        private RaceRestriction restriction = RaceRestriction.include;

        [HideIf("enableMode", EnableMode.always)]
        [HideIf("enableMode", EnableMode.name)]
        [BoxGroup("Enable Info")]
        [SerializeField]
        private List<Race> races = new List<Race>();        

        [AssetIcon]
        [PreviewField]
        [BoxGroup("Sprites")]
        [SerializeField]
        [ReadOnly]
        private Sprite preview;

        [BoxGroup("Sprites")]
        [Required]
        [OnValueChanged("SetPreview")]
        public Texture2D texture;

        [BoxGroup("Sprites")]
        public bool useCustomShader;

        [BoxGroup("Sprites")]
        [ShowIf("useCustomShader")]
        [Required]
        public Material material;

        [BoxGroup("Color Info")]
        public bool canBeColored = true;

        [BoxGroup("Color Info")]
        [ShowIf("canBeColored")]
        [SerializeField]
        private List<ColorTable> colorTables = new List<ColorTable>();

        [BoxGroup("Color Info")]
        [SerializeField]
        [HideLabel]
        private ColorEffect colorEffect;

        [BoxGroup("Part Info")]
        [SerializeField]
        [ReadOnly]
        private string texturePath;

        [BoxGroup("Part Info")]
        [SerializeField]
        [ReadOnly]
        private string fullPath;

        private string replace = "Assets/Resources/";

#if UNITY_EDITOR
        public void SetPreview()
        {            
            this.fullPath = UnityEditor.AssetDatabase.GetAssetPath(this.texture);
            this.texturePath = this.fullPath.Replace(replace,"").Split('.')[0];

            this.preview = GetSprites()[1];
        }
        #endif

        public Sprite GetSprite()
        {
            return this.preview;
        }

        public Vector2Int GetSize()
        {
            return this.size;
        }

        public Sprite[] GetSprites()
        {
            return Resources.LoadAll<Sprite>(this.GetPath()).OrderBy(x => Convert.ToInt32(Regex.Replace(x.name, "[^0-9]", ""))).ToArray();
        }

        public List<ColorTable> GetColorTable()
        {
            if (this.canBeColored) return this.colorTables;
            else return new List<ColorTable>();
        }

        public ColorEffect GetEffect()
        {
            return this.colorEffect;
        }

        public string GetPath()
        {
            return this.texturePath;
        }

        public bool EnableIt(Race race, CharacterCreatorProperty property = null)
        {
            if ((this.enableMode == EnableMode.race && RaceEnabled(race))
                || (this.enableMode == EnableMode.nameAndRace && RaceEnabled(race) && property == this)
                || (this.enableMode == EnableMode.name && property == this)) return true;

            return false;
        }

        private bool RaceEnabled(Race race)
        {
            if (this.restriction == RaceRestriction.include && this.races.Contains(race)) return true;
            else if (this.restriction == RaceRestriction.exclude && !this.races.Contains(race)) return true;
            return false;
        }        
    }
}