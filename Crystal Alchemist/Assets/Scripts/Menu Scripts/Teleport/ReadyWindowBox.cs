using UnityEngine;
using UnityEngine.UI;
using TMPro;

namespace CrystalAlchemist
{
    public class ReadyWindowBox : MonoBehaviour
    {
        private bool isReady = false;
        private string playerName;
        public int ID;

        [SerializeField]
        private TextMeshProUGUI nameLabel;

        [SerializeField]
        private Image ready;

        private void Start()
        {
            this.nameLabel.text = this.playerName;
            UpdateBox(false);
        }

        public void SetBox(string name, int ID)
        {
            this.playerName = name;
            this.ID = ID;
        }

        public bool GetReady()
        {
            return this.isReady;
        }

        public void UpdateBox(bool isReady)
        {
            this.isReady = isReady;
            this.ready.gameObject.SetActive(this.isReady);
        }
    }
}
