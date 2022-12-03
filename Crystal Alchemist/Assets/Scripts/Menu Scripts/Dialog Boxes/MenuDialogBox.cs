using Sirenix.OdinInspector;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace CrystalAlchemist
{
    public enum DialogBoxType
    {
        yesNo,
        ok
    }

    public class MenuDialogBox : MenuBehaviour
    {
        [BoxGroup("DialogBox")]
        [SerializeField]
        private TextMeshProUGUI textfield;

        [BoxGroup("DialogBox")]
        [SerializeField]
        private ButtonExtension YesButton;

        [BoxGroup("DialogBox")]
        [SerializeField]
        private ButtonExtension NoButton;

        [BoxGroup("DialogBox")]
        [SerializeField]
        private ButtonExtension OKButton;

        [BoxGroup("DialogBox")]
        [SerializeField]
        private MiniGamePrice priceField;

        [BoxGroup("DialogBox")]
        [SerializeField]
        private PlayerInventory inventory;

        [BoxGroup("DialogBox")]
        [SerializeField]
        private MenuDialogBoxInfo info;

        private UnityEvent OnConfirm;
        private GameObject lastMainMenu;
        private Costs price;
        private CustomCursor lastCursor;

        public override void Start()
        {
            base.Start();
            Initialize();
        }

        public override void OnDestroy()
        {
            base.OnDestroy();
        }

        private void Initialize()
        {
            this.OnConfirm = this.info.OnConfirm;
            this.lastMainMenu = this.info.parent;
            this.lastCursor = this.info.cursor;
            this.textfield.text = this.info.text;

            if (this.info.type == DialogBoxType.ok) this.OKButton.gameObject.SetActive(true);
            else
            {
                bool enabled = CanPressYes(this.info.costs);

                this.YesButton.gameObject.SetActive(true);
                this.NoButton.gameObject.SetActive(true);
                this.YesButton.SetInteractable(enabled);
            }

            this.EnableButtons(false);
        }

        private bool CanPressYes(Costs cost)
        {
            this.price = cost;

            if (this.price != null && this.price.resourceType != CostType.none)
            {
                this.priceField.gameObject.SetActive(true);
                return this.priceField.CheckPrice(this.inventory, this.price);
            }

            this.transform.localPosition = Vector2.zero;
            return true;
        }

        [Button]
        public void Yes()
        {
            if (this.OnConfirm != null)
            {
                GameEvents.current.DoReduce(this.price);
                this.OnConfirm.Invoke();
            }

            this.CloseDialog();
        }

        public void No() => this.CloseDialog();

        private void CloseDialog()
        {
            this.EnableButtons(true);
            this.ExitMenu();
        }

        public override void Cancel()
        {
            CloseDialog();
        }

        private void EnableButtons(bool value)
        {
            if (this.lastCursor != null) this.lastCursor.gameObject.SetActive(value);
            if (this.lastMainMenu == null) return;

            List<Selectable> selectables = new List<Selectable>();
            UnityUtil.GetChildObjects<Selectable>(this.lastMainMenu.transform, selectables);

            foreach (Selectable selectable in selectables)
            {
                selectable.interactable = value;

                if (value)
                {
                    ButtonExtension buttonExtension = selectable.GetComponent<ButtonExtension>();
                    if (buttonExtension != null)
                    {
                        buttonExtension.enabled = value;
                        buttonExtension.ReSelect();
                    }
                }
            }
        }
    }
}