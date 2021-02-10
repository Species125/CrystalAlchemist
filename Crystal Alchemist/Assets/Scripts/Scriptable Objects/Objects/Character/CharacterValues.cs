using Sirenix.OdinInspector;
using System.Collections.Generic;
using UnityEngine;

namespace CrystalAlchemist
{
    public enum CharacterState
    {
        walk,
        interact, //in Reichweite eines interagierbaren Objektes
        inMenu, //Pause oder Inventar ist offen
        knockedback, //im Knockback
        idle,
        dead,
        manually,
        respawning,
        sleeping
    }

    [CreateAssetMenu(menuName = "Game/Characters/Character Values")]
    public class CharacterValues : ScriptableObject
    {
        [BoxGroup]
        public CharacterType characterType;

        [BoxGroup("Base Stats")]
        public float life;
        [BoxGroup("Base Stats")]
        public float mana;
        [Space(10)]
        [BoxGroup("Base Stats")]
        public float spellspeed;
        [BoxGroup("Base Stats")]
        public float speed;

        [BoxGroup("Base Attributes")]
        public float maxLife;
        [BoxGroup("Base Attributes")]
        public float maxMana;
        [Space(10)]
        [BoxGroup("Base Attributes")]
        public float lifeRegen;
        [BoxGroup("Base Attributes")]
        public float manaRegen;
        [Space(10)]
        [BoxGroup("Base Attributes")]
        public int buffPlus;
        [BoxGroup("Base Attributes")]
        public int debuffMinus;

        [BoxGroup("States")]
        public CharacterState currentState;
        [BoxGroup("States")]
        public bool cantBeHit; //delay
        [BoxGroup("States")]
        public bool cannotDie = false;
        [BoxGroup("States")]
        public bool isInvincible = false; //event

        [BoxGroup("States")]
        public Vector2 direction;
        [BoxGroup("States")]
        public bool lockAnimation = false;
        [BoxGroup("States")]
        public float timeDistortion = 1;
        [BoxGroup("States")]
        public float steps = 0;
        [BoxGroup("States")]
        public bool isOnIce = false;
        [BoxGroup("States")]
        public bool isAttacking = false;

        [BoxGroup("Debug")]
        [ReadOnly]
        public List<StatusEffect> buffs = new List<StatusEffect>();
        [BoxGroup("Debug")]
        [ReadOnly]
        public List<StatusEffect> debuffs = new List<StatusEffect>();
        [BoxGroup("Debug")]
        [ReadOnly]
        public List<Character> activePets = new List<Character>();
        [BoxGroup("Debug")]
        [ReadOnly]
        public List<Skill> activeSkills = new List<Skill>();
        [BoxGroup("Debug")]
        [ReadOnly]
        public ItemDrop itemDrop;
        [BoxGroup("Debug")]
        [ReadOnly]
        public float speedFactor = 5;
        [BoxGroup("Debug")]
        [ReadOnly]
        [ColorUsage(true, true)]
        public Color effectColor = Color.white;

        [Button]
        public void Clear(CharacterStats stats)
        {
            this.isInvincible = stats.isInvincible;
            this.cannotDie = stats.cannotDie;

            this.maxLife = stats.maxLife;
            this.maxMana = stats.maxMana;
            this.lifeRegen = stats.lifeRegeneration;
            this.manaRegen = stats.manaRegeneration;
            this.buffPlus = stats.buffPlus;
            this.debuffMinus = stats.debuffMinus;

            this.characterType = stats.GetCharacterType();
            this.life = stats.startLife;
            this.mana = stats.startMana;
            this.buffs.Clear();
            this.debuffs.Clear();
            this.speed = (stats.startSpeed / 100) * this.speedFactor;
            this.timeDistortion = 1;

            this.cantBeHit = false;
            this.isOnIce = false;

            if (stats.lootTable != null) this.itemDrop = stats.lootTable.GetItemDrop();
        }

        public void Initialize()
        {
            this.currentState = CharacterState.idle;
            //this.activeSkills.RemoveAll(x => x = null);
            //for (int i = 0; i < this.activeSkills.Count; i++) this.activeSkills[i].DeactivateIt();
            this.activeSkills.Clear();
            this.activePets.Clear();
        }

        #region Menu und DialogBox

        public bool IsAlive()
        {
            return (this.currentState != CharacterState.respawning
                    && this.currentState != CharacterState.dead);
        }

        public bool CanOpenMenu()
        {
            return (this.currentState != CharacterState.inMenu
                    && this.currentState != CharacterState.knockedback
                    && IsAlive());
        }

        public bool CanMove()
        {
            return (CanOpenMenu() 
                    && this.currentState != CharacterState.sleeping 
                    && !this.isCharacterStunned());
        }

        public bool CanUseAbilities()
        {
            if (      this.currentState != CharacterState.interact
                    && this.currentState != CharacterState.inMenu
                    //&& this.currentState != CharacterState.knockedback
                    && this.currentState != CharacterState.sleeping
                    && !this.isCharacterStunned()
                    && IsAlive()) return true;
            return false;
        }

        /// <summary>
        /// Is Alive, not in menu, no knockback, not stunned and no attacking
        /// </summary>
        /// <returns></returns>
        public bool CanInteract()
        {
            return (!this.isAttacking && CanMove());
        }


        public bool isCharacterStunned()
        {
            this.debuffs.RemoveAll(item => item == null);

            foreach (StatusEffect debuff in this.debuffs)
            {
                if (debuff != null && debuff.stunTarget) return true;
            }

            return false;
        }

        public void AddStatusEffect(StatusEffect effect)
        {
            if (effect.statusEffectType == StatusEffectType.debuff) this.debuffs.Add(effect);
            else this.buffs.Add(effect);
        }

        public bool HasStatusEffects(List<StatusEffectRequired> effects)
        {
            int count = 0;

            for (int i = 0; i < effects.Count; i++)
            {
                if (HasStatusEffect(effects[i])) count++;
            }

            return count == effects.Count;
        }

        private bool HasStatusEffect(StatusEffectRequired required)
        {
            List<StatusEffect> effects = this.debuffs;
            if (required.effect.statusEffectType == StatusEffectType.buff) effects = this.buffs;

            int found = 0;

            for (int i = 0; i < effects.Count; i++)
            {
                if (effects[i].name == required.effect.name) found++;
            }

            if ((required.type == ShareType.exact && found == required.stacks)
                || (required.type == ShareType.less && found <= required.stacks)
                || (required.type == ShareType.more && found >= required.stacks)) return true;

            return false;
        }

        #endregion
    }
}