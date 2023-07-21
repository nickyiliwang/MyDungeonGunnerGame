using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityEditor.Callbacks;

public class RoomNodeGraphEditor : EditorWindow
{
    private GUIStyle roomNodeStyle;
    private static RoomNodeGraphSO currentRoomNodeGraph;
    private RoomNodeTypeListSO roomNodeTypeList;

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

        // Load Room node types from the Resources prefab
        roomNodeTypeList = GameResources.Instance.roomNodeTypeList;
    }

    // Open the room node graph editor if a room node graph SO asset is double clicked in the inspector
    // OnOpenAssetAttribute

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
            // Process Event;
            // such as mouse clicks or selections in the editor
            ProcessEvent(Event.current);

            DrawRoomNodes();
        }

        if (GUI.changed)
        {
            Repaint(); // The Repaint() method forces the repaint of the current window in the Unity editor. This means that the window will be redrawn immediately, updating its content based on the changes made to the GUI elements.
        }
    }

    private void ProcessEvent(Event currentEvent)
    {
        ProcessRoomNodeGraphEvents(currentEvent);
    }

    private void ProcessRoomNodeGraphEvents(Event currentEvent)
    {
        switch (currentEvent.type)
        {
            case EventType.MouseDown:
                ProcessMouseDownEvent(currentEvent);
                break;

            default:
                break;
        }
    }

    private void ProcessMouseDownEvent(Event currentEvent)
    {
        if (currentEvent.button == 1)
        {
            ShowContextMenu(currentEvent.mousePosition);
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
    }

    private void DrawRoomNodes() { 

        foreach (RoomNodeSO roomNode in currentRoomNodeGraph.roomNodeList)
        {
            roomNode.Draw(roomNodeStyle);

            GUI.changed = true;
        }

    }
}
