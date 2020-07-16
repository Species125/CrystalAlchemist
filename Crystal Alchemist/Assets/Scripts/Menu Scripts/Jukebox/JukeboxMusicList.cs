using System.Collections.Generic;
using UnityEngine;

public class JukeboxMusicList : MonoBehaviour
{
    [SerializeField]
    private List<MusicTheme> themes = new List<MusicTheme>();

    [SerializeField]
    private JukeboxButton template;

    [SerializeField]
    private GameObject content;

    private void OnEnable()
    {
        this.template.gameObject.SetActive(false);

        for (int i = 0; i < themes.Count; i++)
        {
            JukeboxButton newButton = Instantiate(template, this.content.transform);
            newButton.gameObject.SetActive(true);
            newButton.SetMusic(themes[i]);
            newButton.name = "Item " + i + ":" + newButton.GetTheme().name;
        }
    }
}
