using AssetIcons;
using Sirenix.OdinInspector;
using UnityEngine;

[CreateAssetMenu(menuName = "Game/Items/Shop Price")]
public class ShopPriceUI : ScriptableObject
{
    [BoxGroup("Shop Price")]
    [SerializeField]
    public Color color;

    [BoxGroup("Shop Price")]
    [SerializeField]
    public Color outline;

    [BoxGroup("Shop Price")]
    [SerializeField]
    public Sprite shopIcon;

    [AssetIcon]
    public Sprite getSprite()
    {
        if (this.shopIcon != null) return this.shopIcon;
        return null;
    }
}
