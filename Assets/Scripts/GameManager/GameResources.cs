using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameResources : MonoBehaviour
{
    private static GameResources instance;

    public static GameResources Instance
    {
        // used in the context of properties within a class. defines a "getter" method for a property. "getter" It allows you to retrieve the value of a private field in a class.

        // define how the value should be obtained and returned when the property is accessed.
        get
        {
            if (instance == null)
            {
                // Resources.Load method is used in Unity to load assets from the resources folder.
                // ie. prefabs we are going to create => loads in
                // This pattern is delays the initialization of an object until it is actually needed.

                // Anything from the Resources unity special folder can be loaded in here 
                instance = Resources.Load<GameResources>("GameResources");
            }
            return instance;
        }
    }

    [Space(10)]
    [Header("DUNGEON")]
    [Tooltip("Populate with the dungeon RoomNodeTypeListSO")]
    public RoomNodeTypeListSO roomNodeTypeList;
}
