using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Runtime.Serialization;
using System;

public class ListObjectSurrogate
{
    public void GetObjectData(object obj, SerializationInfo info, StreamingContext context)
    {
        List<object> list = (List<object>)obj;
        foreach(object o in list)
        {
            info.AddValue("listElement",o);
        }
    }


    public object SetObjectData(object obj, SerializationInfo info, StreamingContext context, ISurrogateSelector selector)
    {
        throw new NotImplementedException();
    }
}
