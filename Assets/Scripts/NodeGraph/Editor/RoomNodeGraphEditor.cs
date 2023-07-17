using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class RoomNodeGraphEditor : EditorWindow
{
    [MenuItem("Room Node Graph Editor", menuItem = "Window/Dungeon Editor/Room Node Graph Editor")]
    private static void OpenWindow()
    {
        GetWindow<RoomNodeGraphEditor>("Room Nodde Graph Editor by Nick");
    }

    private void OnGUI()
    {
        Debug.Log("OnGUI has been called");
    }
}