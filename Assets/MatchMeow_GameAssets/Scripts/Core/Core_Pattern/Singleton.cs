using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Singleton<T> : MonoBehaviour where T : MonoBehaviour
{
    private static T s_instance;

    public static T Instance
    {
        get
        {
            if (s_instance == null)
            {
                // Find singleton.
                s_instance = FindObjectOfType<T>();

                // Create new instance if one doesn't already exist.
                if (s_instance == null)
                {
                    // Need to create a new GameObject to attach the singleton to.
                    var singletoonObject = new GameObject();
                    s_instance = singletoonObject.AddComponent<T>();
                    singletoonObject.name = typeof(T).ToString() + " (Singleton)";
                }
            }

            return s_instance;
        }
    }
}
