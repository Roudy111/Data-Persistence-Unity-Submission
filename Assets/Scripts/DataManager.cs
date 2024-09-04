using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

public class DataManager : MonoBehaviour
{
   /// <summary>
   /// This script is singlethon to manage the saving between scene and session
   /// </summary>
   /// 
    public static DataManager instance { get; private set;}

    void Awake()
    {
        if (instance != null)
        {
            Destroy(gameObject);
            return;
        }

        instance = this;
        DontDestroyOnLoad(gameObject);
    }

    [System.Serializable]
    class SaveDate
    {

    }
}
