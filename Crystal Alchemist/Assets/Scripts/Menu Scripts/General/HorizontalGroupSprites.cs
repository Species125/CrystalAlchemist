using UnityEngine;

namespace CrystalAlchemist
{
    public class HorizontalGroupSprites : MonoBehaviour
    {
        [SerializeField]
        private float padding;

        private int lastChildCount;

        void Update()
        {
            if (this.lastChildCount != this.transform.childCount)
            {
                this.lastChildCount = this.transform.childCount;
                SetLayout();
            }
        }

        private void SetLayout()
        {
            for (int i = 0; i < this.transform.childCount; i++)
            {
                Transform child = this.transform.GetChild(i);
                float x = child.transform.position.x + (i * this.padding);
                float y = child.transform.position.y;

                child.transform.position = new Vector3(x, y, 1);
            }
        }
    }
}
