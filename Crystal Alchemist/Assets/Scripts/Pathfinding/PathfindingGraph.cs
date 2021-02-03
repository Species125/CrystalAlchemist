
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.Tilemaps;

namespace CrystalAlchemist
{
    public enum GraphType
    {
        ground,
        flying
    }

    [System.Serializable]
    public class PathfindingGraph
    {
        [BoxGroup("Grid")]
        [Tooltip("Name of the graph")]
        public GraphType graphType = GraphType.ground;

        [SerializeField]
        [Required]
        [BoxGroup("Grid")]
        [Tooltip("Required to get the size of the grid")]
        [InfoBox("Tilemap Composite Collider2D works best as polygons")]
        private Tilemap tileMap;

        [SerializeField]
        [ShowIf("tileMap")]
        [BoxGroup("Grid")]
        [Tooltip("Size of Cells")]
        private float cellsize = 1f;

        [SerializeField]
        [ShowIf("tileMap")]
        [BoxGroup("Grid")]
        [Tooltip("Diameter to add collisions into the grid")]
        [Range(0, 1)]
        private float diameter = 1f;

        [SerializeField]
        [ShowIf("tileMap")]
        [BoxGroup("Grid")]
        [Tooltip("Searching for Collisions within filter")]
        private LayerMask filter;

        [SerializeField]
        [BoxGroup("Debug")]
        public bool showDebug = true;

        [SerializeField]
        [BoxGroup("Debug")]
        [ShowIf("showDebug")]
        public bool showDiameter = false;

        [SerializeField]
        [BoxGroup("Debug")]
        [ShowIf("showDebug")]
        private Color gridColor = Color.white;

        [SerializeField]
        [BoxGroup("Debug")]
        [ShowIf("showDebug")]
        private Color notWalkable = Color.yellow;

        private PathfindingGrid grid;

        public PathfindingGrid GetGrid()
        {
            return this.grid;
        }

        public void Initialize()
        {
            this.grid = new PathfindingGrid(this.tileMap, this.cellsize, this.filter, this.diameter);
        }

        public void ShowDebug()
        {
            if (this.showDebug) GizmoUtil.PathfinderGizmo(this.grid, this.gridColor, this.notWalkable, this.showDiameter);
        }
    }
}