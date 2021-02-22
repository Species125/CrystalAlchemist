using Sirenix.OdinInspector;
using System.Collections;
using TMPro;
using UnityEngine;

namespace CrystalAlchemist
{
    public class DeathScreen : MonoBehaviour
    {
        [BoxGroup("Mandatory")]
        [Required]
        [SerializeField]
        private PlayerTeleportList playerTeleport;

        [BoxGroup("Mandatory")]
        [SerializeField]
        private BoolValue CutSceneValue;

        [BoxGroup("Music")]
        [SerializeField]
        private AudioClip deathMusic;

        [BoxGroup("Music")]
        [SerializeField]
        private float fadeIn;

        [BoxGroup("Music")]
        [SerializeField]
        private float fadeOut = 1f;

        [BoxGroup("Mandatory")]
        [SerializeField]
        private TextMeshProUGUI textField;

        [BoxGroup("Mandatory")]
        [SerializeField]
        private GameObject controls;

        [BoxGroup("Mandatory")]
        [SerializeField]
        private GameObject restart;

        [BoxGroup("Time")]
        [SerializeField]
        private float cursorDelay = 2f;

        [BoxGroup("Time")]
        [SerializeField]
        private float textDelay = 0.1f;

        private string currentText;
        private string fullText;
        private bool skip = false;

        private void Awake()
        {
            if (NetworkUtil.IsMaster()) NetworkEvents.current.SetNextTeleport(this.playerTeleport.GetLatestTeleport());

            MenuEvents.current.DoCloseMenu();
            this.fullText = this.textField.text;
            this.textField.text = "";
            this.controls.SetActive(false);
            this.restart.SetActive(false);
        }

        private void Start()
        {
            GameEvents.current.DoSaveGame(false);

            this.CutSceneValue.SetValue(true);
            GameEvents.current.DoCutScene();
            MusicEvents.current.StopMusic(this.fadeOut);
            MenuEvents.current.DoPostProcessingFade(ShowText);
            GameEvents.current.OnCancel += Skip;
        }

        private void OnDestroy() => GameEvents.current.OnCancel -= Skip;

        private void Skip() => this.skip = true;

        private void ShowText()
        {            
            MusicEvents.current.PlayMusicOnce(this.deathMusic, 0, this.fadeIn);
            StartCoroutine(this.ShowTextCo(this.textDelay));
        }

        private IEnumerator ShowTextCo(float delay)
        {
            for (int i = 0; i <= this.fullText.Length; i++)
            {
                this.currentText = this.fullText.Substring(0, i);

                if (this.skip)
                {
                    this.currentText = this.fullText;
                    i = this.fullText.Length;
                }

                this.textField.text = this.currentText;

                if (i >= this.fullText.Length)
                {
                    Invoke("EnableOptions", this.cursorDelay);
                    break;
                }

                yield return new WaitForSeconds(delay);
            }
        }

        private void EnableOptions()
        {
            if (NetworkUtil.IsSolo()) this.controls.SetActive(true);
            else this.restart.SetActive(true);
        }
    }
}
