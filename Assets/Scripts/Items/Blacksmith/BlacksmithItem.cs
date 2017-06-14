using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlacksmithItem : GenericItem {

    [SerializeField] protected PhysicalMaterial physicalMaterial;
    protected bool isColliding = false;
    [SerializeField] private float directionalMultiplier = 20, maxLerpForce = 40f;
    [SerializeField] private float collisionVibrationMagnitude = 0.8F;



    public override void OnTriggerRelease(VR_Controller_Custom referenceCheck)
    {
        base.OnTriggerRelease(referenceCheck);
        rigidBody.useGravity = true;
        rigidBody.velocity = referenceCheck.Velocity;
        rigidBody.angularVelocity = referenceCheck.AngularVelocity;
    }

    public override void OnTriggerPress(VR_Controller_Custom referenceCheck)
    {
        rigidBody.useGravity = false;
        base.OnTriggerPress(referenceCheck);
    }

    
    public override void OnTriggerHold(VR_Controller_Custom referenceCheck)
    {
        
            Vector3 PositionDelta = (referenceCheck.transform.position - transform.position);

            Quaternion RotationDelta = referenceCheck.transform.rotation * Quaternion.Inverse(this.transform.rotation);
            float angle;
            Vector3 axis;
            RotationDelta.ToAngleAxis(out angle, out axis);

            if (angle > 180)
                angle -= 360;

            float angularVelocityNumber = .2f;

            if (isColliding)
                angularVelocityNumber /= 2;


            rigidBody.angularVelocity = axis * angle * angularVelocityNumber;

            float currentForce = maxLerpForce;

            if (isColliding)
                currentForce /= 40;

            rigidBody.velocity = PositionDelta.magnitude * directionalMultiplier > currentForce ? (PositionDelta).normalized * currentForce : PositionDelta * directionalMultiplier;
   
        
    }

    //public override void UpdatePosition()
    //{
    //    Vector3 PositionDelta = (linkedController.transform.position - transform.position);

    //    Quaternion RotationDelta = linkedController.transform.rotation * Quaternion.Inverse(this.transform.rotation);
    //    float angle;
    //    Vector3 axis;
    //    RotationDelta.ToAngleAxis(out angle, out axis);

    //    if (angle > 180)
    //        angle -= 360;

    //    rigidBody.angularVelocity = axis * angle * 0.4f;

    //    rigidBody.velocity = PositionDelta * 40 * rigidBody.mass;
    //}

    protected override void OnCollisionEnter(Collision collision)
    {
        base.OnCollisionEnter(collision);
        if (currentInteractingController != null)
        {
            float value = currentInteractingController.Velocity.magnitude <= collisionVibrationMagnitude ? currentInteractingController.Velocity.magnitude : collisionVibrationMagnitude;
            currentInteractingController.Vibrate(value / 10);
            isColliding = true;
        }

    }

    protected virtual void OnCollisionStay(Collision collision)
    {
        if (currentInteractingController != null)
        {
            float value = Vector3.Distance(transform.rotation.eulerAngles, currentInteractingController.transform.rotation.eulerAngles);

            value = value <= 720 ? value : 720;

            currentInteractingController.Vibrate(value / 720 * collisionVibrationMagnitude);

            isColliding = true;
        }
    }

    protected virtual void OnCollisionExit(Collision collision)
    {
        isColliding = false;
    }




















}