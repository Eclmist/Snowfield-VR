using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VR_ButtonBehindScroll : VR_Button {

    // If the distance between the last recorded trigger press and release is greater than this,
    // the button will not trigger
    [SerializeField] protected float distanceMoveThreshold;
    private Vector3 start;
    private bool clicked = false;

    

    protected override void OnTriggerPress()
    {
        start = currentInteractingController.transform.position;
        clicked = true;
    }


    protected override void OnTriggerRelease()
    {
        if(Vector3.Distance(currentInteractingController.transform.position,start) < distanceMoveThreshold && clicked)
        {
            clicked = false;
            base.OnTriggerRelease();
        }
    }

    

    


}
