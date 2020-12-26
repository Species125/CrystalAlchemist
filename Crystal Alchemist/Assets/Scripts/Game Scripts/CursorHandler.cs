using UnityEngine;
using UnityEngine.InputSystem;

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

    private void OnEnable() => inputs.Enable();

    private void OnDisable() => inputs.Disable();

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
