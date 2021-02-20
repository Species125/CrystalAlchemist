using UnityEngine;
using UnityEngine.Rendering;

namespace CrystalAlchemist
{
    public class MultilevelChange : MonoBehaviour
    {
        [SerializeField]
        private int newPhysics;

        [SerializeField]
        private string newSorting;

        [SerializeField]
        private int newSortingOrder = 1;

        private void OnTriggerEnter2D(Collider2D collision)
        {
           if (collision.isTrigger) return;

            GameObject other = collision.gameObject;
            other.layer = this.newPhysics;

            SortingGroup temp = other.GetComponentInChildren<SortingGroup>();

            if (temp != null) 
            {
                temp.sortingLayerName = newSorting;
                temp.sortingOrder = newSortingOrder;
            }            
        }        
    }
}
