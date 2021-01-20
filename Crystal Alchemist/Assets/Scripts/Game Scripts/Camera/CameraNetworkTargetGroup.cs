
using Cinemachine;
using Sirenix.OdinInspector;
using UnityEngine;

namespace CrystalAlchemist
{
    [RequireComponent(typeof(CinemachineTargetGroup))]
    public class CameraNetworkTargetGroup : MonoBehaviour
    {
        private CinemachineTargetGroup[] groups;

        [InfoBox("Replaces Placeholder with Player on Start")]
        [SerializeField]
        private GameObject placeHolder;

        private void Start()
        {
            GameEvents.current.OnPlayerSpawned += AddPlayer;
            this.groups = this.GetComponents<CinemachineTargetGroup>();
        }

        private void OnDestroy() => GameEvents.current.OnPlayerSpawned -= AddPlayer;

        private void AddPlayer(int ID)
        {
            GameObject player = NetworkUtil.GetGameObject(ID);

            for (int i = 0; i < groups.Length; i++)
            {
                CinemachineTargetGroup group = groups[i];
                CinemachineTargetGroup.Target[] targets = group.m_Targets;

                for (int j = 0; j < targets.Length; j++)
                {
                    CinemachineTargetGroup.Target target = targets[j];

                    if (target.target == placeHolder.transform)
                    {
                        group.RemoveMember(placeHolder.transform);
                        group.AddMember(player.transform, 1f, 0f);
                        break;
                    }
                }
            }
        }
    }
}
