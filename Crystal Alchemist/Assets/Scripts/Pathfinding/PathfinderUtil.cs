using Unity.Mathematics;
using UnityEngine;

namespace CrystalAlchemist {
    public static class PathfinderUtil
    {        
        public const int MOVE_STRAIGHT_COST = 10;
        public const int MOVE_DIAGONAL_COST = 14;

        public static int2 GetInt2(Vector2 pos, PathfinderGraph grid)
        {
            int x = Mathf.FloorToInt((pos - grid.position).x / grid.cellSize);
            int y = Mathf.FloorToInt((pos - grid.position).y / grid.cellSize);
            return new int2(x, y);
        }

        public static Vector2 GetVector(int2 index, PathfinderGraph grid)
        {
            return new Vector2(index.x, index.y) * (grid.cellSize) + (grid.position+Vector2.one*(grid.cellSize/2));
        }

        public static PathNode GetNode(Vector2 position, PathfinderGraph grid)
        {
            int x = Mathf.FloorToInt((position - grid.position).x / grid.cellSize);
            int y = Mathf.FloorToInt((position - grid.position).y / grid.cellSize);

            return grid.grid[x * y];
        }
    }
}
