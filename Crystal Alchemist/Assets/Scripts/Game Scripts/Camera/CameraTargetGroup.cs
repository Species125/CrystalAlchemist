using Cinemachine;
using Sirenix.OdinInspector;
using UnityEngine;

namespace CrystalAlchemist
{
    [RequireComponent(typeof(CinemachineTargetGroup))]
    public class CameraTargetGroup : MonoBehaviour
    {
        [SerializeField]
        private CinemachineTargetGroup _default;

        private CinemachineTargetGroup[] groups;
        private CinemachineTargetGroup activeGroup;

        private void Start()
        {
            this.groups = this.GetComponents<CinemachineTargetGroup>();
            foreach (CinemachineTargetGroup group in groups) group.enabled = false;

            this.activeGroup = _default;
            this.activeGroup.enabled = true;
        }

        [Button]
        public void SwitchToDefault()
        {
            if (this.activeGroup != null) this.activeGroup.enabled = false;
            this.activeGroup = _default;
            this.activeGroup.enabled = true;
        }

        [Button]
        public void SwitchToGroup(int value)
        {
            if (this.activeGroup != null) this.activeGroup.enabled = false;

            if (value < 0) value = 0;
            else if (value >= groups.Length) value = groups.Length - 1;

            this.activeGroup = this.groups[value];
            this.activeGroup.enabled = true;
        }
    }
}
