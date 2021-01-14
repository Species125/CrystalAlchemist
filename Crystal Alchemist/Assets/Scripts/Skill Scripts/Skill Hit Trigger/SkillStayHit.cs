using Sirenix.OdinInspector;
using System.Collections.Generic;
using UnityEngine;

namespace CrystalAlchemist
{
    public class SkillStayHit : SkillHitTrigger
    {
        [InfoBox("Hits Character after staying a certain amount of time in the collider")]
        [BoxGroup("Mechanics")]
        [SerializeField]
        private float minTime = 0f;

        private Dictionary<Character, float> listOfCharacters = new Dictionary<Character, float>();

        private void Update()
        {
            foreach (Character character in this.listOfCharacters.Keys)
            {
                float time = this.listOfCharacters[character];

                if (time > 0) this.listOfCharacters[character] -= (Time.deltaTime * this.skill.getTimeDistortion());
                else
                {
                    this.skill.hitIt(character);
                    this.listOfCharacters[character] = minTime;
                }
            }
        }

        private void OnTriggerEnter2D(Collider2D hittedCharacter)
        {
            if (CollisionUtil.checkCollision(hittedCharacter, this.skill))
            {
                Character character = hittedCharacter.GetComponent<Character>();
                if (character != null)
                {
                    if (!this.listOfCharacters.ContainsKey(character)) this.listOfCharacters.Add(character, this.minTime);
                }
            }
        }

        private void OnTriggerExit2D(Collider2D hittedCharacter)
        {
            if (CollisionUtil.checkCollision(hittedCharacter, this.skill))
            {
                Character character = hittedCharacter.GetComponent<Character>();
                if (character != null && this.listOfCharacters.ContainsKey(character)) this.listOfCharacters.Remove(character);
            }
        }
    }
}
