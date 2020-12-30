using UnityEngine;
using Sirenix.OdinInspector;


[RequireComponent(typeof(SpriteRenderer))]
public class TargetingIndicator : MonoBehaviour
{
    private Character sender;
    private Character target;

    [SerializeField]
    [Required]
    private SpriteRenderer spriteRenderer;

    [SerializeField]
    private CustomRenderer customRenderer;

    public virtual void Initialize(Character sender, Character target)
    {
        this.sender = sender;
        this.target = target;
    }

    public virtual void SetColor(Color color)
    {
        if (this.customRenderer != null && color != null) this.customRenderer.SetGlowColor(color, true);
        else if (this.spriteRenderer != null && color != null) this.spriteRenderer.color = color;
    }

    public virtual void SetSprite(Sprite sprite)
    {
        if (this.spriteRenderer != null) this.spriteRenderer.sprite = sprite;
    }

    public SpriteRenderer GetSpriteRenderer()
    {
        return this.spriteRenderer;
    }

    public Character GetSender()
    {
        return this.sender;
    }

    public Character GetTarget()
    {
        return this.target;
    }

    public virtual void Start()
    {
        
    }

    public virtual void Update()
    {

    }

    public void DestroyIt()
    {
        Destroy(this.gameObject);
    }
}
