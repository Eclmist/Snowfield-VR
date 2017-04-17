using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IInteractable {

    void Interact(Transform referenceCheck,Rigidbody attachedPoint);

    void StopInteraction(Transform referenceCheck);

    Transform LinkedTransform
    {
        get;
    }

    float GetVelocity();
    
}
