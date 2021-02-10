using UnityEngine;

namespace CrystalAlchemist
{
    public class CustomCursorHandler : MonoBehaviour
    {
        [SerializeField]
        private GameObject child;

        private void OnEnable() => OnDeviceChanged();

        private void Start() 
        { 
            GameEvents.current.OnDeviceChanged += OnDeviceChanged;
            OnDeviceChanged();
        }

        private void OnDestroy() => GameEvents.current.OnDeviceChanged -= OnDeviceChanged;
        
        private void OnDeviceChanged() => this.child.SetActive(MasterManager.inputDeviceInfo.type == InputDeviceType.gamepad);
       
    }
}
