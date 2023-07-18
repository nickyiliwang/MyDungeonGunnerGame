using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "RoomNodeGraph", menuName = "Scriptable Object/Dungeon/RoomNodeGraph")]
// Specifying where the menu item is
public class RoomNodeGraphSO : ScriptableObject
{
    // Making hidden properties for a Node
    [HideInInspector]
    public RoomNodeGraphSO roomNodeTypeList;

    [HideInInspector]
    public List<RoomNodeSO> roomNodeList = new List<RoomNodeSO>();

    [HideInInspector]
    public Dictionary<string, RoomNodeSO> roomNodeDictionary = new Dictionary<string, RoomNodeSO>();
}
