using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

// In C#, square brackets are used to apply attributes to classes, methods, properties, or fields. These attributes provide additional information or instructions to the compiler or runtime environment.

// In the specific case you mentioned, "[HideInInspector]" is an attribute in Unity's scripting system. It is used to hide a property or field from the inspector in the Unity Editor. This means that the property or field will not be visible or editable in the Inspector window when selecting an object in Unity's editor.

public class RoomNodeSO : ScriptableObject
{
    [HideInInspector]
    public string id;

    // creates a new instance of the generic List class, specifically instantiated with the type string.
    [HideInInspector]
    public List<string> parentRoomNodeIDList = new List<string>();

    [HideInInspector]
    public List<string> childRoomNodeIDList = new List<string>();

    [HideInInspector]
    public RoomNodeGraphSO roomNodeGraph;

    public RoomNodeTypeSO roomNodeType;

    [HideInInspector]
    public RoomNodeTypeListSO roomNodeTypeList;

#if UNITY_EDITOR


    [HideInInspector]
    public Rect rect;

    public void Initialize(Rect rect, RoomNodeGraphSO nodeGraph, RoomNodeTypeSO roomNodeType)
    {
        this.rect = rect;
        this.id = Guid.NewGuid().ToString();
        this.name = "RoomNode";
        this.roomNodeGraph = nodeGraph;
        this.roomNodeType = roomNodeType;

        roomNodeTypeList = GameResources.Instance.roomNodeTypeList;
    }

    public void Draw(GUIStyle nodeStyle)
    {
        // GUILayout.BeginArea: defined the actual node style I think
        // Draw node box 1
        GUILayout.BeginArea(rect, nodeStyle);

        // Start region to detect popup selection changes
        EditorGUI.BeginChangeCheck();

        // Display a popup using the RoomNodeType name values that can be selected from (default to the currently set roomNodeType)
        int selectedRoomNodeType = roomNodeTypeList.list.FindIndex(x => x == roomNodeType);

        int selection = EditorGUILayout.Popup(
            "",
            selectedRoomNodeType,
            GetRoomNodeTypeNamesToDisplay()
        );

        roomNodeType = roomNodeTypeList.list[selection];

        if (EditorGUI.EndChangeCheck())
            EditorUtility.SetDirty(this); // makes sure any changes gets saves

        GUILayout.EndArea();
    }

    // Populate a string array with the room node types to display what can be selected
    public string[] GetRoomNodeTypeNamesToDisplay()
    {
        string[] roomArray = new string[roomNodeTypeList.list.Count];
        for (int i = 0; i < roomNodeTypeList.list.Count; i++)
        {
            roomArray[i] = roomNodeTypeList.list[i].roomNodeTypeName;
        }

        return roomArray;
    }

#endif
}
