using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(HingeJoint))]
[RequireComponent(typeof(Rigidbody))]
public class RotatableObject : VR_Interactable_Object
{
    private HingeJoint joint;
    [Range(0,180)][Tooltip("Max rotation in degrees")][SerializeField]
    private float maxRotationValue = 90;
    [Range(-180, 0)]
    [Tooltip("Min rotation in degrees")]
    [SerializeField]
    private float minRotationalValue = -90;
    [SerializeField]
    private Transform pivot;
    [Range(0,150)][Tooltip("Multiplier of the force that is added to the object on movement")][SerializeField]
    private float force;
    // Use this for initialization
    protected override void Awake()
    {
        base.Awake();
        joint = GetComponent<HingeJoint>();
    }

    protected override void Start()
    {
        base.Start();
        joint.useLimits = true;
        JointLimits limit = joint.limits;
        limit.max = maxRotationValue;
        limit.min = minRotationalValue;
        joint.useSpring = true;
        joint.limits = limit;
        if (pivot == null)
            pivot = transform;

        joint.anchor = transform.InverseTransformPoint(pivot.transform.position);
        Vector3 rotationeuler = Vector3.Cross(pivot.forward, pivot.up);
        joint.axis = rotationeuler;
        
        
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

 

    public override void OnFixedUpdateInteraction (VR_Controller_Custom controller)
    {
        base.OnFixedUpdateInteraction(controller);
        controller.Vibrate(rigidBody.velocity.magnitude);
        Vector3 PositionDelta = (controller.transform.position - transform.position);
        Vector3 velocity = PositionDelta * 20 * rigidBody.mass;
        velocity = velocity.magnitude <= 1 ? velocity : velocity.normalized * 1f;
        rigidBody.velocity = velocity;
    }





}
