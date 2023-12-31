using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityEditor.Callbacks;
using System;

public class RoomNodeGraphEditor : EditorWindow
{
    private GUIStyle roomNodeStyle;
    private GUIStyle roomNodeSelectedStyle;
    private static RoomNodeGraphSO currentRoomNodeGraph;
    private RoomNodeSO currentRoomNode = null;
    private RoomNodeTypeListSO roomNodeTypeList;

    // Node layout values
    private const float nodeWidth = 160f;
    private const float nodeHeight = 75f;
    private const int nodePadding = 25;
    private const int nodeBorder = 12;

    private const float lineWidth = 3f;
    private const float connectingLineArrowSize = 6f;
    private const float connectingLineWidth = 3f;

    [MenuItem("Room Node Graph Editor", menuItem = "Window/Dungeon Editor/Room Node Graph Editor")]
    private static void OpenWindow()
    {
        GetWindow<RoomNodeGraphEditor>("Room Node Graph Editor by Nick");
    }

    private void OnEnable()
    {
        // Subscribe to inspector selection change and adding our custom method
        Selection.selectionChanged += InspectorSelectionChanged;

        roomNodeStyle = new GUIStyle();
        roomNodeSelectedStyle = new GUIStyle();

        // Define node layout style
        //tailoring what that GUI style background consist of
        //by loading predefined stuff like "node1"
        roomNodeStyle.normal.background = EditorGUIUtility.Load("node1") as Texture2D;
        //Like CSS

        // Define selected node layout style
        roomNodeSelectedStyle.normal.background = EditorGUIUtility.Load("node1 on") as Texture2D;

        // Selected and default textColor + padding + border
        roomNodeStyle.normal.textColor = roomNodeSelectedStyle.normal.textColor = Color.white;
        roomNodeStyle.border = roomNodeSelectedStyle.border = new RectOffset(
            nodeBorder,
            nodeBorder,
            nodeBorder,
            nodeBorder
        );
        roomNodeStyle.padding = roomNodeSelectedStyle.padding = new RectOffset(
            nodePadding,
            nodePadding,
            nodePadding,
            nodePadding
        );

        // Load Room node types from the Resources prefab
        roomNodeTypeList = GameResources.Instance.roomNodeTypeList;
    }

    private void OnDisable()
    {
        Selection.selectionChanged -= InspectorSelectionChanged; // unsubscribe
    }

    private void InspectorSelectionChanged()
    {
        RoomNodeGraphSO roomNodeGraph = Selection.activeObject as RoomNodeGraphSO; // only when the active obj is a RoomNodeGraphSO so we check for null
        if (roomNodeGraph != null)
        {
            currentRoomNodeGraph = roomNodeGraph;
            GUI.changed = true;
        }
    }

    // OnOpenAssetAttribute

    // Open the room node graph editor if a room node graph SO asset is double clicked in the inspector

    [OnOpenAsset(0)]
    //     In Unity, the attribute [OnOpenAsset(0)] is used to create a custom method that gets called whenever an asset is double-clicked in the Unity editor. The (0) in this context specifies the priority of the callback method.

    // The 0 value indicates the highest priority, meaning that the custom method decorated with [OnOpenAsset(0)] will be called before other methods with a higher numerical value. If multiple methods have the same priority value, they will be called in the order in which they are defined.

    // By using the [OnOpenAsset] attribute with different priority values, you can define multiple methods to handle asset opening events, and control the order in which they are executed based on their assigned priority.


    // checks if the roomNodeGraph object is null else handle it by returning true
    public static bool OnDoubleClickAsset(int instanceID, int line)
    {
        // grabs the object from provided ID
        // as keyword to cast it to the type RoomNodeGraphSO
        RoomNodeGraphSO roomNodeGraph =
            EditorUtility.InstanceIDToObject(instanceID) as RoomNodeGraphSO;

        if (roomNodeGraph != null)
        {
            OpenWindow();
            currentRoomNodeGraph = roomNodeGraph;
            return true;
        }
        return false;
    }

    // Draw Editor
    // GUILayout provides more flexibility and control over the layout, while EditorGUILayout offers a simpler and more standardized approach
    private void OnGUI()
    {
        // if a SO of type RoomNodeGraphSO has been selected then proceed
        if (currentRoomNodeGraph != null)
        {
            DrawDraggedLine();

            ProcessEvents(Event.current);

            DrawRoomNodeConnections();

            DrawRoomNodes();
        }

        if (GUI.changed)
        {
            // Debug.Log(currentRoomNodeGraph.linePosition);
            // Debug.Log(Event.current.mousePosition);
            Repaint(); // Redraw editor
        }
    }

