using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Runtime.Serialization;
using System;

public class GameobjectPathSurrogate : ISerializationSurrogate
{

    string sub;

    public GameobjectPathSurrogate(string sub)
    {
        this.sub = sub;
    }

    public void GetObjectData(object obj, SerializationInfo info, StreamingContext context)
    {
        GameObject someObj = (GameObject)obj;
        //info.AddValue("objPath", /*sub + "/" + */someObj.name);
    }

    public object SetObjectData(object obj, SerializationInfo info, StreamingContext context, ISurrogateSelector selector)
    {
        //GameObject[] allResources = Resources.LoadAll<GameObject>("");
        //foreach (GameObject item in allResources)
        //{
        //    if (item.name == info.GetString("objPath"))
        //    {
        //        return item;
        //    }
        //}


        ////GameObject someObj = Resources.Load<GameObject>(info.GetString("objPath"));
        return null;
    }
}
