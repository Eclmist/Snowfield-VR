using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(HingeJoint))]
public class RotatableObject : InteractableItem
{
    private HingeJoint joint;
    [SerializeField] private float minRotationalValue,maxRotationValue;
    [SerializeField] private Transform pivot;
    
    // Use this for initialization
    void Start()
    {
        joint = GetComponent<HingeJoint>();
        joint.useLimits = true;
        JointLimits limit = joint.limits;
        limit.max = maxRotationValue;
        limit.min = minRotationalValue;
        joint.limits = limit;
        if (pivot != null)
        {
            joint.anchor = pivot.localPosition;
            joint.axis = Vector3.Cross(pivot.forward, pivot.up);
        }
        else
            Debug.Log("Please attach a pivot point called Pivot and child it to the rotatable object");
        
    }

    // Update is called once per frame
    public override void UpdatePosition()
    {
        linkedController.Vibrate(rigidBody.velocity.magnitude);
        Vector3 PositionDelta = (linkedController.transform.position - transform.position);
        Vector3 velocity = PositionDelta * 20 * rigidBody.mass;
        velocity = velocity.magnitude <= 1 ? velocity : velocity.normalized * 1f;
        rigidBody.velocity = velocity;
    }
}
