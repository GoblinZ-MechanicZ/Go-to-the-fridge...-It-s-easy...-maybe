using System;
using UnityEngine;

public class Singleton<T> : MonoBehaviour where T : MonoBehaviour
{
    private static T _instance;

    // Getter singleton instance
    public static T instance
    {
        get
        {
            if (_instance == null)
            {
                _instance = FindObjectOfType<T>();

                if (_instance == null)
                {
                    string objectName = typeof(T).Name.ToString();

                    var singleton = new GameObject(objectName);
                    _instance = singleton.AddComponent<T>();
                    try
                    {
                        // Защитить от удаления при переходе между сценами
                        DontDestroyOnLoad(singleton);
                    }
                    catch (Exception e)
                    {
                        Debug.LogError(e);
                    }
                }
            }
            return _instance;
        }
    }
}
