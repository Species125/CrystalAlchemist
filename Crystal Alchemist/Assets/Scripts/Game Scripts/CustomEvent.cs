using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace CrystalAlchemist
{
    public class CustomEvent : MonoBehaviour
    {
        [SerializeField]
        private UnityEvent events;

        public void CallEvents()
        {
            this.events.Invoke();
        }
    }
}
