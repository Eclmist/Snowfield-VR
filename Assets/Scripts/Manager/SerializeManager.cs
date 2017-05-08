using UnityEngine;
using System;
using System.IO;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;
using System.Collections.Generic;

public class SerializeManager
{
    private static BinaryFormatter binaryFormatter = new BinaryFormatter();
   
    public static void Save(string fileName, object obj)
    {
        fileName = "SerializedFiles/" + fileName;
        SurrogateSelector surrogateSelector = new SurrogateSelector();
        surrogateSelector.AddSurrogate(typeof(Sprite), new StreamingContext(StreamingContextStates.All),  new SpriteSurrogate());
        surrogateSelector.AddSurrogate(typeof(Mesh), new StreamingContext(StreamingContextStates.All), new MeshSurrogate());
        surrogateSelector.AddSurrogate(typeof(AudioClip), new StreamingContext(StreamingContextStates.All), new AudioClipSurrogate());
        binaryFormatter.SurrogateSelector = surrogateSelector;

        MemoryStream memoryStream = new MemoryStream();
        binaryFormatter.Serialize(memoryStream, obj);
        string[] temp = new string[1];
        temp[0] = System.Convert.ToBase64String(memoryStream.ToArray());
        Debug.Log(temp[0]);
        try
        {
            File.WriteAllLines(Path.Combine(Application.dataPath, fileName), temp);
        }
        catch (Exception e)
        {
            Debug.Log(e);
        }

    }

    public static void SaveBundle(object o)
    {
        //List<object> myList;
        System.Collections.IList someList;

        // First check
        if (IsList(o))
        {
            //myList = (List<object>)o;
            someList = (System.Collections.IList)o;
            foreach(object someObj in someList)
            {
                if (!IsList(someObj))
                    Save(someObj.GetHashCode().ToString(), someObj);
                else
                    SaveBundle(someObj);
            }
        }

    }

       

    public static bool IsList(object o)
    {
        if (o == null) return false;
        
        return o is System.Collections.IList &&
               o.GetType().IsGenericType &&
               o.GetType().GetGenericTypeDefinition().IsAssignableFrom(typeof(List<>));
    }



    public static object Load(string fileName)
    {
        fileName = "SerializedFiles/" + fileName;

        string[] data = new string[1];
        MemoryStream memoryStream = null;
        try
        {
            data = File.ReadAllLines(Path.Combine(Application.dataPath, fileName));
            memoryStream = new MemoryStream(System.Convert.FromBase64String(data[0]));
            return binaryFormatter.Deserialize(memoryStream);
        }
        catch (Exception e)
        {
            Debug.Log(e);
        }

        return null;

    }



}
