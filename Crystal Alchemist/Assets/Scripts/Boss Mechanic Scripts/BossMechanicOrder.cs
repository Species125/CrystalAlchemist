using System.Collections.Generic;
using UnityEngine;

namespace CrystalAlchemist
{
    public class BossMechanicOrder : MonoBehaviour
    {
        [SerializeField]
        private float delay;

        private List<GameObject> children = new List<GameObject>();
        private float elapsed;

        private void Awake()
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
                int random = Random.Range(0, this.children.Count);
                this.children[random].SetActive(true);
                this.children.RemoveAt(random);

                elapsed = delay;
            }
            else elapsed -= Time.deltaTime;
        }
    }
}
