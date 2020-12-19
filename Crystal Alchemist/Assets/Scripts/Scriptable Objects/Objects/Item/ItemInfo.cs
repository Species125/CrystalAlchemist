using UnityEngine;
using Sirenix.OdinInspector;
using AssetIcons;

[CreateAssetMenu(menuName = "Game/Items/Item Info")]
public class ItemInfo : ScriptableObject
{    
    [BoxGroup("Icon")]
    [SerializeField]
    [AssetIcon]
    private Sprite icon;    

    public void SetInfo(Sprite sprite) => this.icon = sprite;

    public Sprite getSprite()
    {
        return this.icon;
    }

    public string getDescription()
    {
        return FormatUtil.GetLocalisedText(this.name+"_Description", LocalisationFileType.items);
    }

    public string getName()
    {
        return FormatUtil.GetLocalisedText(this.name + "_Name", LocalisationFileType.items);
    }
}
