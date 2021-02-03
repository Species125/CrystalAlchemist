using Sirenix.OdinInspector;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace CrystalAlchemist
{
    public class MiniDialogBox : MonoBehaviour
    {
        [SerializeField]
        private GameObject bubble;

        [SerializeField]
        private GameObject text;

        [SerializeField]
        private TextMeshProUGUI textfield;

        [SerializeField]
        private Image image;

        [SerializeField]
        private Vector2Int max;

        [SerializeField]
        private Vector2Int space;

        [SerializeField]
        private Vector2Int offset;


        private float duration;
        private Vector2 position;

        private void Start()
        {
            this.transform.position = this.position;
            SetSize();
            CheckFlip();
            if (duration > 0) Destroy(this.gameObject, this.duration);
        }

        [Button]
        private void CheckFlip()
        {
            float pos = Camera.main.WorldToScreenPoint(this.transform.position).x;

            if (pos > Screen.currentResolution.width / 2) Flip(true);
            else Flip(false);
        }

        private void Flip(bool value)
        {
            RectTransform rt = (RectTransform)this.image.transform;
            float rotation = 180;
            float position = -rt.sizeDelta.x;

            if (!value)
            {
                rotation = 0;
                position = 0;
            }

            this.bubble.transform.localRotation = Quaternion.Euler(0, rotation, 0);
            this.text.transform.localPosition = new Vector2(position, 0);
        }

        public void setDialogBox(string text, float duration, Vector2 position)
        {
            this.duration = duration;
            this.textfield.text = text;
            this.position = position;
        }

        [Button]
        private void SetSize()
        {
            int length = this.textfield.text.Length;
            int lines = 1;

            if (length > max.x)
            {
                lines = 1 + (length / max.x);
                length = max.x;
            }

            if (lines > max.y) lines = max.y;

            float y = (lines * space.y) + offset.y;
            float x = (length * space.x) + offset.x;

            RectTransform rt = (RectTransform)this.image.transform;
            rt.sizeDelta = new Vector2(x + 25, y + 50);

            rt = (RectTransform)this.textfield.transform;
            rt.sizeDelta = new Vector2(x, y);
        }
    }
}
