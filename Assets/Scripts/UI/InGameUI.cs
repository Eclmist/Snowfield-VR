﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InGameUI : UI_Manager, IInteractable
{
    private VR_Controller_Custom linkedController = null;

    public VR_Controller_Custom LinkedController
    {
        get
        {
            return linkedController;
        }
        set
        {
            linkedController = value;
        }
    }

    public virtual void Interact(VR_Controller_Custom referenceCheck)
    {
        if (referenceCheck.Device.GetTouchDown(SteamVR_Controller.ButtonMask.Trigger))
        {
            StartInteraction(referenceCheck);
        }
        else if (referenceCheck.Device.GetTouchUp(SteamVR_Controller.ButtonMask.Trigger) && linkedController == referenceCheck)
        {
            StopInteraction(referenceCheck);
        }
    }

    public virtual void StartInteraction(VR_Controller_Custom referenceCheck)
    {
        if (linkedController != null && linkedController != referenceCheck)
            linkedController.SetInteraction(null);

        linkedController = referenceCheck;
    }

    public void StopInteraction(VR_Controller_Custom referenceCheck)
    {
        if (linkedController == referenceCheck)
        {
            linkedController = null;
            referenceCheck.SetInteraction(null);
        }
    }
    // Use this for initialization
    void Start () {
		
	}
	
	// Update is called once per frame
	void Update ()
    {
        
    }
}