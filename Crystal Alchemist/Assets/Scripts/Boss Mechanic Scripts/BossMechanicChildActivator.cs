using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;

namespace CrystalAlchemist
{
    public class BossMechanicChildActivator : MonoBehaviour
    {
        public enum Ordertype
        {
            fix,
            random
        }

        [SerializeField]
        [InfoBox("Zur verzögerten Aktivierung von GameObjects")]
        private Ordertype ordertype = Ordertype.fix;

        [SerializeField]
        private float delay;

        [SerializeField]
        private List<GameObject> children = new List<GameObject>();

        private float elapsed;

        [Button]
        private void AddChildren()
        {
            foreach (Transform child in this.transform)
            {
                child.gameObject.SetActive(false);
                children.Add(child.gameObject);
            }
        }

        private void Update()
        {
            if (this.children.Count <= 0) this.enabled = false;
            if (elapsed <= 0)
            {
                int index = 0;

                if(this.ordertype == Ordertype.random) index = Random.Range(0, this.children.Count);

                this.children[index].SetActive(true);
                this.children.RemoveAt(index);

                elapsed = delay;
            }
            else elapsed -= Time.deltaTime;
        }
    }
}
