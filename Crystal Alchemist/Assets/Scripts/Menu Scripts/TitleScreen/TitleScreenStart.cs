using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;

namespace CrystalAlchemist
{
    public class TitleScreenStart : MonoBehaviour
    {
        [SerializeField]
        private UnityEvent OnAnyButtonPressEvent;

        [SerializeField]
        private float inputDelay = 0.3f;

        private bool inputPossible = false;

        private void Start()
        {
            SaveSystem.LoadOptions();
            StartInputDelay();
        }

        private void StartInputDelay()
        {
            this.inputPossible = false;
            Invoke("StopInputDelay", this.inputDelay);
        }

        private void StopInputDelay()
        {
            this.inputPossible = true;
        }

        private void Update()
        {
            if (Input.anyKeyDown && this.inputPossible)
            {
                this.OnAnyButtonPressEvent?.Invoke();
                this.inputPossible = false;
            }
        }
    }
}
