using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "RoomNodeGraph", menuName = "Scriptable Object/Dungeon/RoomNodeGraph")]
// Specifying where the menu item is
public class RoomNodeGraphSO : ScriptableObject
{
    [HideInInspector]
    public RoomNodeGraphSO roomNodeTypeList;

    [HideInInspector]
    public List<RoomNodeSO> roomNodeList = new List<RoomNodeSO>();

    [HideInInspector]
    public Dictionary<string, RoomNodeSO> roomNodeDictionary = new Dictionary<string, RoomNodeSO>();

    private void Awake()
    {
        LoadRoomNodeDictionary();
    }

    private void LoadRoomNodeDictionary()
    {
        roomNodeDictionary.Clear();

        foreach (RoomNodeSO node in roomNodeList)
        {
            roomNodeDictionary[node.id] = node;
        }
    }

#if UNITY_EDITOR
    [HideInInspector]
    public RoomNodeSO roomNodeToDrawLineFrom = null;

    [HideInInspector]
    public Vector2 linePosition;

    // repopulate he dict every time a change happens
    public void OnValidate()
    {
        LoadRoomNodeDictionary();
    }

    public void SetNodeToDrawConnectionLineFrom(RoomNodeSO node, Vector2 position)
    {
        roomNodeToDrawLineFrom = node;
        linePosition = position;
    }

#endif
}
