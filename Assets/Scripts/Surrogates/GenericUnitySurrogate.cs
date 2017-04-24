using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Runtime.Serialization;
using System;
using UnityObject = UnityEngine.Object;

public class GenericUnitySurrogate : ISerializationSurrogate
{

    private Dictionary<string, UnityObject> serializedObjects;

    public GenericUnitySurrogate(Dictionary<string,UnityObject> serializedObjects)
    {
        this.serializedObjects = serializedObjects;
    }
    

    public void GetObjectData(object obj, SerializationInfo info, StreamingContext context)
    {
        string fieldName = context.Context as string;
        serializedObjects[fieldName] = obj as UnityObject;
        
    }

    public object SetObjectData(object obj, SerializationInfo info, StreamingContext context, ISurrogateSelector selector)
    {
        string fieldName = context.Context as string;
        return serializedObjects[fieldName];
    }



}
