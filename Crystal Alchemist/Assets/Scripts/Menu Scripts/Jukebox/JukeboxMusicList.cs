using System.Collections.Generic;

using UnityEngine;

namespace CrystalAlchemist
{
    public class JukeboxMusicList : MonoBehaviour
    {
        [SerializeField]
        private List<MusicTheme> themes = new List<MusicTheme>();

        [SerializeField]
        private JukeboxButton template;

        [SerializeField]
        private GameObject content;

        private void Start()
        {
            for (int i = 0; i < themes.Count; i++)
            {
                JukeboxButton newButton = Instantiate(template, this.content.transform);
                newButton.gameObject.SetActive(true);
                newButton.SetMusic(themes[i]);
                newButton.name = newButton.GetTheme().name;
            }

            Destroy(this.template.gameObject);
        }
    }
}
