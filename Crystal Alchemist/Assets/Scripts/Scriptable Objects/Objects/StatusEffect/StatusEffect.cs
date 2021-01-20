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

    public enum StatusEffectMode
    {
        none,
        overrideIt,
        destroyIt
    }

    [CreateAssetMenu(menuName = "Game/StatusEffect")]
    public class StatusEffect : NetworkScriptableObject
    {
        [FoldoutGroup("Basis Attribute")]
        [Tooltip("Handelt es sich um einen positiven oder negativen Effekt?")]
        public StatusEffectType statusEffectType = StatusEffectType.debuff;

        #region Attribute
        [FoldoutGroup("Basis Attribute")]
        public bool hasDuration = true;

        [FoldoutGroup("Basis Attribute")]
        [ShowIf("hasDuration")]
        [MinValue(1)]
        public float maxDuration = 1;

        [FoldoutGroup("Basis Attribute")]
        [Tooltip("Update if the same effect is applied")]
        public StatusEffectMode mode = StatusEffectMode.none;

        [FoldoutGroup("Basis Attribute")]
        [Tooltip("Anzahl der maximalen gleichen Effekte (Stacks)")]
        [MinValue(1)]
        public float maxStacks = 1;


        [FoldoutGroup("Basis Attribute Detail")]
        public bool canBeModified = true;

        [FoldoutGroup("Basis Attribute Detail")]
        public bool canBeDispelled = true;

        [FoldoutGroup("Basis Attribute Detail")]
        public bool ignoreTimeDistortion = false;

        [FoldoutGroup("Basis Attribute Detail")]
        [Tooltip("Ist der Charakter betäubt?")]
        public bool stunTarget = false;


        [FoldoutGroup("Trigger and Actions", expanded: false)]
        public List<StatusEffectEvent> statusEffectEvents = new List<StatusEffectEvent>();



        [FoldoutGroup("Visuals", expanded: false)]
        [Tooltip("Farbe während der Dauer des Statuseffekts")]
        [SerializeField]
        private float destroyDelay = 0.2f;

        [FoldoutGroup("Visuals", expanded: false)]
        [Tooltip("Farbe während der Dauer des Statuseffekts")]
        public bool changeColor = true;

        [FoldoutGroup("Visuals", expanded: false)]
        [Tooltip("Farbe während der Dauer des Statuseffekts")]
        [ShowIf("changeColor")]
        [ColorUsage(true, true)]
        public Color statusEffectColor;

        [FoldoutGroup("Visuals", expanded: false)]
        [Tooltip("Farbe während der Dauer des Statuseffekts")]
        public bool invertColor;

        [FoldoutGroup("Visuals", expanded: false)]
        [SerializeField]
        private StatusEffectGameObject statusEffectObject;

        [FoldoutGroup("Visuals", expanded: false)]
        [Tooltip("Icon des Statuseffekts für das UI")]
        [AssetIcon]
        public Sprite iconSprite;

        private Character target;
        private float statusEffectTimeLeft;
        private float timeDistortion = 1;
        #endregion


        #region Start Funktionen (Init)
        public void Initialize(Character character)
        {
            this.target = character;
            setTime();
            initActions();
        }

        public string GetVisualsName()
        {
            if(this.statusEffectObject != null) return this.statusEffectObject.name;
            return string.Empty;
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

        public void AddVisuals(GameObject parent)
        {
            if (this.statusEffectObject == null) return;
            StatusEffectGameObject activeVisuals = Instantiate(this.statusEffectObject, parent.transform.position, Quaternion.identity, parent.transform);
            activeVisuals.name = this.statusEffectObject.name;
            activeVisuals.Initialize(this);
            this.target.statusEffectVisuals.Add(activeVisuals);
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
                this.target.RemoveStatusEffectVisual(this);                
                this.resetValues();
            }

            DoModuleDestroy();           
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

        public void SetTimeLeft(float time) => this.statusEffectTimeLeft = time;

        public Character GetTarget()
        {
            return this.target;
        }

        public void SetTarget(Character target)
        {
            if(this.target == null) this.target = target;
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
            if (this.statusEffectObject != null && this.statusEffectObject.GetComponent<StatusEffectModule>() != null) 
                this.statusEffectObject.GetComponent<StatusEffectModule>().DoAction();
        }

        public void DoModuleDestroy()
        {
            if (this.statusEffectObject != null && this.statusEffectObject.GetComponent<StatusEffectModule>() != null) 
                this.statusEffectObject.GetComponent<StatusEffectModule>().DoDestroy();
        }

        #endregion
    }
}