using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlacksmithItem : GenericItem
{

    //Comment empty Line
    [SerializeField] protected PhysicalMaterial physicalMaterial;
    


    protected override void Start()
    {
        base.Start();
        jobType = JobType.BLACKSMITH;
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




















}