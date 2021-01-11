using AssetIcons;
using UnityEngine;
using Sirenix.OdinInspector;

[CreateAssetMenu(menuName = "Game/Items/Item Drop")]
public class ItemDrop : ScriptableObject
{
    [BoxGroup("Inspector")]
    [ReadOnly]
    public string path;

#if UNITY_EDITOR
    private void OnValidate()
    {
        this.path = UnityUtil.GetResourcePath(this);
    }
#endif

    [BoxGroup("Required")]
    [Required]
    public ItemStats stats;

    [BoxGroup("Required")]
    [Required]
    public Collectable collectable;

    [BoxGroup("Time")]
    public bool hasSelfDestruction = true;

    [BoxGroup("Time")]
    [ShowIf("hasSelfDestruction")]
    [Tooltip("Destroy Collectable Gameobject x seconds after spawn")]
    public float duration = 60f;

    [HideLabel]
    public ProgressValue progress;

    [AssetIcon]
    private Sprite GetSprite()
    {
        if(this.stats != null) return stats.getSprite();
        return null;
    }

    public ItemDrop Instantiate(int amount)
    {
        ItemDrop clone = Instantiate(this);
        clone.name = this.name;
        clone.Initialize(amount); //Set correct stats name for unique items
        return clone;
    }

    public void Initialize(int amount)
    {
        ItemStats temp = Instantiate(this.stats);
        temp.name = this.name;
        temp.Initialize(amount);
        this.stats = temp;
    }
}