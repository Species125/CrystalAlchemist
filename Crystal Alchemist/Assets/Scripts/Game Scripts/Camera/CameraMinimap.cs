using Sirenix.OdinInspector;
using UnityEngine;

[RequireComponent(typeof(Camera))]
public class CameraMinimap : MonoBehaviour
{
    private Camera cam;
    public Shader shader;

    private void Start()
    {
        this.cam = this.GetComponent<Camera>();
        this.cam.SetReplacementShader(shader, "");        
    }
}
