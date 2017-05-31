using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shield : Equipment,IBlock {

    private bool isBlocking;

    private Ward ward;
    protected override void Awake()
    {
        base.Awake();
        ward = GetComponent<Ward>();
    }
    public bool IsBlocking
    {
        get
        {
            return isBlocking;
        }
        set
        {
            isBlocking = value;
        }
    }
    // Use this for initialization
    public override void Interact(VR_Controller_Custom referenceCheck)
    {
        base.Interact(referenceCheck);
        if (referenceCheck.Device.GetTouchDown(SteamVR_Controller.ButtonMask.Grip))
        {
            isBlocking = true;
            ward.SetWardActive(true);
        }
        else if (referenceCheck.Device.GetTouchUp(SteamVR_Controller.ButtonMask.Grip))
        {
            isBlocking = false;
            ward.SetWardActive(false);
        }
    }

    public override void StopInteraction(VR_Controller_Custom referenceCheck)
    {
        if (removable && !toggled)
        {
            base.StopInteraction(referenceCheck);
            IsBlocking = false;
        }
    }


}
