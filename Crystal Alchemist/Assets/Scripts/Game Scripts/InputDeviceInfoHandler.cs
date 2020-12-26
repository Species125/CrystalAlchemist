using UnityEngine.InputSystem;
using UnityEngine;

public class InputDeviceInfoHandler : MonoBehaviour
{
    private void Start() => InputSystem.onActionChange += OnActionChange;       

    private void OnDestroy() => InputSystem.onActionChange -= OnActionChange; 

    private void OnActionChange(object obj, InputActionChange change)
    {
        if (change == InputActionChange.ActionPerformed)
        {
            var inputAction = (InputAction)obj;     
            var lastControl = inputAction.activeControl;
            var lastDevice = lastControl.device;

            object value = inputAction.ReadValueAsObject().ToString();
            if(value.ToString() != "(0.0, 0.0)")
               MasterManager.inputDeviceInfo.SetDevice(lastDevice.displayName, lastControl.name);
        }
    }
}
