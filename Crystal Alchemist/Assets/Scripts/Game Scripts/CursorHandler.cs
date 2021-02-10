using UnityEngine;
using UnityEngine.InputSystem;

namespace CrystalAlchemist
{
    public class CursorHandler : MonoBehaviour
    {
        [SerializeField]
        private float timeVisible = 3f;

        private PlayerInputs inputs;
        private float countdown;


        private void Awake()
        {
            inputs = new PlayerInputs();
            inputs.Controls.MouseMovement.performed += MouseMovement;
        }

        private void Start() => GameEvents.current.OnDeviceChanged += OnDeviceChanged;

        private void OnDestroy() => GameEvents.current.OnDeviceChanged -= OnDeviceChanged;

        private void OnEnable() => inputs.Enable();

        private void OnDisable() => inputs.Disable();

        private void OnDeviceChanged()
        {
            if (MasterManager.inputDeviceInfo.type == InputDeviceType.gamepad) Cursor.visible = false;
        }

        private void MouseMovement(InputAction.CallbackContext ctx)
        {
            this.countdown = this.timeVisible;
            Cursor.visible = true;
        }

        private void FixedUpdate()
        {
            if (this.countdown > 0) this.countdown -= Time.fixedDeltaTime;
            else
            {
                this.countdown = 0;
                Cursor.visible = false;
            }
        }
    }
}
