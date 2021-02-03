using Sirenix.OdinInspector;
using System.Collections.Generic;
using UnityEngine;

namespace CrystalAlchemist
{
    public class PathSeeker : MonoBehaviour
    {
        [SerializeField]
        [InfoBox("Determine on which layer it moves")]
        private GraphType graphType = GraphType.ground;

        public List<PathNode> openList = new List<PathNode>();
        private List<PathNode> closeList = new List<PathNode>();
        private PathfindingGrid grid;

        private void Start()
        {
            if (Pathfinding.Instance != null) this.grid = Pathfinding.Instance.GetGrid(this.graphType);
        }

        public List<Vector2> FindPath(Vector2 startPosition, Vector2 endPosition)
        {
            if (this.grid != null)
            {
                List<PathNode> path = FindPath(grid.GetNode(startPosition), grid.GetNode(endPosition));

                if (path != null)
                {
                    List<Vector2> result = new List<Vector2>();
                    foreach (PathNode node in path)
                    {
                        result.Add(node.GetVector());
                    }
                    return result;
                }
            }

            return null;
        }

        private List<PathNode> FindPath(PathNode start, PathNode end)
        {
            PathNode startNode = start;
            PathNode endNode = end;

            this.closeList.Clear();
            this.openList.Clear();

            this.openList.Add(startNode);

            /*
        for(int x = 0; x < grid.GetWidth(); x++)
        {
            for(int y = 0; y < grid.GetHeight(); y++)
            {
                PathNode pathNode = this.grid.GetNode(x, y);
                pathNode.gCost = int.MaxValue;
                pathNode.CalculateFCost();
                pathNode.previousNode = null;
            }
        }           */

            this.grid.InitializeNodes();

            //Set Startnode
            startNode.SetStartNode(endNode);

            while (openList.Count > 0)
            {
                PathNode currentNode = GetLowestFCostNode(this.openList);
                if (currentNode == endNode)
                {
                    //reached final node
                    return GetPath(endNode);
                }

                openList.Remove(currentNode);
                closeList.Add(currentNode);

                foreach (PathNode neighbor in GetNeighborNodes(currentNode))
                {
                    if (closeList.Contains(neighbor)) continue;
                    if (neighbor != endNode && !neighbor.getWalkable(this.gameObject))
                    {
                        //cannot walk there
                        this.closeList.Add(neighbor);
                        continue;
                    }

                    int tentantiveGCost = currentNode.CalculateTentativeGCost(neighbor);
                    if (tentantiveGCost < neighbor.gCost)
                    {
                        neighbor.SetNeighbornode(currentNode, endNode, tentantiveGCost);
                        if (!this.openList.Contains(neighbor)) this.openList.Add(neighbor);
                    }
                }
            }

            //no path found;
            return null;
        }

        private List<PathNode> GetNeighborNodes(PathNode node)
        {
            List<PathNode> result = new List<PathNode>();

            if (node.x - 1 >= 0)
            {
                result.Add(this.grid.GetNode(node.x - 1, node.y));
                if (node.y - 1 >= 0) result.Add(this.grid.GetNode(node.x - 1, node.y - 1));
                if (node.y + 1 < this.grid.GetHeight()) result.Add(this.grid.GetNode(node.x - 1, node.y + 1));
            }
            if (node.x + 1 < this.grid.GetWidth())
            {
                result.Add(this.grid.GetNode(node.x + 1, node.y));
                if (node.y - 1 >= 0) result.Add(this.grid.GetNode(node.x + 1, node.y - 1));
                if (node.y + 1 < this.grid.GetHeight()) result.Add(this.grid.GetNode(node.x + 1, node.y + 1));
            }
            if (node.y - 1 >= 0)
            {
                result.Add(this.grid.GetNode(node.x, node.y - 1));
            }
            if (node.y + 1 < this.grid.GetHeight())
            {
                result.Add(this.grid.GetNode(node.x, node.y + 1));
            }

            return result;
        }

        private List<PathNode> GetPath(PathNode endNode)
        {
            List<PathNode> path = new List<PathNode>();
            path.Add(endNode);
            PathNode currentNode = endNode;

            while (currentNode.GetPreviousNode() != null)
            {
                path.Add(currentNode.GetPreviousNode());
                currentNode = currentNode.GetPreviousNode();
            }

            path.Reverse();
            return path;
        }

        private PathNode GetLowestFCostNode(List<PathNode> pathNodeList)
        {
            PathNode result = pathNodeList[0];

            foreach (PathNode node in pathNodeList)
            {
                if (node.fCost < result.fCost) result = node;
            }

            return result;
        }
    }
}
