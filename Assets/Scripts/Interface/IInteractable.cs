using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IInteractable {

    void Interact(VR_Controller_Custom referenceCheck);

    void StopInteraction(VR_Controller_Custom referenceCheck);

    void StartInteraction(VR_Controller_Custom referenceCheck);

    VR_Controller_Custom LinkedController
    {
        get;
    }
    
}
