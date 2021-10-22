using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SaveHandler : MonoBehaviour
{
    //SingletonPattern
    public static SaveHandler instance;
    private void Awake()
    {
        if (instance)
        {
            Destroy(gameObject);
        }
        else
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
    }


    public SaveFile mySaveFile;

    //JSON2SCO
    public void Decode()
    {
        //fetch the file hard drive
        string jsonStr = "path";

        JsonUtility.FromJsonOverwrite(jsonStr, mySaveFile);

    }
    public void Load()
    {

    }
    public void Save()
    {

    }

    //SCO2JSON
    public void Encode()
    {

    }


}
