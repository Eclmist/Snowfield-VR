using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlacksmithItem : GenericItem {

    [SerializeField] protected PhysicalMaterial physicalMaterial;
    protected bool isColliding = false;
    [SerializeField] private float directionalMultiplier = 20;
    protected float maxForce = 40f;

    private void Start()
    {
        jobType = JobType.BLACKSMITH;
        
    }

    public override void StopInteraction(VR_Controller_Custom referenceCheck)
    {
        rigidBody.useGravity = true;
        base.StopInteraction(referenceCheck);
        rigidBody.velocity = referenceCheck.Velocity();
        rigidBody.angularVelocity = referenceCheck.AngularVelocity();
    }

    public override void StartInteraction(VR_Controller_Custom referenceCheck)
    {
        rigidBody.useGravity = false;
        base.StartInteraction(referenceCheck);
    }

    
    public override void Interact(VR_Controller_Custom referenceCheck)
    {
        base.Interact(referenceCheck);
        if (LinkedController != null)
        {
            Vector3 PositionDelta = (linkedController.transform.position - transform.position);

            Quaternion RotationDelta = linkedController.transform.rotation * Quaternion.Inverse(this.transform.rotation);
            float angle;
            Vector3 axis;
            RotationDelta.ToAngleAxis(out angle, out axis);

            if (angle > 180)
                angle -= 360;

            float angularVelocityNumber = .2f;

            if (isColliding)
                angularVelocityNumber /= 2;


            rigidBody.angularVelocity = axis * angle * angularVelocityNumber;

            float currentForce = maxForce;

            if (isColliding)
                currentForce /= 40;

            rigidBody.velocity = PositionDelta.magnitude * directionalMultiplier > currentForce ? (PositionDelta).normalized * currentForce : PositionDelta * directionalMultiplier;
   
        }
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
        if (linkedController != null)
        {
            float value = linkedController.Velocity().magnitude <= 1 ? linkedController.Velocity().magnitude : 1;
            linkedController.Vibrate(value / 10);
            isColliding = true;
        }

    }

    protected virtual void OnCollisionStay(Collision collision)
    {
        if (linkedController != null)
        {
            float value = Vector3.Distance(transform.rotation.eulerAngles, linkedController.transform.rotation.eulerAngles);

            value = value <= 720 ? value : 720;

            linkedController.Vibrate(value / 720 * 5);

            isColliding = true;
        }
    }

    protected virtual void OnCollisionExit(Collision collision)
    {
        isColliding = false;
    }




















}