using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class Extensions
{
    //Even though they are used like normal methods, extension
    //methods must be declared static. Notice that the first
    //parameter has the 'this' keyword followed by a Transform
    //variable. This variable denotes which class the extension
    //method becomes a part of.
    public static bool OverlapPoint(this Collider collider, Vector3 point)
    {
        return false;
    }
}
