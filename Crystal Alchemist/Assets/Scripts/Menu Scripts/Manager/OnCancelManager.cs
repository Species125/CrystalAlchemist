using Sirenix.OdinInspector;
using System.Collections;
using System.Linq;
using UnityEngine;
using UnityEngine.Events;

namespace CrystalAlchemist
{
    public class OnCancelManager : MonoBehaviour
    {
        [BoxGroup]
        [SerializeField]
        private UnityEvent onCancel;

        private bool inputPossible = false;

        private void Start()
        {
            if (GameEvents.current != null) GameEvents.current.OnCancel += OnCancel;
            if (TitleScreenControls.current != null) TitleScreenControls.current.OnCancel += OnCancel;
        }

        private void OnEnable()
        {
            StartCoroutine(delayCo());
        }

        private void OnDestroy()
        {
            if (GameEvents.current != null) GameEvents.current.OnCancel -= OnCancel;
            if (TitleScreenControls.current != null) TitleScreenControls.current.OnCancel -= OnCancel;
        }

        public void OnCancel()
        {
            if (this.gameObject.activeInHierarchy
                && this.onCancel != null
                && this.inputPossible
                && temp()) this.onCancel.Invoke();
        }

        private bool temp()
        {
            return (MasterManager.globalValues.openedMenues.Count <= 1
                    || MasterManager.globalValues.openedMenues.Last() == this.gameObject);
        }

        private IEnumerator delayCo()
        {
            this.inputPossible = false;
            yield return new WaitForSeconds(0.1f);
            this.inputPossible = true;
        }
    }
}
