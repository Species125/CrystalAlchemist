using System.Collections.Generic;
using UnityEngine;

namespace CrystalAlchemist
{
    public class PathNode
    {
        public int x;
        public int y;
        private Vector2 position;
        private bool isWalkable;
        private bool canModifyWalkable = true;
        private GameObject owner = null;

        public int gCost;
        private int hCost;
        public int fCost;

        public PathNode previousNode;
        private float cellSize;
        private float radius;

        private const int straightCost = 10;
        private const int diagonalCost = 14;

        public PathNode(int x, int y, float cellSize, Vector2 origin)
        {
            this.x = x;
            this.y = y;
            this.cellSize = cellSize;

            this.position = new Vector2(this.x, this.y) * this.cellSize + origin;
            this.isWalkable = true;
        }

        public void Initialize(LayerMask layerMask, float radius)
        {
            this.gCost = int.MaxValue;
            this.CalculateFCost();
            this.previousNode = null;
            this.radius = radius;

            SetWalkable(layerMask);
        }

        public void SetWalkable(LayerMask layerMask)
        {
            SetWalkable(layerMask, null, null);
        }

        public void SetWalkable(LayerMask layerMask, Collider2D collider, GameObject gameObject)
        {
            if (this.canModifyWalkable)
            {
                ContactFilter2D filter = new ContactFilter2D();
                filter.layerMask = layerMask;
                filter.useLayerMask = true;
                filter.useTriggers = false;

                List<Collider2D> colls = new List<Collider2D>();
                int result = Physics2D.OverlapCircle(this.GetVector(), GetRadius(), filter, colls);

                if (result > 0)
                {
                    if (collider == null) SetObstacle(); //static collision, cannot be modified
                    else if (colls.Contains(collider) && this.owner == null) SetDynamicObstacle(gameObject); //dynamic collision, can be modified    
                }
                else if (this.owner == gameObject) SetDynamicObstacle(null);
            }
        }

        private void SetObstacle()
        {
            this.isWalkable = false;
            this.canModifyWalkable = false;
        }

        private void SetDynamicObstacle(GameObject gameObject)
        {
            if (gameObject == null) this.isWalkable = true;
            else this.isWalkable = false;
            this.owner = gameObject;
        }

        public void SetStartNode(PathNode endNode)
        {
            this.gCost = 0;
            this.hCost = CalculateHCost(endNode);
            this.CalculateFCost();
        }

        public void SetNeighbornode(PathNode currentNode, PathNode endNode, int tentativeGCost)
        {
            this.previousNode = currentNode;
            this.gCost = tentativeGCost;
            this.hCost = CalculateHCost(endNode);
            this.CalculateFCost();
        }

        public void CalculateFCost()
        {
            this.fCost = this.gCost + this.hCost;
        }

        public int CalculateHCost(PathNode node)
        {
            int xDistance = Mathf.Abs(this.x - node.x);
            int yDistance = Mathf.Abs(this.y - node.y);
            int remaining = Mathf.Abs(xDistance - yDistance);
            int minimum = Mathf.Min(xDistance, yDistance);
            return diagonalCost * minimum + straightCost * remaining;
        }

        public int CalculateTentativeGCost(PathNode neighbor)
        {
            return this.gCost + this.CalculateHCost(neighbor);
        }

        public PathNode GetPreviousNode()
        {
            return this.previousNode;
        }

        public Vector2 GetVector()
        {
            return this.position + Vector2.one * this.cellSize * 0.5f;
        }

        public Vector2 GetSize()
        {
            return Vector2.one * this.cellSize;
        }

        public float GetRadius()
        {
            return (this.cellSize * this.radius * 0.5f);
        }

        public bool getWalkable()
        {
            return this.isWalkable;
        }

        public bool getWalkable(GameObject gameObject)
        {
            if (this.owner == gameObject) return true; //walk trough its own unwalkable node
            else return this.isWalkable; //normal behaviour
        }
    }
}
