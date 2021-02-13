using Unity.Mathematics;
using Unity.Collections;
using Unity.Jobs;
using Unity.Burst;
using System.Collections.Generic;
using UnityEngine;

namespace CrystalAlchemist 
{
    public class PathSeeker : MonoBehaviour
    {
        [SerializeField]
        private int graphIndex;

        [SerializeField]
        private bool diagonalMovement = false;

        private PathfinderGraph graph;

        private void Awake()
        {
            if(Pathfinding.Instance != null) this.graph = Pathfinding.Instance.graphs[graphIndex];
        }

        public List<Vector2> FindPath(Vector2 startPosition, Vector2 targetPosition)
        {
            if (this.graph == null) return new List<Vector2>();

            NativeList<int2> result = new NativeList<int2>(1, Allocator.TempJob);
            NativeArray<PathNode> grid = new NativeArray<PathNode>(graph.width * graph.height, Allocator.TempJob);

            for (int i = 0; i < graph.grid.Length; i++) grid[i] = graph.grid[i];

            FindPathJob findPathJob = new FindPathJob
            {
                ID = this.gameObject.GetInstanceID(),
                grid = grid,
                diagonal = this.diagonalMovement,
                startPosition = PathfinderUtil.GetInt2(startPosition, graph),
                endPosition = PathfinderUtil.GetInt2(targetPosition, graph),
                gridSize = new int2(graph.width, graph.height),
                result = result,
            };

            JobHandle jobHandle = findPathJob.Schedule();
            jobHandle.Complete();

            List<Vector2> path = GetPath(result);
            result.Dispose();
            grid.Dispose();
            return path;
        }

        private List<Vector2> GetPath(NativeList<int2> path)
        {
            List<Vector2> result = new List<Vector2>();

            for(int i = path.Length-2; i >= 0; i--) result.Add(PathfinderUtil.GetVector(path[i], graph));

            return result;
        }

        [BurstCompile]
        private struct FindPathJob : IJob
        {
            public int ID;
            public int2 startPosition;
            public int2 endPosition;
            public int2 gridSize;
            public bool diagonal;

            public NativeArray<PathNode> grid;
            public NativeList<int2> result;

            public void Execute() => FindPath();