    private void DrawRoomNodeConnections()
    {
        // from each room node list, draw a line to all the child nodes from childID
        foreach (RoomNodeSO roomNode in currentRoomNodeGraph.roomNodeList)
        {
            if (roomNode.childRoomNodeIDList.Count > 0)
            {
                foreach (string childID in roomNode.childRoomNodeIDList)
                {
                    if (currentRoomNodeGraph.roomNodeDictionary.ContainsKey(childID))
                    {
                        DrawCOnnectionLine(
                            roomNode,
                            currentRoomNodeGraph.roomNodeDictionary[childID]
                        );

                        GUI.changed = true;
                    }
                }
            }
        }
    }

    private void DrawCOnnectionLine(RoomNodeSO parentRoomNode, RoomNodeSO childRoomNode)
    {
        Vector2 startPosition = parentRoomNode.rect.center;
        Vector2 endPosition = childRoomNode.rect.center;

        // drawing a arrow
        // I don't really get this I went to OCAD, but it's something about using perpendicular vectors

        // Vector2 threeThirdPosition = startPosition + (endPosition - startPosition) * 2 / 3f;
        Vector2 midPosition = (startPosition + endPosition) / 2f;

        // direction vector
        Vector2 direction = endPosition - startPosition;

        // calculate normalized perpendicular positions form the mid point
        Vector2 arrowTailPoint1 =
            midPosition
            - new Vector2(-direction.y, direction.x).normalized * connectingLineArrowSize;
        Vector2 arrowTailPoint2 =
            midPosition
            + new Vector2(-direction.y, direction.x).normalized * connectingLineArrowSize;

        Vector2 arrowHeadPoint = midPosition + direction.normalized * connectingLineArrowSize;

        // Draw arrow from points
        Handles.DrawBezier(
            arrowHeadPoint,
            arrowTailPoint1,
            arrowHeadPoint,
            arrowTailPoint1,
            Color.white,
            null,
            connectingLineWidth
        );

        Handles.DrawBezier(
            arrowHeadPoint,
            arrowTailPoint2,
            arrowHeadPoint,
            arrowTailPoint2,
            Color.white,
            null,
            connectingLineWidth
        );

        Handles.DrawBezier(
            startPosition,
            endPosition,
            startPosition,
            endPosition,
            Color.white,
            null,
            lineWidth
        );
    }

    private void DrawDraggedLine()
    {
        if (currentRoomNodeGraph.linePosition != Vector2.zero) // if line position none zero
        {
            // Draw line from node to line pos
            Handles.DrawBezier(
                currentRoomNodeGraph.roomNodeToDrawLineFrom.rect.center, // line start
                currentRoomNodeGraph.linePosition, // line end
                currentRoomNodeGraph.roomNodeToDrawLineFrom.rect.center, // tangent start
                currentRoomNodeGraph.linePosition, // tangent end
                Color.white,
                null,
                lineWidth // line width
            );
        }
    }

    private void ProcessEvents(Event currentEvent)
    {
        // Get room node that mouse is hovering if it's null or not currently being dragged
        if (currentRoomNode == null || currentRoomNode.isLeftClickDragging == false)
        {
            currentRoomNode = currentMouseHoveringRoomNode(currentEvent);
        }

        // if mouse isn't over a room node or currently dragging a line from the room node then process the graph event

        // if (currentRoomNode == null)
        if (currentRoomNode == null || currentRoomNodeGraph.roomNodeToDrawLineFrom != null)
        {
            ProcessGraphEvents(currentEvent);
        }
        else
        {
            currentRoomNode.ProcessNodeEvents(currentEvent);
        }
    }

    // looping through every node in the list and comparing the rectangle position with current mouse position, if found return the node as current.
    private RoomNodeSO currentMouseHoveringRoomNode(Event currentEvent)
    {
        for (int i = currentRoomNodeGraph.roomNodeList.Count - 1; i >= 0; i--)
        {
            if (currentRoomNodeGraph.roomNodeList[i].rect.Contains(currentEvent.mousePosition))
            {
                // Debug.Log("hovering" + currentRoomNodeGraph.roomNodeList[i]);
                return currentRoomNodeGraph.roomNodeList[i];
            }
        }
        return null;
    }

