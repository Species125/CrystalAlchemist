using AssetIcons;
using UnityEngine;

namespace CrystalAlchemist
{
    [CreateAssetMenu(menuName = "Game/Player/Teleport Stats")]
    public class TeleportStats : NetworkScriptableObject
    {
        public string teleportID = "";
        public string scene;
        public Vector2 position;
        public bool showAnimationIn = false;
        public bool showAnimationOut = false;

        [AssetIcon]
        public Sprite icon;

        public void Clear()
        {
            this.teleportID = "";
            this.scene = null;
            this.position = Vector2.zero;
            this.showAnimationIn = true;
            this.showAnimationOut = false;
        }

        public bool Exists(string scene, string ID)
        {
            if (scene == null || ID == null || this.teleportID == null || this.scene == null) return false;
            return this.teleportID == ID && scene == this.scene;
        }

        public void SetValue(string teleportID, string targetScene, Vector2 position, bool showIn, bool showOut, Sprite icon)
        {
            if (targetScene != null && targetScene != "")
            {
                this.teleportID = teleportID;
                this.scene = targetScene;
                this.position = position;
                this.showAnimationIn = showIn;
                this.showAnimationOut = showOut;
                this.icon = icon;
            }
            else Clear();
        }

        public string GetFullID()
        {
            return this.scene + "_" + this.teleportID;
        }

        public string[] GetStatData()
        {
            return new string[] { this.scene, this.teleportID };
        }

        public void SetStats(TeleportStats stats)
        {
            SetValue(stats.teleportID, stats.scene, stats.position, stats.showAnimationIn, this.showAnimationOut, stats.icon);
        }

        public string GetTeleportName()
        {
            return FormatUtil.GetLocalisedText(GetFullID(), LocalisationFileType.maps);
        }
    }
}
