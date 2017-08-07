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
    protected override void OnGripPress()
    {
        isBlocking = true;
        ward.SetWardActive(true);
    }

	protected override void OnGripRelease()
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
