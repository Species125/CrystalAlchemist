using Sirenix.OdinInspector;
using UnityEngine;

namespace CrystalAlchemist
{
    public enum attributeType
    {
        lifeExpander,
        manaExpander,
        lifeRegen,
        manaRegen,
        buffPlus,
        debuffMinus
    }

    [CreateAssetMenu(menuName = "Game/Player/PlayerAttributes")]
    public class PlayerAttributes : ScriptableObject
    {
        [BoxGroup("Base")]
        [SerializeField]
        private CharacterValues playerValues;

        [BoxGroup("Attributes")]
        [MinValue(0)]
        [MaxValue(4)]
        public int health;

        [MinValue(0)]
        [MaxValue(4)]
        [BoxGroup("Attributes")]
        public int mana;

        [MinValue(0)]
        [MaxValue(4)]
        [BoxGroup("Attributes")]
        public int healthRegen;

        [MinValue(0)]
        [MaxValue(4)]
        [BoxGroup("Attributes")]
        public int manaRegen;

        [MinValue(0)]
        [MaxValue(4)]
        [BoxGroup("Attributes")]
        public int buffPlus;

        [MinValue(0)]
        [MaxValue(4)]
        [BoxGroup("Attributes")]
        public int debuffMinus;

        private int[] percentageValues = new int[] { 0, 25, 50, 75, 100 };
        private int[] expanderValues = new int[] { 1, 3, 5, 7, 9 };

        [BoxGroup("Base")]
        public int pointsSpent;

        public void Clear()
        {
            this.health = 0;
            this.mana = 0;
            this.healthRegen = 0;
            this.manaRegen = 0;
            this.buffPlus = 0;
            this.debuffMinus = 0;
            this.pointsSpent = 0;
        }

        public int GetPoints(attributeType type)
        {
            switch (type)
            {
                case attributeType.lifeExpander: return this.health;
                case attributeType.lifeRegen: return this.healthRegen;
                case attributeType.manaExpander: return this.mana;
                case attributeType.manaRegen: return this.manaRegen;
                case attributeType.buffPlus: return this.buffPlus;
                case attributeType.debuffMinus: return this.debuffMinus;
            }

            return 0;
        }

        [Button]
        private void SetPoints()
        {
            SetPoints(attributeType.lifeExpander, this.health);
            SetPoints(attributeType.lifeRegen, this.healthRegen);
            SetPoints(attributeType.manaExpander, this.mana);
            SetPoints(attributeType.manaRegen, this.manaRegen);
            SetPoints(attributeType.buffPlus, this.buffPlus);
            SetPoints(attributeType.debuffMinus, this.debuffMinus);

            SetValues();
        }

        public void SetPoints(attributeType type, int points)
        {
            switch (type)
            {
                case attributeType.lifeExpander: this.health = points; break;
                case attributeType.lifeRegen: this.healthRegen = points; break;
                case attributeType.manaExpander: this.mana = points; break;
                case attributeType.manaRegen: this.manaRegen = points; break;
                case attributeType.buffPlus: this.buffPlus = points; break;
                case attributeType.debuffMinus: this.debuffMinus = points; break;
            }

            this.pointsSpent = this.health + this.mana + this.healthRegen + this.manaRegen + this.buffPlus + this.debuffMinus;
        }

        public void SetValues()
        {
            try
            {
                playerValues.maxLife = this.expanderValues[this.health];
                if (playerValues.life > playerValues.maxLife) playerValues.life = playerValues.maxLife;

                playerValues.maxMana = this.expanderValues[this.mana];
                if (playerValues.mana > playerValues.maxMana) playerValues.mana = playerValues.maxMana;

                playerValues.lifeRegen = (float)this.percentageValues[this.healthRegen] / 100f;

                playerValues.manaRegen = (float)this.percentageValues[this.manaRegen] / 100f;

                playerValues.buffPlus = this.percentageValues[this.buffPlus];

                playerValues.debuffMinus = -this.percentageValues[this.debuffMinus];
            }
            catch
            {

            }

            GameEvents.current.DoManaLifeUpdate();
        }
    }
}