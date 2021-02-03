using UnityEngine;
using UnityEngine.Events;

namespace CrystalAlchemist
{
    [CreateAssetMenu(menuName = "Game/Menu/Menu Dialogbox Info")]
    public class MenuDialogBoxInfo : ScriptableObject
    {
        public UnityEvent OnConfirm;
        public Costs costs;
        public string text;
        public DialogBoxType type;
        public GameObject parent;
        public CustomCursor cursor;

        public void SetValue(UnityEvent OnConfirm, CustomCursor cursor, Costs costs, string text, DialogBoxType type, GameObject parent)
        {
            this.OnConfirm = OnConfirm;
            this.costs = costs;
            this.text = text;
            this.type = type;
            this.parent = parent;
            this.cursor = cursor;
        }

        public void Clear()
        {
            this.OnConfirm = null;
            this.costs = null;
            this.text = "";
            this.type = DialogBoxType.ok;
            this.parent = null;
            this.cursor = null;
        }
    }
}
