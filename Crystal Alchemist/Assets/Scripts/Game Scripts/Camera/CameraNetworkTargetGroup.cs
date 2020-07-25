using UnityEngine;
using Cinemachine;

[RequireComponent(typeof(CinemachineTargetGroup))]
public class CameraNetworkTargetGroup : MonoBehaviour
{
    private CinemachineTargetGroup[] groups;

    [SerializeField]
    private GameObject placeHolder;

    private void Start()
    {
        GameEvents.current.OnStart += AddPlayer;
        this.groups = this.GetComponents<CinemachineTargetGroup>();
    }
    
    private void OnDestroy() => GameEvents.current.OnStart -= AddPlayer;
    
    private void AddPlayer(GameObject gameObject)
    {
        for(int i = 0; i < groups.Length; i++)
        {
            CinemachineTargetGroup group = groups[i];
            CinemachineTargetGroup.Target[] targets = group.m_Targets;

            for(int j = 0; j < targets.Length; j++)
            {
                CinemachineTargetGroup.Target target = targets[j];                

                if (target.target == placeHolder.transform)
                {
                    group.RemoveMember(placeHolder.transform);
                    group.AddMember(gameObject.transform, 1f, 0f);
                    break;
                }
            }
        }
    }
}
