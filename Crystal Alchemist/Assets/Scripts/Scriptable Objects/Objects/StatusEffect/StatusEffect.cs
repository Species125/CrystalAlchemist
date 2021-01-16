using AssetIcons;
using Sirenix.OdinInspector;
using System.Collections.Generic;
using UnityEngine;

namespace CrystalAlchemist
{
    public enum StatusEffectType
    {
        buff,
        debuff
    }

    [CreateAssetMenu(menuName = "Game/StatusEffect")]
    public class StatusEffect : NetworkScriptableObject
    {
        #region Attribute
        [FoldoutGroup("Basis Attribute")]
        public bool hasDuration = true;

        [FoldoutGroup("Basis Attribute")]
        [ShowIf("hasDuration")]
        [MinValue(1)]
        public float maxDuration = 1;

        [FoldoutGroup("Basis Attribute")]
        public bool canOverride = false;

        [FoldoutGroup("Basis Attribute")]
        public bool canBeModified = true;

        [FoldoutGroup("Basis Attribute")]
        public bool canDeactivateIt = false;

        [FoldoutGroup("Basis Attribute")]
        public bool canBeDispelled = true;

        [FoldoutGroup("Basis Attribute")]
        public bool ignoreTimeDistortion = false;

        [FoldoutGroup("Basis Attribute")]
        [Tooltip("Anzahl der maximalen gleichen Effekte (Stacks)")]
        [MinValue(1)]
        public float maxStacks = 1;

        [FoldoutGroup("Basis Attribute")]
        [Tooltip("Ist der Charakter betäubt?")]
        public bool stunTarget = false;

        [FoldoutGroup("Basis Attribute")]
        [Tooltip("Handelt es sich um einen positiven oder negativen Effekt?")]
        public StatusEffectType statusEffectType = StatusEffectType.debuff;

        [FoldoutGroup("Trigger and Actions", expanded: false)]
        public List<StatusEffectEvent> statusEffectEvents = new List<StatusEffectEvent>();

        [FoldoutGroup("Visuals", expanded: false)]
        [Tooltip("Farbe während der Dauer des Statuseffekts")]
        [SerializeField]
        private float destroyDelay = 0.2f;

        [FoldoutGroup("Visuals", expanded: false)]
        [Tooltip("Farbe während der Dauer des Statuseffekts")]
        [SerializeField]
        private bool changeColor = true;

        [FoldoutGroup("Visuals", expanded: false)]
        [Tooltip("Farbe während der Dauer des Statuseffekts")]
        [SerializeField]
        [ShowIf("changeColor")]
        [ColorUsage(true, true)]
        private Color statusEffectColor;

        [FoldoutGroup("Visuals", expanded: false)]
        [Tooltip("Farbe während der Dauer des Statuseffekts")]
        [SerializeField]
        private bool invertColor;

        [FoldoutGroup("Visuals", expanded: false)]
        [SerializeField]
        [Required]
        private StatusEffectGameObject statusEffectObject;

        [FoldoutGroup("Visuals", expanded: false)]
        [Tooltip("Icon des Statuseffekts für das UI")]
        [AssetIcon]
        public Sprite iconSprite;

        private Character target;
        private float statusEffectTimeLeft;
        private float timeDistortion = 1;
        private StatusEffectGameObject activeObject;
        #endregion


        #region Start Funktionen (Init)
        public void Initialize(Character character)
        {
            this.target = character;
            setTime();
            initActions();
        }

        public StatusEffectGameObject GetVisuals()
        {
            return this.statusEffectObject;
        }

        public Color GetColor()
        {
            return this.statusEffectColor;
        }

        public bool CanChangeColor()
        {
            return this.changeColor;
        }

        public bool CanInvertColor()
        {
            return this.invertColor;
        }

        public StatusEffectGameObject Instantiate(GameObject parent)
        {
            StatusEffectGameObject effect = Instantiate(this.statusEffectObject, parent.transform.position, Quaternion.identity, parent.transform);
            effect.name = this.statusEffectObject.name;
            effect.Initialize(this);
            this.activeObject = effect;
            return effect;
        }

        public void UpdateTimeDistortion(float distortion)
        {
            if (!this.ignoreTimeDistortion) this.timeDistortion = 1 + (distortion / 100);
        }

        private void setTime()
        {
            this.statusEffectTimeLeft = this.maxDuration;

            if (this.canBeModified && this.target.stats.canChangeBuffs)
            {
                float percentage = 0;
                if (this.statusEffectType == StatusEffectType.buff) percentage = (float)this.target.values.buffPlus;
                else percentage = (float)this.target.values.debuffMinus;

                this.statusEffectTimeLeft *= ((100f + (float)percentage) / 100f);
            }
        }

        #endregion


        #region Update

        public void Updating(Character character)
        {
            if (this.target != character) this.target = character;
            doOnUpdate();
        }

        private void doOnUpdate()
        {
            GameEvents.current.DoStatusEffectUpdate();

            if (this.hasDuration && this.statusEffectTimeLeft > 0)
            {
                this.statusEffectTimeLeft -= (Time.deltaTime * this.timeDistortion);
            }

            updateActions();
            if (this.statusEffectTimeLeft <= 0) DestroyIt();
        }

        private void updateActions()
        {
            foreach (StatusEffectEvent effectEvent in this.statusEffectEvents)
            {
                effectEvent.Updating(this.timeDistortion);
                effectEvent.DoEvents(this.target, this);
            }
        }

        private void initActions()
        {
            foreach (StatusEffectEvent effectEvent in this.statusEffectEvents) effectEvent.Initialize(this.target, this);
        }

        public void DestroyIt()
        {
            if (this.target != null)
            {
                //Charakter-Farbe zurücksetzen
                if (this.changeColor) this.target.removeColor(this.statusEffectColor);
                if (this.invertColor) this.target.InvertColor(false);
                this.resetValues();
            }

            DoModuleDestroy();
            if (this.activeObject != null) this.activeObject.Deactivate();

            //GUI updaten und Objekt kurz danach zerstören
            GameEvents.current.DoStatusEffectUpdate();
            Destroy(this, this.destroyDelay);
        }

        private void resetValues()
        {
            foreach (StatusEffectEvent buffEvent in this.statusEffectEvents) buffEvent.ResetEvent(this.target, this);
        }

        public void changeTime(float extendTimePercentage)
        {
            this.statusEffectTimeLeft += (this.statusEffectTimeLeft * extendTimePercentage) / 100;
        }

        public float getTimeLeft()
        {
            return this.statusEffectTimeLeft;
        }

        public Character GetTarget()
        {
            return this.target;
        }

        public string GetName()
        {
            return FormatUtil.GetLocalisedText(this.name + "_Name", LocalisationFileType.statuseffects);
        }

        public string GetDescription()
        {
            return FormatUtil.GetLocalisedText(this.name + "_Description", LocalisationFileType.statuseffects);
        }

        public void doModule()
        {
            if (this.statusEffectObject.GetComponent<StatusEffectModule>() != null) this.statusEffectObject.GetComponent<StatusEffectModule>().DoAction();
        }

        public void DoModuleDestroy()
        {
            if (this.statusEffectObject.GetComponent<StatusEffectModule>() != null) this.statusEffectObject.GetComponent<StatusEffectModule>().DoDestroy();
        }

        #endregion
    }
}