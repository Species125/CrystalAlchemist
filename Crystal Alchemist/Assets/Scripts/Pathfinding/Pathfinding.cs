using UnityEngine;
using Unity.Mathematics;
using System.Collections.Generic;

namespace CrystalAlchemist
{
    public struct PathNode
    {
        public int x;
        public int y;

        public int index;

        public int gCost;
        public int hCost;
        public int fCost;

        public bool isWalkable;
        public bool isDynamic;
        public int cameFromNodeIndex;

        public int owner;

        public int2 GetInt2()
        {
            return new int2(x, y);
        }

        public void CalculateFCost() => fCost = gCost + hCost;

        public bool IsWalkable(int ID = 0)
        {
            return (this.isWalkable && (owner == 0 || owner == ID));
        }
    }

    public class Pathfinding : MonoBehaviour
    {
        public static Pathfinding Instance { get; private set; }

        public List<PathfinderGraph> graphs = new List<PathfinderGraph>();

        private void Awake() => Instance = this;         
    }
}