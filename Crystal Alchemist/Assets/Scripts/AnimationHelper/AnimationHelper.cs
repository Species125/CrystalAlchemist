using UnityEngine;

namespace CrystalAlchemist
{
    public class AnimationHelper : MonoBehaviour
    {
        [SerializeField] private GameObject mainObject;

        private void Start()
        {
            if (this.mainObject == null) this.mainObject = this.gameObject;
        }

        public void PlaySoundEffect(AudioClip audioClip) => AudioUtil.playSoundEffect(this.mainObject, audioClip);

        public void DestroyIt()
        {
            if (this.mainObject.GetComponent<Skill>() != null) this.mainObject.GetComponent<Skill>().DestroyIt();
            else if (this.mainObject.GetComponent<Character>() != null)
                this.mainObject.GetComponent<Character>().DestroyIt();
            else Destroy(this.mainObject);
        }

        public void SetActive()
        {
            this.mainObject.SetActive(true);
        }

        public void SetNotActive()
        {
            this.mainObject.SetActive(false);
        }

        public void EnableTreasure()
        {
            if (this.mainObject.GetComponent<Treasure>() != null)
                this.mainObject.GetComponent<Treasure>().SetEnabled(true);
        }

        public void DisableTreasure()
        {
            if (this.mainObject.GetComponent<Treasure>() != null)
                this.mainObject.GetComponent<Treasure>().SetEnabled(false);
        }

        public void PlayTreasureSoundEffect()
        {
            if (this.mainObject.GetComponent<Treasure>() != null)
                this.mainObject.GetComponent<Treasure>().PlayTreasureSoundEffect();
        }

        public void PlayTreasureMusic()
        {
            if (this.mainObject.GetComponent<Treasure>() != null)
                this.mainObject.GetComponent<Treasure>().PlayTreasureMusic();
        }

        public void ShowTreasureItem()
        {
            if (this.mainObject.GetComponent<Treasure>() != null)
                this.mainObject.GetComponent<Treasure>().ShowTreasureItem();
        }

        public void ResetRotation() => this.transform.rotation = Quaternion.identity;
    }
}
