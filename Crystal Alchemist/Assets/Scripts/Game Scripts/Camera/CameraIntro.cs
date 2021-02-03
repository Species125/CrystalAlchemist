using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.Events;

namespace CrystalAlchemist
{
    public class CameraIntro : MonoBehaviour
    {
        [SerializeField]
        private UnityEvent onTriggered;

        private void OnTriggerEnter2D(Collider2D collision)
        {
            if (!collision.isTrigger && NetworkUtil.IsMaster()) this.onTriggered?.Invoke();
        }
    }
}
