using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IInteractable {

    void Interact();

    void StopInteraction();

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
