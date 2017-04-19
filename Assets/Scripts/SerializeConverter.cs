using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Runtime.Serialization;

public class SerializeConverter{


	public static string ConvertToPath(Object o)
    {
        string path = UnityEditor.AssetDatabase.GetAssetPath(o);
        path = path.Replace("Assets/Resources/","");
        path = path.Replace(".mp3", "");

        return path;

    }

    public static List<Object> GetSerializableObject(params Object[] stuffToPackage)
    {
        List<Object> serializablePackage = new List<Object>();
        
        foreach(Object obj in stuffToPackage)
        {
            serializablePackage.Add(obj);
        }

        return serializablePackage;


    }

    



}
