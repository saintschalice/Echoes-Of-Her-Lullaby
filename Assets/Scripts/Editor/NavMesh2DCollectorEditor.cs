using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(NavMesh2DCollector))]
public class NavMesh2DCollectorEditor : Editor
{
    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();

        EditorGUILayout.Space(10);

        NavMesh2DCollector collector = (NavMesh2DCollector)target;

        // Big bake button
        GUI.backgroundColor = Color.cyan;
        if (GUILayout.Button("Bake 2D NavMesh", GUILayout.Height(40)))
        {
            collector.Bake2DNavMesh();
        }
        GUI.backgroundColor = Color.white;

        EditorGUILayout.Space(5);
        EditorGUILayout.HelpBox(
            "This will collect all Collider2D components that match the layer mask and bake them into a NavMesh.",
            MessageType.Info
        );
    }
}