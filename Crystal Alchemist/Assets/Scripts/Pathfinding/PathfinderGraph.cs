using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;
using Unity.Mathematics;
using Sirenix.OdinInspector;

namespace CrystalAlchemist
{
    public class PathfinderGraph : MonoBehaviour
    {
        [BoxGroup("Pathfinding")]
        [SerializeField]
        [Required]
        private Tilemap tilemap;

        [BoxGroup("Pathfinding")]
        [SerializeField]
        private LayerMask layerMask;

        [BoxGroup("Pathfinding")]
        public float cellSize = 1;

        [BoxGroup("Pathfinding")]
        [SerializeField]
        private float diameter = 0.25f;

        [BoxGroup("Dynamic Obstacles")]
        [SerializeField]
        private LayerMask dynamicMask;

        [BoxGroup("Dynamic Obstacles")]
        [SerializeField]
        private float radius = 0.25f;

        [BoxGroup("Dynamic Obstacles")]
        [SerializeField]
        private float interval = 1;

        [HideInInspector]
        public int width;

        [HideInInspector]
        public int height;

        [HideInInspector]
        public Vector2 position;

        [HideInInspector]
        public PathNode[] grid = new PathNode[1];

        private void Awake() => Initialize();

        [Button]
        public void Initialize()
        {
            if (this.tilemap == null || this.cellSize <= 0) return;
            InitGrid(new int2(this.width, this.height));
            Updating();
        }

        private void Start()
        {
            InvokeRepeating("Updating", 0, this.interval);
        }

        public void Updating()
        {
            for (int i = 0; i < this.grid.Length; i++)
            {
                PathNode node = this.grid[i];
                if (!node.isDynamic) continue;

                ContactFilter2D filter = new ContactFilter2D();
                filter.layerMask = dynamicMask;
                filter.useLayerMask = true;
                filter.useTriggers = false;

                List<Collider2D> colls = new List<Collider2D>();
                Vector2 pos = PathfinderUtil.GetVector(node.GetInt2(), this);
                int result = Physics2D.OverlapCircle(pos, this.radius, filter, colls); ;

                if (result > 0)
                {
                    this.grid[i].owner = colls[0].gameObject.GetInstanceID();
                }
                else this.grid[i].owner = 0;

            }
        }

        private void InitGrid(int2 gridSize)
        {
            this.width = (int)(tilemap.size.x / cellSize);
            this.height = (int)(tilemap.size.y / cellSize);
            this.position = tilemap.localBounds.min + tilemap.transform.position;

            this.grid = new PathNode[gridSize.x * gridSize.y];

            for (int x = 0; x < gridSize.x; x++)
            {
                for (int y = 0; y < gridSize.y; y++)
                {
                    PathNode pathNode = new PathNode();
                    pathNode.x = x;
                    pathNode.y = y;

                    pathNode.index = CalculateIndex(x, y, gridSize.x);
                    pathNode.gCost = int.MaxValue;
                    pathNode.hCost = 0;
                    pathNode.fCost = 0;

                    pathNode.isWalkable = true;
                    pathNode.isDynamic = true;
                    pathNode.cameFromNodeIndex = -1;

                    ContactFilter2D filter = new ContactFilter2D();
                    filter.layerMask = this.layerMask;
                    filter.useLayerMask = true;
                    filter.useTriggers = false;

                    List<Collider2D> colls = new List<Collider2D>();
                    Vector2 pos = PathfinderUtil.GetVector(new int2(x, y), this);
                    int result = Physics2D.OverlapCircle(pos, diameter, filter, colls);

                    if (result > 0)
                    {
                        pathNode.isWalkable = false;
                        pathNode.isDynamic = false;
                    }

                    this.grid[pathNode.index] = pathNode;
                }
            }
        }

        private int CalculateIndex(int x, int y, int gridWidth)
        {
            return x + y * gridWidth;
        }

        private void OnDrawGizmos()
        {
            for (int i = 0; i < this.grid.Length; i++)
            {
                PathNode node = this.grid[i];
                Vector2 pos = PathfinderUtil.GetVector(node.GetInt2(), this);

                Gizmos.DrawWireCube(pos, Vector2.one * this.cellSize);

                if (!node.isWalkable)
                    Gizmos.DrawIcon(pos, "Pathfinding/NotWalkable", false, Color.red);

                if (node.owner != 0)
                    Gizmos.DrawIcon(pos, "Pathfinding/NotWalkable", false, Color.blue);
            }
        }
    }
}
