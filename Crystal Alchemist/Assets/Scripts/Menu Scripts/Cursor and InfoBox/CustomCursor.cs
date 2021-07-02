using UnityEngine;
using UnityEngine.UI;

namespace CrystalAlchemist
{
    public class CustomCursor : MonoBehaviour
    {
        [SerializeField]
        private AudioClip soundEffect;
        //private bool isPlaying = false;

        public InfoBox infoBox;

        [SerializeField]
        private Image image;

        [SerializeField]
        private GameObject cursor;

        [SerializeField]
        private GameObject cursorSelected;

        private Vector2 cursorScale = Vector2.one;
        private Vector2 cursorSize;
        private float offset = 16;
        private int distance = 400;
        private RectTransform rect;
        public Selectable selectedObject;


        private void Awake()
        {
            this.image.sprite = null;
            this.cursorSelected.SetActive(false);
            this.cursor.SetActive(true);

            this.getCursorSize();
        }

        private void getCursorSize()
        {
            RectTransform rt = (RectTransform)this.cursor.transform;
            this.cursorSize = new Vector2(rt.rect.width, rt.rect.height);
            this.cursorScale = rt.lossyScale;
        }

        public void Select(bool playEffect, Selectable selectable)
        {
            if (selectable != null && this.selectedObject != selectable)
            {
                this.selectedObject = selectable;
                SetPositionToSelectable(playEffect);

                setInfoBox(selectable);
            }
        }

        public void SetPositionToSelectable(bool playEffect)
        {
            Selectable selectable = this.selectedObject;
            this.rect = (RectTransform)selectable.transform;

            if (selectable.GetComponent<Scrollbar>() != null) this.rect = selectable.GetComponent<Scrollbar>().handleRect;
            if (selectable.GetComponent<Slider>() != null) this.rect = selectable.GetComponent<Slider>().handleRect;

            UpdatePosition();
            if (playEffect) playSoundEffect();
        }

        public void SetTransform(RectTransform rect) => this.rect = rect;

        public void UpdateRect()
        {
            Selectable selectable = this.selectedObject;
            this.rect = (RectTransform)selectable.transform;

            UpdatePosition();
        }

        public void UpdatePosition()
        {
            if (this.rect == null) return;
            Vector2 size = new Vector2(rect.rect.width, rect.rect.height);
            Vector2 scale = rect.lossyScale;

            float x_new = (((size.x * scale.x) + (this.cursorSize.x * this.cursorScale.x)) / 2) - this.offset;
            float y_new = (((size.y * scale.y) + (this.cursorSize.y * this.cursorScale.y)) / 2) - this.offset;

            this.transform.position = new Vector2(rect.position.x - (x_new),
                rect.position.y + (y_new));
        }

        private void setInfoBox(Selectable button)
        {
            if (button != null)
            {
                button.Select();

                if (this.infoBox != null)
                {
                    if (this.transform.localPosition.x < 0)
                    {
                        //right
                        RectTransform panelRectTransform = (RectTransform)this.infoBox.transform;
                        panelRectTransform.anchorMin = new Vector2(1, 0);
                        panelRectTransform.anchorMax = new Vector2(1, 0);
                        panelRectTransform.pivot = new Vector2(0.5f, 0.5f);
                        panelRectTransform.anchoredPosition = new Vector3(-175, 275);
                    }
                    else
                    {
                        //left
                        RectTransform panelRectTransform = (RectTransform)this.infoBox.transform;
                        panelRectTransform.anchorMin = new Vector2(0, 0);
                        panelRectTransform.anchorMax = new Vector2(0, 0);
                        panelRectTransform.pivot = new Vector2(0.5f, 0.5f);
                        panelRectTransform.anchoredPosition = new Vector3(175,275);
                    }

                    ItemUI itemUI = button.gameObject.GetComponent<ItemUI>();
                    SkillSlot skillSlot = button.gameObject.GetComponent<SkillSlot>();
                    SkillMenuActiveSlots activeSlot = button.gameObject.GetComponent<SkillMenuActiveSlots>();
                    CharacterAttributeStats attributesStat = button.gameObject.GetComponent<CharacterAttributeStats>();

                    if (itemUI != null && itemUI.GetInventoryItem() != null)
                    {
                        this.infoBox.Show(itemUI.GetInventoryItem());
                    }
                    else if (itemUI != null && itemUI.GetItemStat() != null)
                    {
                        this.infoBox.Show(itemUI.GetItemStat());
                    }
                    else if (itemUI != null && itemUI.GetItemDrop() != null)
                    {
                        this.infoBox.Show(itemUI.GetItemDrop());
                    }
                    else if (skillSlot != null && skillSlot.ability != null)
                    {
                        this.infoBox.Show(skillSlot.ability);
                    }
                    else if (activeSlot != null && activeSlot.ability != null)
                    {
                        this.infoBox.Show(activeSlot.ability);
                    }
                    else if (attributesStat != null)
                    {
                        this.infoBox.Show(attributesStat);
                    }
                    else
                    {
                        this.infoBox.Hide();
                    }
                }
            }
        }

        public void setSelectedGameObject(Sprite sprite)
        {
            if (sprite == null)
            {
                this.image.sprite = null;
                this.cursorSelected.SetActive(false);
                this.cursor.SetActive(true);
            }
            else
            {
                this.image.sprite = sprite;
                this.cursorSelected.SetActive(true);
                this.cursor.SetActive(false);
            }
        }

        public void playSoundEffect()
        {
            AudioUtil.playSoundEffect(this.soundEffect);
        }
    }
}
