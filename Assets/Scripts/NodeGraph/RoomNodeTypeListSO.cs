using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(
    fileName = "RoomNodeTypeListSO",
    menuName = "Scriptable Object/Dungeon/Room Node Type List"
)]
public class RoomNodeTypeListSO : ScriptableObject
{
    [Space(10)]
    [Header("ROOM NODE TYPE LIST")]
    [Tooltip(
        "This list should be populated with all the RoomNodeTypeSO for the game - it is used instead of an enum, In C#, an enum (short for enumeration) is a value type that defines a set of named constants. It is used to represent a fixed number of possible values, making the code more readable and maintainable."
    )]
    public List<RoomNodeTypeSO> list;

#if UNITY_EDITOR
    private void OnValidate()
    {
        HelperUtilities.ValidateCheckEnumerableValues(this, nameof(list), list);
    }
#endif
}
