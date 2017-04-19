using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Runtime.Serialization;
using System;

public class SessionSurrogate : ISerializationSurrogate
{
    public void GetObjectData(object obj, SerializationInfo info, StreamingContext context)
    {
        Session s = (Session)obj;
        info.AddValue("title",s.Title);
    }

    public object SetObjectData(object obj, SerializationInfo info, StreamingContext context, ISurrogateSelector selector)
    {
        Session s = (Session)obj;
        s.Title = info.GetValue("title",typeof(string)) as string;
        return obj;
    }
}
