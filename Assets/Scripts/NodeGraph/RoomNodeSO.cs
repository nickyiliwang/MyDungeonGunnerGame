using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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
}
