using Sirenix.OdinInspector;
using System.Collections.Generic;
using UnityEngine;

namespace CrystalAlchemist
{
    public enum ShareType
    {
        less,
        exact,
        more
    }

    public class SkillStackHit : SkillHitTrigger
    {
        [InfoBox("Hits Character depending on how many characters are in the same collider")]
        [BoxGroup("Mechanics")]
        [SerializeField]
        private int amountNeeded;

        [BoxGroup("Mechanics")]
        [SerializeField]
        private ShareType type;

        //TODO: Show amount as points

        private List<Character> listOfCharacters = new List<Character>();

        public void CalculateDamage()
        {
            if ((this.type == ShareType.exact && GetAmount() == this.amountNeeded)
                || (this.type == ShareType.less && GetAmount() <= this.amountNeeded)
                || (this.type == ShareType.more && GetAmount() >= this.amountNeeded)) this.skill.SetPercentage(0);
            else this.skill.SetPercentage(100);
        }

        public int GetAmount()
        {
            return this.listOfCharacters.Count;
        }

        private void OnTriggerEnter2D(Collider2D hittedCharacter)
        {
            if (CollisionUtil.checkCollision(hittedCharacter, this.skill))
            {
                Character character = hittedCharacter.GetComponent<Character>();
                if (character != null)
                {
                    if (!this.listOfCharacters.Contains(character)) this.listOfCharacters.Add(character);
                }
            }
        }

        private void OnTriggerExit2D(Collider2D hittedCharacter)
        {
            if (CollisionUtil.checkCollision(hittedCharacter, this.skill))
            {
                Character character = hittedCharacter.GetComponent<Character>();
                if (character != null && this.listOfCharacters.Contains(character)) this.listOfCharacters.Remove(character);
            }
        }
    }
}