    private void ProcessGraphEvents(Event currentEvent)
    {
        switch (currentEvent.type)
        {
            case EventType.MouseDown:
                ProcessMouseDownEvent(currentEvent);
                break;

            case EventType.MouseUp:
                ProcessMouseUpEvent(currentEvent);
                break;

            case EventType.MouseDrag:
                ProcessMouseDragEvent(currentEvent);
                break;

            default:
                break;
        }
    }

    private void ProcessMouseUpEvent(Event currentEvent)
    {
        // if releasing the right mouse button and currently dragging a line
        if (currentEvent.button == 1 && currentRoomNodeGraph.roomNodeToDrawLineFrom != null)
        {
            RoomNodeSO roomNode = currentMouseHoveringRoomNode(currentEvent);

            if (roomNode != null)
            {
                // adding childID to parent node and add parentID to child node parent list
                if (
                    currentRoomNodeGraph.roomNodeToDrawLineFrom.AddChildRoomNodeIDToRoomNode(
                        roomNode.id
                    )
                )
                {
                    roomNode.addParentRoomNodeIDToRoomNode(
                        currentRoomNodeGraph.roomNodeToDrawLineFrom.id
                    );
                }
            }

            ClearLineDrag();
        }
    }

    private void ClearLineDrag()
    {
        currentRoomNodeGraph.roomNodeToDrawLineFrom = null;
        currentRoomNodeGraph.linePosition = Vector2.zero;
        GUI.changed = true;
    }

    private void ProcessMouseDownEvent(Event currentEvent)
    {
        if (currentEvent.button == 1)
        {
            ShowContextMenu(currentEvent.mousePosition);
        }
        else if (currentEvent.button == 0)
        {
            ClearLineDrag();
            ClearAllSelectedRoomNodes();
        }
    }

    private void ClearAllSelectedRoomNodes()
    {
        foreach (RoomNodeSO roomNode in currentRoomNodeGraph.roomNodeList)
        {
            roomNode.isSelected = !roomNode.isSelected;
            GUI.changed = true;
        }
    }

    private void ProcessMouseDragEvent(Event currentEvent)
    {
        if (currentEvent.button == 1)
        {
            // process right mouse drag
            if (currentRoomNodeGraph.roomNodeToDrawLineFrom != null)
            {
                currentRoomNodeGraph.linePosition += currentEvent.delta;
                GUI.changed = true;
            }
        }
    }

    private void ShowContextMenu(Vector2 mousePosition)
    {
        GenericMenu menu = new GenericMenu();

        // Add the create Room Node into the menu at the mouse position
        menu.AddItem(new GUIContent("Create Room Node"), false, CreateRoomNode, mousePosition);

        menu.ShowAsContext();
    }

    private void CreateRoomNode(object mousePositionObject)
    {
        if (currentRoomNodeGraph.roomNodeList.Count == 0)
        {
            CreateRoomNode(new Vector2(200f, 200f), roomNodeTypeList.list.Find(x => x.isEntrance));
        }

        // find the isNone room node type
        CreateRoomNode(mousePositionObject, roomNodeTypeList.list.Find(x => x.isNone));
    }

    // Overloaded methods, redirects to this with different params used, such as above
    private void CreateRoomNode(object mousePositionObject, RoomNodeTypeSO roomNodeType)
    {
        Vector2 mousePosition = (Vector2)mousePositionObject;

        // create room node SO asset
        RoomNodeSO roomNode = ScriptableObject.CreateInstance<RoomNodeSO>();

        // add room node SO asset
        currentRoomNodeGraph.roomNodeList.Add(roomNode);

        // set room node values
        roomNode.Initialize(
            new Rect(mousePosition, new Vector2(nodeWidth, nodeHeight)),
            currentRoomNodeGraph,
            roomNodeType
        );

        // add and save asset
        AssetDatabase.AddObjectToAsset(roomNode, currentRoomNodeGraph);
        AssetDatabase.SaveAssets();

        // Refresh graph node dict
        currentRoomNodeGraph.OnValidate();
    }

    private void DrawRoomNodes()
    {
        foreach (RoomNodeSO roomNode in currentRoomNodeGraph.roomNodeList)
        {
            if (roomNode.isSelected)
            {
                roomNode.Draw(roomNodeSelectedStyle);
            }
            else
            {
                roomNode.Draw(roomNodeStyle);
            }
        }
        GUI.changed = true;
    }
}
