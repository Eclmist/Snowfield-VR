using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Runtime.Serialization;
using System;

public class AudioClipSurrogate : ISerializationSurrogate
{
    public void GetObjectData(object obj, SerializationInfo info, StreamingContext context)
    {
		//TODO: Implement store the thing what ever it take in as a string which is the path ten when you set object data then you something
        //AudioClip someObj = (AudioClip)obj;
    }

    public object SetObjectData(object obj, SerializationInfo info, StreamingContext context, ISurrogateSelector selector)
    {
        AudioClip someObj = (AudioClip)obj;
        return someObj;
    }
}
