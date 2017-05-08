using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(ConfigurableJoint))]
public class DisplacableObject : InteractableItem {

    [SerializeField]
    private bool[] axisLock = new bool[3];
    [SerializeField]
    private float maxDistance;
    private ConfigurableJoint joint;
   
	// Use this for initialization
	void Start () {
        joint = GetComponent<ConfigurableJoint>();
        Rigidbody parentRigidBody = GetComponentInParent<Rigidbody>();
        if (parentRigidBody != rigidBody)
            joint.connectedBody = parentRigidBody;
        SoftJointLimit limit = joint.linearLimit;
        limit.limit = maxDistance/2;
        joint.linearLimit = limit;
        joint.xMotion = axisLock[0] ? ConfigurableJointMotion.Locked : ConfigurableJointMotion.Limited;
        joint.yMotion = axisLock[1] ? ConfigurableJointMotion.Locked : ConfigurableJointMotion.Limited;
        joint.zMotion = axisLock[2] ? ConfigurableJointMotion.Locked : ConfigurableJointMotion.Limited;
        joint.angularXMotion = ConfigurableJointMotion.Locked;
        joint.angularYMotion = ConfigurableJointMotion.Locked;
        joint.angularZMotion = ConfigurableJointMotion.Locked;
    }

    //protected override void Update()
    //{
    //    base.Update();
    //    if(linkedController == null)
    //    {

    //        Vector3 localVelocity = transform.InverseTransformDirection(rigidBody.velocity);

    //        Vector3 nextPosition = localVelocity * Time.deltaTime + transform.position;

    //        Vector3 positionalOffset = transform.InverseTransformDirection(nextPosition - startPosition);

    //        rigidBody.velocity = transform.TransformDirection(localVelocity);

    //        Vector3 maxDisplacementDirection = maxDisplacement.normalized;

    //        if (Mathf.Abs(positionalOffset.x) > maxDisplacement.x)
    //            localVelocity.x = localVelocity.x/ -2 * maxDisplacement.x;
    //        if (Mathf.Abs(positionalOffset.y) > maxDisplacement.y)
    //            localVelocity.y = localVelocity.y / -2 * maxDisplacement.y;
    //        if (Mathf.Abs(positionalOffset.z) > maxDisplacement.z)
    //            localVelocity.z = localVelocity.z / -2 * maxDisplacement.z;

    //        rigidBody.velocity = transform.TransformDirection(localVelocity);
    //    }
    //}

    // Update is called once per frame

    public override void Interact(VR_Controller_Custom referenceCheck)
    {
        base.Interact(referenceCheck);
        if (linkedController != null)
        {
            linkedController.Vibrate(rigidBody.velocity.magnitude / 5 * 10);

            Vector3 PositionDelta = (linkedController.transform.position - transform.position);

            rigidBody.velocity = PositionDelta * 20 * rigidBody.mass;
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
