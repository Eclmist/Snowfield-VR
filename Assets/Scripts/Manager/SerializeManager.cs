using UnityEngine;
using System;
using System.IO;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;

public class SerializeManager {

    public static SerializeManager Instance = new SerializeManager();

    private BinaryFormatter binaryFormatter = new BinaryFormatter();
   
	public void Save(string saveTag,object obj)
    {
        MemoryStream memoryStream = new MemoryStream();
        binaryFormatter.Serialize(memoryStream, obj);
        string temp = System.Convert.ToBase64String(memoryStream.ToArray());
        PlayerPrefs.SetString(saveTag, temp);
        Debug.Log("Save: "+saveTag);
    }
	
    public object Load(string saveTag)
    {
        string temp = PlayerPrefs.GetString(saveTag);
        if (temp == string.Empty)
            return null;
        MemoryStream memoryStream = new MemoryStream(System.Convert.FromBase64String(temp));
        Debug.Log("Load: " + saveTag);
        return binaryFormatter.Deserialize(memoryStream);
    }
	
}
