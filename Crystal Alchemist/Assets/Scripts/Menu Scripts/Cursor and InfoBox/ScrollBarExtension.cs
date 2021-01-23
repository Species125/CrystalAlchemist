using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace CrystalAlchemist
{
    public class ScrollBarExtension : MonoBehaviour, ISelectHandler
    {
        private enum ScrollBarType
        {
            horizontal,
            vertical
        }

        [SerializeField]
        private Scrollbar scrollBar;

        [SerializeField]
        private CustomCursor cursor;

        [SerializeField]
        private ScrollBarType type = ScrollBarType.vertical;

        [SerializeField]
        private int start = 2;

        [SerializeField]
        private int end = 2;

        private bool selectable = true;

        public void OnSelect(BaseEventData data)
        {
            if (MasterManager.inputDeviceInfo.type == InputDeviceType.mouse) return;
            SetOnSelect();
        }

        private void SetOnSelect()
        {
            int value = this.gameObject.transform.GetSiblingIndex();

            if (type == ScrollBarType.horizontal) SetHorizontal(value);
            else SetVertical(value);

            cursor.SetTransform((RectTransform)this.transform);
            cursor.UpdatePosition();            
        }

        private int GetCount()
        {
            int count = 0;

            foreach (Transform child in this.transform.parent.transform)
            {
                if (child.gameObject.activeInHierarchy) count++;
            }

            return count;
        }

        private void SetHorizontal(int item)
        {
            if (this.scrollBar == null) return;

            int count = GetCount();
            float index = (float)item;
            float factor = 1 / (float)(count - 1);
            float value = index * factor;

            if (index < this.start) value = 0;
            else if (item >= count - this.end) value = 1;

            this.scrollBar.value = value;
        }

        private void SetVertical(int item)
        {
            if (this.scrollBar == null) return;
            float index = (float)item;
            int count = GetCount();

            float value = 0f;

            if (index <= this.start) value = 1f;
            else if (index < count - this.end)
            {
                float factor = 1 / ((float)count - (this.end + this.start + 1));
                value = 1 - ((index - (this.start)) * factor);
            }

            this.scrollBar.value = value;
        }

        private void OnDestroy()
        {
            cursor.SetPositionToSelectable(false);
        }
    }
}
