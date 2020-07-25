using UnityEngine;
using AssetIcons;

[CreateAssetMenu(menuName = "Game/Player/Teleport Stats")]
public class TeleportStats : ScriptableObject
{
    public string teleportName = "";
    public string scene;
    public Vector2 position;
    public bool showAnimationIn = false;
    public bool showAnimationOut = false;
    public bool isEmpty = true;

    [AssetIcon]
    public Sprite icon;

    public void Clear()
    {
        this.teleportName = "";
        this.scene = null;
        this.position = Vector2.zero;
        this.showAnimationIn = true;
        this.showAnimationOut = false;
        this.isEmpty = true;
    }

    public bool Exists(string name)
    {
        return this.teleportName == name;
    }

    public void SetValue(string teleportName, string targetScene, Vector2 position, bool showIn, bool showOut, Sprite icon)
    {
        if (targetScene != null && targetScene != "")
        {
            this.teleportName = teleportName;
            this.scene = targetScene;
            this.position = position;
            this.showAnimationIn = showIn;
            this.showAnimationOut = showOut;
            this.icon = icon;
            this.isEmpty = false;
        }
        else Clear();
    }

    public string GetTeleportName()
    {
        return FormatUtil.GetLocalisedText(this.teleportName, LocalisationFileType.maps);
    }
}
