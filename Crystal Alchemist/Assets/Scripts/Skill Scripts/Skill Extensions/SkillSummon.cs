


using Photon.Pun;
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
            if (!NetworkUtil.IsMaster()) return;

            //TODO: Spawn

            AI ai = this.summon.GetComponent<AI>();
            Breakable breakable = this.summon.GetComponent<Breakable>();

            if (ai != null)
            {
                AI pet = PhotonNetwork.Instantiate(ai.path, this.transform.position, Quaternion.identity).GetComponent<AI>();
                pet.name = ai.name;
                pet.values.direction = this.skill.GetDirection();
                pet.partner = this.skill.sender;
                pet.InitializeAddSpawn();

                this.skill.sender.values.activePets.Add(pet);
            }
            else if (breakable != null)
            {
                Breakable objectPet = PhotonNetwork.Instantiate(breakable.path, this.transform.position, Quaternion.identity).GetComponent<Breakable>();
                objectPet.values.direction = this.skill.GetDirection();
                objectPet.ChangeDirection(objectPet.values.direction);
                objectPet.InitializeAddSpawn();
            }
        }
    }
}
