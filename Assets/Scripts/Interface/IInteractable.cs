using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IInteractable {

    void Interact(VR_Controller_Custom referenceCheck);//The controller checks the interacted object every frame

    void StopInteraction(VR_Controller_Custom referenceCheck);//Method to call to define the end of interaction

    void StartInteraction(VR_Controller_Custom referenceCheck);//Method to call to define the start of interaction

    VR_Controller_Custom LinkedController
    {
        get;
    }
    
}
