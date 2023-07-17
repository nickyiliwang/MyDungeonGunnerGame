using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class RoomNodeGraphEditor : EditorWindow
{
    private GUIStyle roomNodeStyle;

    // Node layout values
    private const float nodeWidth = 160f;

    private const float nodeHeight = 75f;
    private const int nodePadding = 25;
    private const int nodeBorder = 12;

    [MenuItem("Room Node Graph Editor", menuItem = "Window/Dungeon Editor/Room Node Graph Editor")]
    private static void OpenWindow()
    {
        GetWindow<RoomNodeGraphEditor>("Room Nodde Graph Editor by Nick");
    }

    private void OnEnable()
    {
        // Define node layout style
        roomNodeStyle = new GUIStyle();
        //taloring what that GUI style background consist of
        //by loading predefined stuff like "node1"
        roomNodeStyle.normal.background = EditorGUIUtility.Load("node1") as Texture2D;
        //Like CSS
        roomNodeStyle.normal.textColor = Color.white;
        roomNodeStyle.padding = new RectOffset(nodePadding, nodePadding, nodePadding, nodePadding);
        roomNodeStyle.border = new RectOffset(nodeBorder, nodeBorder, nodeBorder, nodeBorder);
    }

    // Draw Editor
    // GUILayout provides more flexibility and control over the layout, while EditorGUILayout offers a simpler and more standardized approach
    private void OnGUI()
    {
        // GUILayout.BeginArea: defined the actual node style I think
        // Draw node 1
        GUILayout.BeginArea(
            new Rect(new Vector2(100f, 100f), new Vector2(nodeWidth, nodeHeight)),
            roomNodeStyle
        );

        EditorGUILayout.LabelField("Node 1");
        GUILayout.EndArea();

        // Draw node 2
        GUILayout.BeginArea(
            new Rect(new Vector2(300f, 300f), new Vector2(nodeWidth, nodeHeight)),
            roomNodeStyle
        );

        EditorGUILayout.LabelField("Node 2");
        GUILayout.EndArea();
    }
}
