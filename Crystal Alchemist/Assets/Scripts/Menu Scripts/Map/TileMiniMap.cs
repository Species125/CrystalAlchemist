using UnityEngine;
using UnityEngine.Tilemaps;

namespace CrystalAlchemist
{
    public class TileMiniMap : MonoBehaviour
    {
        [SerializeField]
        private Material material;

        [SerializeField]
        private int layer = 30;

        [SerializeField]
        private GameObject source;

        private void Start()
        {
            GameObject minimap = Instantiate(this.source, this.transform.parent);

            foreach (Transform child in minimap.transform)
            {
                if (child.GetComponent<TilemapRenderer>() != null)
                {
                    child.GetComponent<TilemapRenderer>().material = this.material;
                    child.gameObject.layer = this.layer;
                }
            }
        }
    }
}
