using Sirenix.OdinInspector;
using UnityEngine;
using Photon.Pun;

namespace CrystalAlchemist
{
    public class SkillTeleport : SkillExtension
    {
        [Required]
        [SerializeField]
        private SkillSceneTransition summon;

        [SerializeField]
        private PlayerTeleportList teleports;

        public override void Initialize()
        {
            if (!NetworkUtil.IsLocal(this.skill.sender.GetComponent<Player>())) return;

            object[] path = new object[] { this.teleports.GetReturnTeleport().path };
            PhotonNetwork.Instantiate(this.summon.path, this.transform.position, Quaternion.identity, 0, path);
        }
    }
}