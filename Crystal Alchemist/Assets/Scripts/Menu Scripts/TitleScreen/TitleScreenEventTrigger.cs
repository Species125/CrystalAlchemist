using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace CrystalAlchemist
{
    public class TitleScreenEventTrigger : MonoBehaviour
    {
        [SerializeField]
        private List<UnityEvent> OnAnimationEnd;

        public void DoAnimationEnd(int index)
        {
            this.OnAnimationEnd[index]?.Invoke();
        }
    }
}
