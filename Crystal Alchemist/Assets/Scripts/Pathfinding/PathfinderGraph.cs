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
        [Tooltip("Is required to determine the size of the pathfinder grind")]
        [Required]
        private Tilemap tilemap;

        [BoxGroup("Pathfinding")]
        [Min(0)]
        [Tooltip("The size of a cell of the grid")]
        public float cellSize = 1;

        [Space(10)]
        [BoxGroup("Collision")]
        [SerializeField]
        [Tooltip("Physics Layer to set which nodes are not walkable by the collision (map collision, walls)")]
        private LayerMask collisionLayerMask;

        [BoxGroup("Collision")]
        [SerializeField]
        [Tooltip("The accuarcy of the collision for the not walkable nodes")]
        [Min(0)]
        private float collisionRadius = 0.5f;

        [Space(10)]
        [BoxGroup("Dynamic Obstacles")]
        [SerializeField]
        [Tooltip("The physics layer for all objects with a dynamic obstacle node (for enemies or NPCs)")]
        private LayerMask dynamicLayerMask;

        [BoxGroup("Dynamic Obstacles")]
        [SerializeField]
        [Tooltip("The accuarcy of the collision for the not walkable nodes")]
        [Min(0)]
        private float dynamicRadius = 0.25f;

        [BoxGroup("Dynamic Obstacles")]
        [SerializeField]
        [Tooltip("The interval how often the the dynamic obstacles will be updated")]
        [Min(0.1f)]
        private float dynamicInterval = 1;

        [HideInInspector]
        public int width;

        [HideInInspector]
        public int height;

        [HideInInspector]
        public Vector2 position;

        [HideInInspector]
        public PathNode[] grid = new PathNode[1];

        private void Awake() => Initialize();

        public void Initialize()
        {
            if (this.tilemap == null || this.cellSize <= 0) return;
            InitGrid(new int2(this.width, this.height));
            Updating();
        }

        private void Start()
        {
            InvokeRepeating("Updating", 0, this.dynamicInterval);
        }

        public void Updating()
        {
            for (int i = 0; i < this.grid.Length; i++)
            {
                PathNode node = this.grid[i];
                if (!node.isDynamic) continue;

                ContactFilter2D filter = new ContactFilter2D();
                filter.layerMask = dynamicLayerMask;
                filter.useLayerMask = true;
                filter.useTriggers = false;

                List<Collider2D> colls = new List<Collider2D>();
                Vector2 pos = PathfinderUtil.GetVector(node.GetInt2(), this);
                int result = Physics2D.OverlapCircle(pos, this.dynamicRadius, filter, colls); ;

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
                    filter.layerMask = this.collisionLayerMask;
                    filter.useLayerMask = true;
                    filter.useTriggers = false;

                    List<Collider2D> colls = new List<Collider2D>();
                    Vector2 pos = PathfinderUtil.GetVector(new int2(x, y), this);
                    int result = Physics2D.OverlapCircle(pos, collisionRadius, filter, colls);

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
