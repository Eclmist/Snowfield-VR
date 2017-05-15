using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(HingeJoint))]
[RequireComponent(typeof(Rigidbody))]
public class RotatableObject : MonoBehaviour, IInteractable
{
    private HingeJoint joint;
    private VR_Controller_Custom linkedController;
    private Rigidbody rigidBody;
    [SerializeField]
    private float minRotationalValue, maxRotationValue;
    [SerializeField]
    private Transform pivot;
    [SerializeField]
    private float force;
    // Use this for initialization
    void Awake()
    {
        rigidBody = GetComponent<Rigidbody>();
        joint = GetComponent<HingeJoint>();
    }

    void Start()
    {
        joint.useLimits = true;
        JointLimits limit = joint.limits;
        limit.max = maxRotationValue;
        limit.min = minRotationalValue;
        joint.useSpring = true;
        joint.limits = limit;
        if (pivot != null)
        {
            joint.anchor = pivot.localPosition;
            Vector3 rotationeuler = Vector3.Cross(pivot.forward, pivot.up);
            joint.axis = rotationeuler;
        }
        else
            Debug.Log("Please attach a pivot point called Pivot and child it to the rotatable object");
    }

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
    // Update is called once per frame
    //public override void UpdatePosition()
    //{
    //    linkedController.Vibrate(rigidBody.velocity.magnitude);
    //    Vector3 PositionDelta = (linkedController.transform.position - transform.position);
    //    Vector3 velocity = PositionDelta * 20 * rigidBody.mass;
    //    velocity = velocity.magnitude <= 1 ? velocity : velocity.normalized * 1f;
    //    rigidBody.velocity = velocity;
    //}

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
            linkedController.Vibrate(rigidBody.velocity.magnitude);
            Vector3 PositionDelta = (linkedController.transform.position - transform.position);
            Vector3 velocity = PositionDelta * 20 * rigidBody.mass;
            velocity = velocity.magnitude <= 1 ? velocity : velocity.normalized * 1f;
            rigidBody.velocity = velocity;
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
