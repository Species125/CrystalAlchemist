using Sirenix.OdinInspector;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace CrystalAlchemist
{
    public class ButtonExtension : MonoBehaviour, ISelectHandler, IPointerEnterHandler
    {
        [SerializeField]
        [Required]
        public CustomCursor cursor;

        [SerializeField]
        private bool selectFirst = false;

        [SerializeField]
        private bool overrideNavigation = false;

        [ShowIf("overrideNavigation", true)]
        [SerializeField]
        private List<Selectable> up = new List<Selectable>();

        [ShowIf("overrideNavigation", true)]
        [SerializeField]
        private List<Selectable> down = new List<Selectable>();

        [ShowIf("overrideNavigation", true)]
        [SerializeField]
        private List<Selectable> left = new List<Selectable>();

        [ShowIf("overrideNavigation", true)]
        [SerializeField]
        private List<Selectable> right = new List<Selectable>();

        private bool isInit = true;
        private Selectable selectable;

        private void OnEnable()
        {
            if (isInit) Initialize();
            ReSelect();
        }

        private void Initialize()
        {
            this.selectable = this.GetComponent<Selectable>();
            this.setButtonNavigation();
            SetColors();
            this.isInit = false;
        }

        [Button]
        public void setButtonNavigation()
        {
            if (this.overrideNavigation)
            {
                if (this.selectable != null)
                {
                    Navigation nav = this.selectable.navigation;
                    nav.mode = Navigation.Mode.Explicit;

                    nav.selectOnUp = GetNavigationNeighbor(this.up);
                    nav.selectOnDown = GetNavigationNeighbor(this.down);
                    nav.selectOnLeft = GetNavigationNeighbor(this.left);
                    nav.selectOnRight = GetNavigationNeighbor(this.right);
                    this.selectable.navigation = nav;
                }
            }
        }

        private Selectable GetNavigationNeighbor(List<Selectable> nexts)
        {
            try
            {
                foreach (Selectable next in nexts)
                {
                    if (next.gameObject.activeInHierarchy) return next;
                }
            }
            catch
            {
                Debug.LogError("Butten Extension Error at Navigation: " + this.transform.parent.gameObject.name + " - " + this.gameObject.name);
            }
            return null;
        }


        public void Select()
        {
            if (EventSystem.current != null && this.gameObject.activeInHierarchy)
            {
                EventSystem.current.firstSelectedGameObject = this.gameObject;
                EventSystem.current.SetSelectedGameObject(this.gameObject);

                SetCursor(); //CHANGED
            }            
        }

        public void OnPointerEnter(PointerEventData eventData) => Select();

        public void OnSelect(BaseEventData eventData) => SetCursor();

        public void SetCursor() => this.cursor.Select(!this.isInit, this.selectable);

        public void ReSelect()
        {
            if (this.selectFirst) Select();
        }

        public void SetInteractable(bool value) => this.selectable.interactable = value;

        public void SetAsFirst() => this.selectFirst = true;

        public void SetColors()
        {
            if (this.selectable != null)
            {
                ColorBlock colors = this.selectable.colors;
                colors.disabledColor = MasterManager.globalValues.buttonNotActive;
                colors.highlightedColor = Color.white;
                colors.selectedColor = MasterManager.globalValues.buttonSelect;
                this.selectable.colors = colors;
            }
        }
    }
}

