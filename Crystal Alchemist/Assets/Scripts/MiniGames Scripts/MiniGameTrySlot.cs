using UnityEngine;

namespace CrystalAlchemist
{
    public class MiniGameTrySlot : MonoBehaviour
    {
        [SerializeField]
        private GameObject successMark;

        [SerializeField]
        private GameObject failMark;

        [SerializeField]
        private GameObject goldMark;

        public void reset()
        {
            this.successMark.SetActive(false);
            this.failMark.SetActive(false);
            this.goldMark.SetActive(false);
        }

        public void setAsNeccessary()
        {
            this.goldMark.SetActive(true);
        }

        public void setMark(bool success)
        {
            if (success) this.successMark.SetActive(true);
            else
            {
                this.goldMark.SetActive(false);
                this.failMark.SetActive(true);
            }
        }
    }
}
