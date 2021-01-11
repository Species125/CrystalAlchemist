using UnityEngine;
using Sirenix.OdinInspector;

[RequireComponent(typeof(SpriteRenderer))]
public class CustomRenderer : MonoBehaviour
{
    [HideInInspector]
    public Material material;

    [HideInInspector]
    public SpriteRenderer spriteRenderer;

    [SerializeField]
    private bool useGlow = false;

    [ShowIf("useGlow")]
    [SerializeField]
    [ColorUsage(true, true)]
    private Color glowColor = Color.white;

    [SerializeField]
    private bool invert = false;

    private void Start()
    {
        this.spriteRenderer = this.GetComponent<SpriteRenderer>();
        this.material = this.GetComponent<SpriteRenderer>().material;
        AddGlow();
    }

    public void InvertColors(bool invert)
    {
        this.material.SetFloat("_Invert", invert ? 1f : 0f);
    }

    private void AddGlow()
    {
        this.material.SetFloat("_UseGlow", this.useGlow ? 1f : 0f);
        this.material.SetColor("_GlowColor", this.glowColor);        
    }

    [Button]
    public virtual void Test()
    {
        SpriteRenderer spriteRenderer = this.GetComponent<SpriteRenderer>();
        Material mat = spriteRenderer.sharedMaterial;
        mat.SetFloat("_Invert", invert ? 1f : 0f);
        mat.SetFloat("_UseGlow", this.useGlow ? 1f : 0f);
        mat.SetColor("_GlowColor", this.glowColor);
    }

    [Button]
    public virtual void Clear()
    {
        SpriteRenderer spriteRenderer = this.GetComponent<SpriteRenderer>();
        Material mat = spriteRenderer.sharedMaterial;
        mat.SetFloat("_Invert", 0f);
        mat.SetFloat("_UseGlow", 0f);
        mat.SetColor("_GlowColor", Color.black);
    }
}
