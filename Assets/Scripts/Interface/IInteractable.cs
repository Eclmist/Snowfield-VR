using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IInteractable {

    void Interact(Transform referenceCheck);

    void StopInteraction(Transform referenceCheck);

    Transform LinkedTransform
    {
        get;
    }

    bool HasPivot
    {
        get;
    }

    Vector3 PositionalOffset
    {
        get;
    }

    Vector3 RotationalOffset
    {
        get;
    }
}
