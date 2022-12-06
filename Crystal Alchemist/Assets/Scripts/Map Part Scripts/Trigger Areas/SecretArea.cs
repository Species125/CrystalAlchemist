using Sirenix.OdinInspector;
using System.Collections;
using UnityEngine;
using UnityEngine.Tilemaps;

namespace CrystalAlchemist
{
    [RequireComponent(typeof(Tilemap))]
    public class SecretArea : MonoBehaviour
    {
        [DetailedInfoBox("For fading hidden Map Areas","Add this to the Trigger2D, Tilemap is required", InfoMessageType.Info)]       
        [SerializeField]
        private float delay = .0025f;

        [SerializeField]
        private AudioClip secretSoundEffect;

        private Tilemap map;

        private void Awake()
        {
            this.map = this.GetComponent<Tilemap>();
        }

        private void OnTriggerEnter2D(Collider2D collision)
        {
            if (collision.isTrigger) return;
            if (!NetworkUtil.IsLocal(collision.GetComponent<Player>())) return;
            AudioUtil.playSoundEffect(this.secretSoundEffect, MasterManager.settings.GetMusicVolume());
            StartCoroutine(FadeOut());
        }

        IEnumerator FadeIn()
        {
            for (float f = .05f; f <= 1.1; f += .05f)
            {
                SetColor(f);
                yield return new WaitForSeconds(this.delay);
            }
        }

        private void OnTriggerExit2D(Collider2D collision)
        {
            if (collision.isTrigger) return;
            if (!NetworkUtil.IsLocal(collision.GetComponent<Player>())) return;
            StartCoroutine(FadeIn());
        }

        IEnumerator FadeOut()
        {
            for (float f = 1f; f >= -.05f; f -= .05f)
            {
                SetColor(f);
                yield return new WaitForSeconds(this.delay);
            }
        }

        private void SetColor(float f)
        {            
            Color newcolor = this.map.color;
            newcolor.a = f;
            this.map.color = newcolor;
        }
    }
}
