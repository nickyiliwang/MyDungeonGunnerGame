using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class HelperUtilities
{
    public static bool ValidateCheckEmptyString(
        Object thisObject,
        string fieldName,
        string stringToCheck
    )
    {
        if (stringToCheck == "")
        {
            Debug.Log(
                fieldName
                    + " is empty and must contain a value in object "
                    + thisObject.name.ToString()
            );
            return true;
        }

        return false;
    }

    public static bool ValidateCheckEnumerableValues(
        Object thisObject,
        string fieldName,
        IEnumerable enumerableObjectToCheck
    )
    {
        bool error = false;
        int count = 0;

        foreach (var item in enumerableObjectToCheck)
        {
            if (item == null)
            {
                // in case a value deleted an entry in the inspector but it's actually just became a null value
                Debug.Log(fieldName + " has null values in object " + thisObject.name.ToString());
                Debug.Log("Count: " + count);
                error = true;
            }
            else
            {
                count++;
            }
        }

        if (count == 0)
        {
            Debug.Log(fieldName + " has no values in object " + thisObject.name.ToString());
            error = true;
        }

        return error;
    }
}
