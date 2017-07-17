using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(ConfigurableJoint))]
[RequireComponent(typeof(Rigidbody))]
public class DisplacableObject : VR_Interactable_Object
{

    [SerializeField]
    [Tooltip("The axis which the object is locked in")]
    private bool[] axisLock = new bool[3];
    [SerializeField]
    [Tooltip("The maxdistance the object can travel in its unlocked axis")]
    private float maxDistance;
    private ConfigurableJoint joint;
    [SerializeField]
    [Tooltip("The rigidbody this object is bound to")]
    private Rigidbody boundRigidBody;


    protected override void Awake()
    {
        joint = GetComponent<ConfigurableJoint>();
    }
    // Use this for initialization
    void Start()
    {

        if (boundRigidBody != null)
            joint.connectedBody = boundRigidBody;

        SoftJointLimit limit = joint.linearLimit;
        limit.limit = maxDistance / 2;
        joint.linearLimit = limit;
        joint.xMotion = axisLock[0] ? ConfigurableJointMotion.Locked : ConfigurableJointMotion.Limited;
        joint.yMotion = axisLock[1] ? ConfigurableJointMotion.Locked : ConfigurableJointMotion.Limited;
        joint.zMotion = axisLock[2] ? ConfigurableJointMotion.Locked : ConfigurableJointMotion.Limited;
        joint.angularXMotion = ConfigurableJointMotion.Locked;
        joint.angularYMotion = ConfigurableJointMotion.Locked;
        joint.angularZMotion = ConfigurableJointMotion.Locked;
    }


    public override void OnTriggerHold(VR_Controller_Custom controller)
    {
        if (currentInteractingController != null)
        {
            controller.Vibrate(rigidBody.velocity.magnitude / 5 * 10);

            Vector3 PositionDelta = (controller.transform.position - transform.position);

            rigidBody.velocity = PositionDelta * 20 * rigidBody.mass;
        }
    }
}
