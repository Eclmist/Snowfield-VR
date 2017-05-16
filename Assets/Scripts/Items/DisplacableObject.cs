using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(ConfigurableJoint))]
[RequireComponent(typeof(Rigidbody))]
public class DisplacableObject : MonoBehaviour, IInteractable
{

    [SerializeField]
    [Tooltip("The axis which the object is locked in")]
    private bool[] axisLock = new bool[3];
    [SerializeField]
    [Tooltip("The maxdistance the object can travel in its unlocked axis")]
    private float maxDistance;
    private ConfigurableJoint joint;
    private VR_Controller_Custom linkedController;
    [SerializeField]
    [Tooltip("The rigidbody this object is bound to")]
    private Rigidbody boundRigidBody;
    private Rigidbody rigidBody;

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

    void Awake()
    {
        rigidBody = GetComponent<Rigidbody>();
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



    public virtual void Interact(VR_Controller_Custom referenceCheck)
    {
        if (referenceCheck.Device.GetTouchDown(SteamVR_Controller.ButtonMask.Trigger))
        {
            StartInteraction(referenceCheck);
        }
        else if (referenceCheck.Device.GetTouchUp(SteamVR_Controller.ButtonMask.Trigger))
        {
            StopInteraction(referenceCheck);
        }
        else if (referenceCheck.Device.GetTouch(SteamVR_Controller.ButtonMask.Trigger))
        {
            linkedController.Vibrate(rigidBody.velocity.magnitude / 5 * 10);

            Vector3 PositionDelta = (linkedController.transform.position - transform.position);

            rigidBody.velocity = PositionDelta * 20 * rigidBody.mass;
        }
    }

    public virtual void StopInteraction(VR_Controller_Custom referenceCheck)
    {
        if (linkedController == referenceCheck)
        {
            linkedController = null;
            referenceCheck.SetInteraction(null);

            rigidBody.velocity = referenceCheck.Device.velocity;
            rigidBody.angularVelocity = referenceCheck.Device.angularVelocity;
        }
    }

    public virtual void StartInteraction(VR_Controller_Custom referenceCheck)
    {
        if (linkedController != null && linkedController != referenceCheck)
            linkedController.SetInteraction(null);

        linkedController = referenceCheck;
    }
}