            private void FindPath()
            {
                NativeArray<PathNode> pathNodeArray = new NativeArray<PathNode>(gridSize.x * gridSize.y, Allocator.Temp);

                for (int i = 0; i < grid.Length; i++) pathNodeArray[i] = grid[i];

                for (int i = 0; i < pathNodeArray.Length; i++)
                {
                    PathNode node = pathNodeArray[i];
                    node.index = CalculateIndex(node.x, node.y, gridSize.x);
                    node.hCost = CalculateDistanceCost(new int2(node.x, node.y), endPosition);
                    node.CalculateFCost();
                }               

                NativeArray<int2> neighbourOffsetArray = new NativeArray<int2>(4, Allocator.Temp);
                neighbourOffsetArray[0] = new int2(-1, 0);
                neighbourOffsetArray[1] = new int2(+1, 0);
                neighbourOffsetArray[2] = new int2(0, +1);
                neighbourOffsetArray[3] = new int2(0, -1);

                if (diagonal)
                {
                    neighbourOffsetArray = new NativeArray<int2>(8, Allocator.Temp);
                    neighbourOffsetArray[0] = new int2(-1, 0);
                    neighbourOffsetArray[1] = new int2(+1, 0);
                    neighbourOffsetArray[2] = new int2(0, +1);
                    neighbourOffsetArray[3] = new int2(0, -1);
                    neighbourOffsetArray[4] = new int2(-1, -1);
                    neighbourOffsetArray[5] = new int2(-1, +1);
                    neighbourOffsetArray[6] = new int2(+1, -1);
                    neighbourOffsetArray[7] = new int2(+1, +1);
                }

                int endNodeIndex = CalculateIndex(endPosition.x, endPosition.y, gridSize.x);

                PathNode startNode = pathNodeArray[CalculateIndex(startPosition.x, startPosition.y, gridSize.x)];
                startNode.gCost = 0;
                startNode.CalculateFCost();
                pathNodeArray[startNode.index] = startNode;

                NativeList<int> openList = new NativeList<int>(Allocator.Temp);
                NativeList<int> closedList = new NativeList<int>(Allocator.Temp);

                openList.Add(startNode.index);

                while (openList.Length > 0)
                {
                    int currentNodeIndex = GetLowestCostFNodeIndex(openList, pathNodeArray);
                    PathNode currentNode = pathNodeArray[currentNodeIndex];

                    if (currentNodeIndex == endNodeIndex)
                    {
                        break; //reached destination
                    }

                    //Remove current node from openlist
                    for (int i = 0; i < openList.Length; i++)
                    {
                        if (openList[i] == currentNodeIndex)
                        {
                            openList.RemoveAtSwapBack(i);
                            break;
                        }
                    }

                    closedList.Add(currentNodeIndex);

                    for (int i = 0; i < neighbourOffsetArray.Length; i++)
                    {
                        int2 neighbourOffset = neighbourOffsetArray[i];

                        int2 neighbourPosition = new int2(currentNode.x + neighbourOffset.x, currentNode.y + neighbourOffset.y);
                        if (!isPositionInsideGrid(neighbourPosition, gridSize)) continue;

                        int neighbourNodeIndex = CalculateIndex(neighbourPosition.x, neighbourPosition.y, gridSize.x);
                        if (closedList.Contains(neighbourNodeIndex)) continue;

                        PathNode neighbourNode = pathNodeArray[neighbourNodeIndex];
                        if (!neighbourNode.IsWalkable(ID)) continue;

                        int2 currentNodePosition = new int2(currentNode.x, currentNode.y);

                        int tentativeGCost = currentNode.gCost + CalculateDistanceCost(currentNodePosition, neighbourPosition);
                        if (tentativeGCost < neighbourNode.gCost)
                        {
                            neighbourNode.cameFromNodeIndex = currentNodeIndex;
                            neighbourNode.gCost = tentativeGCost;
                            neighbourNode.CalculateFCost();
                            pathNodeArray[neighbourNodeIndex] = neighbourNode;

                            if (!openList.Contains(neighbourNode.index)) openList.Add(neighbourNode.index);
                        }
                    }
                }

                PathNode endNode = pathNodeArray[endNodeIndex];
                if (endNode.cameFromNodeIndex == -1)
                {

                }
                else
                {
                    result.AddRange(CalculatePath(pathNodeArray, endNode));
                }

                pathNodeArray.Dispose();
                neighbourOffsetArray.Dispose();
                openList.Dispose();
                closedList.Dispose();

            }

            private NativeList<int2> CalculatePath(NativeArray<PathNode> pathNodeArray, PathNode endNode)
            {
                if (endNode.cameFromNodeIndex == -1)
                {
                    //no path
                    return new NativeList<int2>(Allocator.Temp);
                }
                else
                {
                    //found path
                    NativeList<int2> path = new NativeList<int2>(Allocator.Temp);
                    path.Add(new int2(endNode.x, endNode.y));

                    PathNode currentNode = endNode;
                    while (currentNode.cameFromNodeIndex != -1)
                    {
                        PathNode cameFromNode = pathNodeArray[currentNode.cameFromNodeIndex];
                        path.Add(new int2(cameFromNode.x, cameFromNode.y));
                        currentNode = cameFromNode;
                    }

                    return path;
                }
            }

            private bool isPositionInsideGrid(int2 gridPosition, int2 gridSize)
            {
                return gridPosition.x >= 0
                    && gridPosition.y >= 0
                    && gridPosition.x < gridSize.x
                    && gridPosition.y < gridSize.y;
            }

            private int CalculateIndex(int x, int y, int gridWidth)
            {
                return x + y * gridWidth;
            }

            private int CalculateDistanceCost(int2 aPosition, int2 bPosition)
            {
                int xDistance = math.abs(aPosition.x - bPosition.x);
                int yDistance = math.abs(aPosition.y - bPosition.y);
                int remaining = math.abs(xDistance - yDistance);
                return PathfinderUtil.MOVE_DIAGONAL_COST * math.min(xDistance, yDistance) + PathfinderUtil.MOVE_STRAIGHT_COST * remaining;
            }

            private int GetLowestCostFNodeIndex(NativeList<int> openList, NativeArray<PathNode> pathNodeArray)
            {
                PathNode lowestCostPathNode = pathNodeArray[openList[0]];
                for (int i = 1; i < openList.Length; i++)
                {
                    PathNode testPathNode = pathNodeArray[openList[i]];
                    if (testPathNode.fCost < lowestCostPathNode.fCost) lowestCostPathNode = testPathNode;
                }
                return lowestCostPathNode.index;
            }
        }
    }
}
