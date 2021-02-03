using System.Collections;



using UnityEngine;

namespace CrystalAlchemist
{
    public class TitleScreenStart : MonoBehaviour
    {
        [SerializeField]
        private GameObject mainMenu;

        [SerializeField]
        private GameObject anyKey;

        [SerializeField]
        private PlayerSaveGame saveGame;

        [SerializeField]
        private TimeValue timeValue;

        [SerializeField]
        private float delay = 0.3f;

        private bool inputPossible = false;

        private void Awake()
        {
            this.mainMenu.SetActive(false);
            StartCoroutine(this.delayInput());
        }

        private void Start()
        {
            SaveSystem.loadOptions();
            this.timeValue.Clear();
            this.saveGame.Clear();
        }

        private void Update()
        {
            if (Input.anyKeyDown && inputPossible) Invoke("showMenuCo", 0.1f);
        }

        private IEnumerator delayInput()
        {
            this.anyKey.SetActive(false);
            this.inputPossible = false;
            yield return new WaitForSeconds(this.delay);
            this.inputPossible = true;
            this.anyKey.SetActive(true);
        }

        private void showMenuCo()
        {
            this.gameObject.SetActive(false);
            this.mainMenu.SetActive(true);
        }
    }
}
