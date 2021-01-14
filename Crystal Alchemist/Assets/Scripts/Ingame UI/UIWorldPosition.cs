
using UnityEngine;

namespace CrystalAlchemist
{
    public class UIWorldPosition : MonoBehaviour
    {
        [SerializeField]
        private GameObject parent;

        private void FixedUpdate() => UnityUtil.ScreenToWorld(this.transform, this.parent);
    }
}
