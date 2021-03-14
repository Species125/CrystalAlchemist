using Sirenix.OdinInspector;
using System.Collections.Generic;
using UnityEngine;

namespace CrystalAlchemist
{
    public class SkillEffectModule : SkillModule
    {
        public enum RotationType
        {
            identity,
            rotate
        }

        public enum Mode
        {
            OnStart,
            OnUpdate,
            OnHit
        }

        [BoxGroup("Options")]
        [SerializeField]
        private Object effect;

        [BoxGroup("Options")]
        [SerializeField]
        private Mode mode;

        [BoxGroup("Options")]
        [SerializeField]
        private RotationType rotationType;

        [BoxGroup("Options")]
        [SerializeField]
        private bool attachToSender = false;

        [BoxGroup("Options")]
        [ShowIf("mode", Mode.OnUpdate)]
        [SerializeField]
        private float delay;

        [BoxGroup("Distances between Effects")]
        [SerializeField]
        private bool hasMaxDistance = false;

        [BoxGroup("Distances between Effects")]
        [ShowIf("hasMaxDistance")]
        [SerializeField]
        private float maxDistanceBetween = 1f;
        
        [BoxGroup("Destroy")]
        [SerializeField]
        private bool autoDestroy = false;

        [BoxGroup("Destroy")]
        [SerializeField]
        [ShowIf("autoDestroy")]
        [MinValue(0.1f)]
        private float autoDestroyAfter;

        private List<GameObject> hitPoints = new List<GameObject>();
        private float ghostDelay;

        public override void Initialize()
        {
            base.Initialize();
            if (this.mode == Mode.OnStart) SetImpactEffect(this.transform.position);
        }

        public override void Updating()
        {
            base.Updating();
            OnUpdate();
        }

        private void OnUpdate()
        {
            if (this.mode != Mode.OnUpdate) return;
            if (ghostDelay > 0) this.ghostDelay -= (Time.deltaTime * this.skill.getTimeDistortion());
            else
            {
                SetImpactEffect(this.skill.transform.position);
                this.ghostDelay = this.delay;
            }
        }

        public void OnHit(Vector2 position)
        {
            if (this.mode == Mode.OnHit) SetImpactEffect(position);
        }

        private void SetImpactEffect(Vector2 position)
        {
            if (this.effect != null)
            {
                this.hitPoints.RemoveAll(item => item == null);

                bool impactPossible = true;
                if (this.hasMaxDistance) impactPossible = UnityUtil.CheckDistances(position, this.maxDistanceBetween, this.hitPoints);

                if (impactPossible)
                {
                    if (this.effect.GetType() == typeof(GameObject))
                    {
                        Quaternion rotation = this.transform.rotation;
                        if (this.rotationType == RotationType.identity) rotation = Quaternion.identity;
                        
                        GameObject gameObject = Instantiate(this.effect, this.transform.position, rotation) as GameObject;
                        if (this.attachToSender) gameObject.transform.SetParent(this.skill.sender.transform);

                        AddToList(gameObject);
                    }
                    if (this.effect.GetType() == typeof(Ability))
                    {
                        Ability ability = Instantiate(this.effect) as Ability;

                        Skill skill = AbilityUtil.InstantiateEffectSkill(ability, position, this.skill.sender);
                        skill.transform.SetParent(this.transform);
                        Destroy(ability);
                        AddToList(skill.gameObject);
                    }
                }
            }
        }

        private void AddToList(GameObject gameObject)
        {
            hitPoints.Add(gameObject);
            if (this.autoDestroy) Destroy(gameObject, this.autoDestroyAfter);
        }
    }
}
