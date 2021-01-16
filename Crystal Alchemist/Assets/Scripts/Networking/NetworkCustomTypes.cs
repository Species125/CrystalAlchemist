using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using ExitGames.Client.Photon;

namespace CrystalAlchemist
{
    public class NetworkCustomTypes : MonoBehaviour
    {
        private void Awake()
        {
            //PhotonPeer.RegisterType(typeof(SkillAffections), NetworkUtil.SKILL_AFFECTIONS, SkillAffections.Serialize, SkillAffections.Deserialize);
        }
    }
}