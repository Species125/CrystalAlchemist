using System.Collections;
using UnityEngine;
using UnityEngine.Tilemaps;

namespace CrystalAlchemist
{
    [RequireComponent(typeof(Collider2D))]
    public class SecretArea : MonoBehaviour
    {
        private Tilemap map;
        [SerializeField]
        private float delay = .0025f;
        [SerializeField]
        private AudioClip secretSoundEffect;

        void Start()
        {
            this.map = GetComponent<Tilemap>();
        }

        void OnTriggerEnter2D(Collider2D collision)
        {
            if (!collision.isTrigger)
            {
                if (!NetworkUtil.IsLocal(collision.GetComponent<Player>())) return;
                AudioUtil.playSoundEffect(this.secretSoundEffect, MasterManager.settings.backgroundMusicVolume);
                StartCoroutine(FadeOut());
            }
        }

        IEnumerator FadeIn()
        {
            for (float f = .05f; f <= 1.1; f += .05f)
            {
                setColor(f);
                yield return new WaitForSeconds(this.delay);
            }
        }

        void OnTriggerExit2D(Collider2D collision)
        {
            if (!collision.isTrigger)
            {
                if (!NetworkUtil.IsLocal(collision.GetComponent<Player>())) return;
                StartCoroutine(FadeIn());
            }
        }

        IEnumerator FadeOut()
        {
            for (float f = 1f; f >= -.05f; f -= .05f)
            {
                setColor(f);
                yield return new WaitForSeconds(this.delay);
            }
        }

        private void setColor(float f)
        {            
            Color newcolor = this.map.color;
            newcolor.a = f;
            this.map.color = newcolor;
        }
    }
}
