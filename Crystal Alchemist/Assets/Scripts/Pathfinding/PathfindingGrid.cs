using UnityEngine;
using UnityEngine.Tilemaps;

namespace CrystalAlchemist
{
    public class PathfindingGrid
    {
        private int width;
        private int height;
        private float cellSize;

        public PathNode[,] gridArray;

        private Vector2 position;
        private LayerMask filter;
        private float diameter;

        public PathfindingGrid(Tilemap tilemap, float cellSize, LayerMask filter, float diameter)
        {
            if (cellSize > 0 && tilemap != null)
            {
                this.width = (int)(tilemap.size.x / cellSize);
                this.height = (int)(tilemap.size.y / cellSize);
                this.position = tilemap.localBounds.min + tilemap.transform.position;
                this.cellSize = cellSize;
                this.filter = filter;
                this.diameter = diameter;

                InitializeGrid();
                InitializeNodes();
            }
        }

        private void InitializeGrid()
        {
            this.gridArray = new PathNode[width, height];

            //HEAVY
            for (int x = 0; x < this.gridArray.GetLength(0); x++)
            {
                for (int y = 0; y < this.gridArray.GetLength(1); y++)
                {
                    this.gridArray[x, y] = new PathNode(x, y, this.cellSize, this.position);
                }
            }
        }

        public void InitializeNodes()
        {
            //Initialize all nodes
            //HEAVY
            for (int x = 0; x < this.gridArray.GetLength(0); x++)
            {
                for (int y = 0; y < this.gridArray.GetLength(1); y++)
                {
                    PathNode node = this.GetNode(x, y);
                    node.Initialize(this.filter, this.diameter);
                }
            }
        }

        public void UpdateGrid(GameObject gameObject, Collider2D collider)
        {
            //HEAVY
            for (int x = 0; x < this.gridArray.GetLength(0); x++)
            {
                for (int y = 0; y < this.gridArray.GetLength(1); y++)
                {
                    LayerMask mask = (1 << gameObject.layer);
                    PathNode node = this.GetNode(x, y);
                    node.SetWalkable(mask, collider, gameObject);
                }
            }
        }

        public PathNode GetNode(int x, int y)
        {
            return this.gridArray[x, y];
        }

        public PathNode GetNode(Vector2 position)
        {
            int x = Mathf.FloorToInt((position - this.position).x / this.cellSize);
            int y = Mathf.FloorToInt((position - this.position).y / this.cellSize);
            return GetNode(x, y);
        }

        public int GetWidth()
        {
            return this.width;
        }

        public int GetHeight()
        {
            return this.height;
        }
    }
}
