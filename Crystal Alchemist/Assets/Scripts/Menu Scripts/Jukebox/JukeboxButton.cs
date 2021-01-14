using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace CrystalAlchemist
{
    public class JukeboxButton : MonoBehaviour
    {
        private MusicTheme theme;

        [SerializeField]
        private TMP_Text textField;

        [SerializeField]
        private Image image;

        public void SetMusic(MusicTheme theme)
        {
            this.theme = theme;
            this.textField.text = theme.name;
            this.image.color = theme.color;
        }

        public MusicTheme GetTheme()
        {
            return this.theme;
        }
    }
}
