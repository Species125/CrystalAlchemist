


using UnityEngine;

namespace CrystalAlchemist
{
    public class SkillSummon : SkillExtension
    {
        [SerializeField]
        private Character summon;

        public override void Initialize()
        {
            summoning();
        }

        public string getPetName()
        {
            return this.summon.GetCharacterName();
        }

        private void summoning()
        {
            AI ai = this.summon.GetComponent<AI>();
            Breakable breakable = this.summon.GetComponent<Breakable>();

            if (ai != null)
            {
                AI pet = Instantiate(ai, this.transform.position, Quaternion.Euler(0, 0, 0));
                pet.name = ai.name;
                pet.values.direction = this.skill.GetDirection();
                pet.partner = this.skill.sender;
                pet.InitializeAddSpawn();

                this.skill.sender.values.activePets.Add(pet);
            }
            else if (breakable != null)
            {
                Breakable objectPet = Instantiate(breakable, this.transform.position, Quaternion.Euler(0, 0, 0));
                objectPet.values.direction = this.skill.GetDirection();
                objectPet.ChangeDirection(objectPet.values.direction);
                objectPet.InitializeAddSpawn();
            }
        }
    }
}
