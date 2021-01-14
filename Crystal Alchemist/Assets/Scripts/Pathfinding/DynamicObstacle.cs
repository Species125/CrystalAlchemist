using Sirenix.OdinInspector;
using UnityEngine;

namespace CrystalAlchemist
{
    public class DynamicObstacle : MonoBehaviour
    {
        [SerializeField]
        [InfoBox("Determine on which layer it shows as obstacle")]
        private GraphType graphType = GraphType.ground;

        [SerializeField]
        [MinValue(0.1f)]
        [MaxValue(10f)]
        [Tooltip("How often the grid will be updated")]
        private float updateInterval = 0.2f;

        private PathfindingGrid grid;

        private void Start()
        {
            if (Pathfinding.Instance != null)
            {
                this.grid = Pathfinding.Instance.GetGrid(this.graphType);
                InvokeRepeating("SetNode", 0.5f, this.updateInterval);
            }
        }

        private void SetNode()
        {
            if (this.grid != null) this.grid.UpdateGrid(this.gameObject, this.GetComponent<Collider2D>());
        }
    }
}
