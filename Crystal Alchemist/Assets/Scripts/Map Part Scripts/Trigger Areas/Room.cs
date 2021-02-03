using Cinemachine;
using Sirenix.OdinInspector;
using UnityEngine;

namespace CrystalAlchemist
{
    public class Room : MonoBehaviour
    {
        [BoxGroup("Area")]
        [Required]
        [SerializeField]
        private CinemachineVirtualCamera virtualCamera;

        [BoxGroup("Area")]
        [SerializeField]
        private bool deactivate = false;

        [ShowIf("deactivate")]
        [BoxGroup("Area")]
        [SerializeField]
        private GameObject objectsInArea;

        [BoxGroup("Map")]
        [SerializeField]
        [Required]
        private StringValue stringValue;

        [BoxGroup("Map")]
        [SerializeField]
        private string localisationID;

        private void Awake()
        {
            SetObjects(false);
            this.virtualCamera.gameObject.SetActive(false);            
        }

        private void SetObjects(bool value)
        {
            if (this.objectsInArea != null && deactivate) this.objectsInArea.SetActive(value);
        }

        private void OnTriggerEnter2D(Collider2D other)
        {
            if (!other.isTrigger && NetworkUtil.IsLocal(other.GetComponent<Player>()))
            {
                SetObjects(true);
                this.virtualCamera.gameObject.SetActive(true);

                this.stringValue.SetValue(this.localisationID);
                SettingsEvents.current.DoLanguageChange();
            }
        }

        private void OnTriggerExit2D(Collider2D other)
        {
            if (!other.isTrigger && this.virtualCamera != null && NetworkUtil.IsLocal(other.GetComponent<Player>()))
            {
                SetObjects(false);
                this.virtualCamera.gameObject.SetActive(false);
            }
        }
    }
}