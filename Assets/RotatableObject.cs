using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(HingeJoint))]
public class RotatableObject : InteractableItem
{
    private HingeJoint joint;
    [SerializeField]
    private float minRotationalValue,maxRotationValue;
    // Use this for initialization
    void Start()
    {
        joint = GetComponent<HingeJoint>();
        joint.useLimits = true;
        JointLimits limit = joint.limits;
        limit.max = maxRotationValue;
        limit.min = minRotationalValue;
        joint.limits = limit;
        Transform pivotPoint = transform.FindChild("Pivot");
        if (pivotPoint != null)
            joint.anchor = pivotPoint.localPosition;
        else
            Debug.Log("Please attach a pivot point called Pivot and child it to the rotatable object");
    }

    // Update is called once per frame
    //public override void UpdatePosition()
    //{
    //    linkedController.Vibrate(rigidBody.velocity.magnitude);
    //    Vector3 PositionDelta = (linkedController.transform.position - transform.position);
    //    Vector3 velocity = PositionDelta * 20 * rigidBody.mass;
    //    velocity = velocity.magnitude <= 1 ? velocity : velocity.normalized * 1f;
    //    rigidBody.velocity = velocity;
    //}

    public override void Interact(VR_Controller_Custom referenceCheck)
    {
        base.Interact(referenceCheck);
        if (linkedController != null)
        {
            linkedController.Vibrate(rigidBody.velocity.magnitude);
            Vector3 PositionDelta = (linkedController.transform.position - transform.position);
            Vector3 velocity = PositionDelta * 20 * rigidBody.mass;
            velocity = velocity.magnitude <= 1 ? velocity : velocity.normalized * 1f;
            rigidBody.velocity = velocity;
        }

        //Vector3 localVelocity = transform.InverseTransformDirection(rigidBody.velocity);

        //Vector3 nextPosition = localVelocity * Time.deltaTime + transform.position;

        //Vector3 positionalOffset = transform.InverseTransformDirection(nextPosition - startPosition);

        //if (Mathf.Abs(positionalOffset.x) > maxDisplacement.x)
        //    localVelocity.x = 0;
        //if (Mathf.Abs(positionalOffset.y) > maxDisplacement.y)
        //    localVelocity.y = 0;
        //if (Mathf.Abs(positionalOffset.z) > maxDisplacement.z)
        //    localVelocity.z = 0;

        //rigidBody.velocity = transform.TransformDirection(localVelocity);


    }
}
