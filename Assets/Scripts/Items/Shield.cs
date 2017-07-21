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
        ward.SetWardActive(true);
        isBlocking = true;
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
    public override void OnGripPress(VR_Controller_Custom controller)
    {
        isBlocking = true;
        ward.SetWardActive(true);
    }

    public override void OnGripRelease(VR_Controller_Custom controller)
    {
        isBlocking = false;
        ward.SetWardActive(false);
    }

    public override void OnTriggerRelease(VR_Controller_Custom referenceCheck)
    {
        if (removable && !toggled)
        {
            base.OnTriggerRelease(referenceCheck);
            IsBlocking = false;
        }
    }


}
