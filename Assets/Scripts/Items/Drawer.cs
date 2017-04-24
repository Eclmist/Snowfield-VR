using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Drawer : GenericItem {

    [SerializeField]
    private Vector3 maxDisplacement;
    private Vector3 startPosition;
	// Use this for initialization
	void Start () {
        startPosition = transform.localPosition;
	}
	
	// Update is called once per frame
    public override void StopInteraction(VR_Controller_Custom referenceCheck)
    {
        base.StopInteraction(referenceCheck);
        rigidBody.velocity = Vector3.zero;
    }
    public override void UpdatePosition()
    {
        Vector3 PositionDelta = (linkedController.transform.position - transform.position);

        rigidBody.velocity = PositionDelta * 4000 * rigidBody.mass * Time.fixedDeltaTime;

        Vector3 localVelocity = transform.InverseTransformDirection(rigidBody.velocity);

        if (Mathf.Abs(localVelocity.x * Time.fixedDeltaTime + transform.localPosition.x) > startPosition.x + maxDisplacement.x)
            localVelocity.x = 0;
        if (Mathf.Abs(localVelocity.y * Time.deltaTime) + transform.localPosition.y > startPosition.y + maxDisplacement.y)
            localVelocity.y = 0;
        if (Mathf.Abs(localVelocity.z * Time.deltaTime) + transform.localPosition.z > startPosition.z + maxDisplacement.z)
            localVelocity.z = 0;

        rigidBody.velocity = transform.TransformDirection(localVelocity);

        linkedController.Vibrate(rigidBody.velocity.magnitude / 5 * 10);
    }
}
