using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IInteractable {

    void Interact(VR_Controller_Custom referenceCheck);//make whatever u want to interact implement iinteractable, put a trigger area on the same object as the script then copy and 
    //paste the interactable item interact/stopinteraction/startinteraction/linkedcontroller functions and 
    //finally just put your input over for example referenceCheck.Device.GetTouchDown(SteamVR_Controller.ButtonMask.Trigger){}

    void StopInteraction(VR_Controller_Custom referenceCheck);

    void StartInteraction(VR_Controller_Custom referenceCheck);

    VR_Controller_Custom LinkedController
    {
        get;
    }
    
}
