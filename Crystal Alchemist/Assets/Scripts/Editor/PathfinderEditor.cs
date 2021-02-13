using UnityEditor;
using UnityEngine;

namespace CrystalAlchemist
{
    [CustomEditor(typeof(PathfinderGraph))]
    public class PathfinderEditor : Editor
    {
        public override void OnInspectorGUI()
        {
            EditorGUILayout.HelpBox("Tilemap is required!", MessageType.Info);
            DrawDefaultInspector();
            PathfinderGraph myTarget = (PathfinderGraph)target;
            if (GUILayout.Button("Initialize Graph")) myTarget.Initialize();
        }
    }
}
