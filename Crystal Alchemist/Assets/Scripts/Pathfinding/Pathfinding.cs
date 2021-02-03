using Sirenix.OdinInspector;
using System.Collections.Generic;
using UnityEngine;

namespace CrystalAlchemist
{
    public class Pathfinding : MonoBehaviour
    {
        [SerializeField]
        private List<PathfindingGraph> graphs = new List<PathfindingGraph>();

        public static Pathfinding Instance { get; private set; }

        private void OnDrawGizmos()
        {
            foreach (PathfindingGraph graph in this.graphs)
            {
                graph.ShowDebug();
            }
        }

        public PathfindingGrid GetGrid(GraphType type)
        {
            foreach (PathfindingGraph graph in this.graphs)
            {
                if (graph.graphType == type) return graph.GetGrid();
            }

            return null;
        }

        [Button]
        private void InitializeGraphs()
        {
            foreach (PathfindingGraph graph in this.graphs)
            {
                graph.Initialize();
            }

            Instance = this;
        }

        private void Awake()
        {
            InitializeGraphs();
        }
    }
}
