using System.Collections.Generic;
using Unity.Collections;
using Unity.Mathematics;
using UnityEngine;

namespace CrystalAlchemist
{
    public static class GizmoUtil
    {
        /*
        public static void PathfinderGizmo(PathfindingGrid grid, Color gridColor, Color collisionColor, bool showSphere)
        {
            Gizmos.color = gridColor;

            if (grid != null && grid.gridArray != null)
            {
                for (int x = 0; x < grid.gridArray.GetLength(0); x++)
                {
                    for (int y = 0; y < grid.gridArray.GetLength(1); y++)
                    {
                        PathNode node = grid.gridArray[x, y];
                        if (node != null)
                        {
                            Gizmos.DrawWireCube(node.GetVector(), node.GetSize());
                            if (showSphere) Gizmos.DrawWireSphere(node.GetVector(), node.GetRadius());
                            if (!node.getWalkable())
                                Gizmos.DrawIcon(node.GetVector(), "Pathfinding/NotWalkable", false, collisionColor);
                        }
                    }
                }
            }
        }*/

        public static void ShowGrid(NativeArray<int2> pathNodeArray, float cellsize)
        {
            for(int i = 0; i < pathNodeArray.Length; i++)
            {
                Gizmos.DrawWireCube(new Vector2(pathNodeArray[i].x, pathNodeArray[i].y), Vector2.one*cellsize);
            }
        }

        public static void ShowWalkingLines(List<Vector2> path)
        {
            if (path != null)
            {
                for (int i = 0; i < path.Count - 1; i++)
                {
                    Debug.DrawLine(path[i], path[i + 1], Color.red,60f);
                }
            }
        }
    }
}
