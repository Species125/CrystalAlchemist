using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class QuickTravelMenu : MonoBehaviour
{
    [SerializeField]
    private PlayerTeleportList list;

    [SerializeField]
    private QuickTravelButton template;

    [SerializeField]
    private GameObject content;

    private void Start()
    {
        for(int i = 0; i < list.GetStats().Count; i++)
        {
            TeleportStats stats = list.GetStats(i);
            if (stats.scene == SceneManager.GetActiveScene().name) continue;

            QuickTravelButton newButton = Instantiate(template, this.content.transform);
            newButton.gameObject.SetActive(true);
            newButton.SetLocation(stats);
            newButton.name = stats.scene;
        }

        Destroy(this.template.gameObject);
    }
}
