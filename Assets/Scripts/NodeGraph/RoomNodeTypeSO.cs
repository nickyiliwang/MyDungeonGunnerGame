using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "RoomNodeType_", menuName = "Scriptable Object/Dungeon/Room Node Type")]
public class RoomNodeTypeSO : ScriptableObject
{
    public string roomNodeTypeName;

    [Header("Only flag the RoomNodeTypes that should be visible in the editor")]
    public bool displayInNodeGraphEditor = true;

    [Header("One Type Should Be A Corridor")]
    public bool isCorridor;

    [Header("One Type Should Be A CorridorNS")]
    public bool isCorridorNS;

    [Header("One Type Should Be A CorridorEW")]
    public bool isCorridorEW;

    [Header("One Type Should Be An Entrance")]
    public bool isEntrance;

    [Header("One Type Should Be A Boss Room")]
    public bool isBossRoom;

    [Header("One Type Should Be A None (Unassigned)")]
    public bool isNone;

    // only run in the editor
#if UNITY_EDITOR
    // check every time it changes
    private void OnValidate()
    {
        HelperUtilities.ValidateCheckEmptyString(this, nameof(roomNodeTypeName), roomNodeTypeName);
    }
#endif
}
