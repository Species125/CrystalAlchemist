using Sirenix.OdinInspector;
using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace CrystalAlchemist
{
    #region Enums
    public enum UIType
    {
        resource,
        buffsOnly,          //Nur Buffs
        debuffsOnly,        //Nur Debuffs
        BuffsAndDebuffs,    //Buffs und Debuffs
        DebuffsAndBuffs     //Debuffs und Buffs
    }
    #endregion

    public class StatusBar : MonoBehaviour
    {
        #region Attribute
        [BoxGroup]
        [Required]
        [SerializeField]
        private CharacterValues values;

        [BoxGroup("UI Typ")]
        [SerializeField]
        private UIType UIType = UIType.resource;

        [BoxGroup("UI Typ")]
        [ShowIf("UIType", UIType.resource)]
        [SerializeField]
        private CostType resourceType;

        [FoldoutGroup("Leiste für Mana und Leben", expanded: false)]
        [SerializeField]
        private GameObject bar;

        [FoldoutGroup("Leiste für Mana und Leben", expanded: false)]
        [SerializeField]
        private float maxLength = 900;

        [FoldoutGroup("Leiste für Mana und Leben", expanded: false)]
        [SerializeField]
        private Image frameBar;

        [FoldoutGroup("Leiste für Mana und Leben", expanded: false)]
        [SerializeField]
        private Image fillBar;

        [FoldoutGroup("Leiste für Mana und Leben", expanded: false)]
        [SerializeField]
        private Image backgroundBar;

        [FoldoutGroup("Leiste für Mana und Leben", expanded: false)]
        [SerializeField]
        private TextMeshProUGUI textFieldBar;

        [FoldoutGroup("Sprites für Mana und Leben", expanded: false)]
        [SerializeField]
        private GameObject symbol;

        [FoldoutGroup("Sprites für Mana und Leben", expanded: false)]
        [SerializeField]
        private Sprite full;
        [FoldoutGroup("Sprites für Mana und Leben", expanded: false)]
        [SerializeField]
        private Sprite quarterhalf;
        [FoldoutGroup("Sprites für Mana und Leben", expanded: false)]
        [SerializeField]
        private Sprite half;
        [FoldoutGroup("Sprites für Mana und Leben", expanded: false)]
        [SerializeField]
        private Sprite quarter;
        [FoldoutGroup("Sprites für Mana und Leben", expanded: false)]
        [SerializeField]
        private Sprite empty;
        [FoldoutGroup("Sprites für Mana und Leben", expanded: false)]
        [SerializeField]
        private List<GameObject> icons = new List<GameObject>();

        [FoldoutGroup("Warning", expanded: false)]
        [SerializeField]
        private GameObject warning;
        [SerializeField]
        [FoldoutGroup("Warning", expanded: false)]
        private AudioClip lowSoundEffect;
        [SerializeField]
        [FoldoutGroup("Warning", expanded: false)]
        private float schwelle = 0.5f;
        [FoldoutGroup("Warning", expanded: false)]
        [SerializeField]
        private float audioInterval = 1.5f;

        private AudioSource audioSource;
        private bool playLow;
        private float elapsed;
        private float maxValue;
        private float currentValue;
        #endregion


        #region Start Funktionen (Awake, Init, SetValues)

        private void Start() => init();

        private void OnDestroy()
        {
            SettingsEvents.current.OnHUDChanged -= setBarOrSymbol;
            GameEvents.current.OnLifeManaUpdate -= UpdateGUIHealthMana;
        }

        private void init()
        {
            SettingsEvents.current.OnHUDChanged += setBarOrSymbol;
            GameEvents.current.OnLifeManaUpdate += UpdateGUIHealthMana;

            this.audioSource = GetComponent<AudioSource>();
            setStatusBar();

            if (this.UIType == UIType.resource) UpdateGUIHealthMana();
            setBarOrSymbol();
        }

        private void LateUpdate()
        {
            if (this.playLow && this.values.currentState != CharacterState.dead)
            {
                if (elapsed <= 0)
                {
                    elapsed = audioInterval;
                    AudioUtil.playSoundEffect(this.lowSoundEffect);
                }
                else
                {
                    elapsed -= Time.deltaTime;
                }
            }
        }

        private void setValues()
        {
            if (this.resourceType == CostType.life)
            {
                this.maxValue = this.values.maxLife;
                this.currentValue = this.values.life;
            }
            else
            {
                this.maxValue = this.values.maxMana;
                this.currentValue = this.values.mana;
            }
        }

        private void setStatusBar()
        {
            setValues();

            for (int i = 0; i < this.icons.Count; i++)
            {
                this.icons[i].SetActive(false);
                if (i + 1 <= this.maxValue) this.icons[i].SetActive(true);
            }
        }

        private void setBarValues()
        {
            this.fillBar.fillAmount = (float)(this.currentValue / this.maxValue);
            this.textFieldBar.text = this.currentValue.ToString("N1") + " / " + this.maxValue.ToString("N1");
        }

        private void setSizeAndPositionBar(Image image, float value)
        {
            float temp = value * 100;
            float offset = (this.maxLength - temp) / 2;
            image.transform.GetComponent<RectTransform>().sizeDelta = new Vector2(temp, 100);
            image.transform.position = new Vector2(image.transform.position.x - offset, image.transform.position.y);
        }

        public void setBarOrSymbol()
        {
            this.bar.SetActive(false);
            this.symbol.SetActive(false);

            if (this.resourceType == CostType.life) setLayout(MasterManager.settings.healthBar);
            else if (this.resourceType == CostType.mana) setLayout(MasterManager.settings.manaBar);
        }

        private void setLayout(bool value)
        {
            if (value) this.bar.SetActive(true);
            else this.symbol.SetActive(true);
        }

        #endregion


        #region Update Signal Funktionen (Life, Mana, StatusEffects)    
        public void UpdateGUIHealthMana()
        {
            setStatusBar();
            setBarValues();

            Sprite sprite = null;

            for (int i = 0; i < (int)this.maxValue; i++)
            {
                if (i <= this.currentValue - 1)
                {
                    sprite = this.full;
                }
                else if (i <= this.currentValue - 0.75f)
                {
                    sprite = this.quarterhalf;
                }
                else if (i <= this.currentValue - 0.5f)
                {
                    sprite = this.half;
                }
                else if (i <= this.currentValue - 0.25f)
                {
                    sprite = this.quarter;
                }
                else
                {
                    sprite = this.empty;
                }

                try
                {
                    Transform temp = this.icons[i].transform;
                    temp.GetComponent<Image>().sprite = sprite;
                }
                catch (Exception ex)
                {
                    string temp = ex.ToString();
                }
            }

            if (this.currentValue <= this.schwelle && this.values.currentState != CharacterState.dead)
            {
                this.playLow = true;
                if (this.warning != null) this.warning.SetActive(true);
            }
            else
            {
                this.playLow = false;
                if (this.warning != null) this.warning.SetActive(false);
            }

        }
        #endregion
    }
}