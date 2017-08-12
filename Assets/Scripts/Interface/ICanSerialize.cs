using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface ICanSerialize
{

    void Save();
    string SerializedFileName { get; }
}
