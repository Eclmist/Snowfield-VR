using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlacksmithItem : GenericItem
{
    [SerializeField] protected PhysicalMaterial physicalMaterial;

    protected override void Start()
    {
        base.Start();
        jobType = JobType.BLACKSMITH;
    }
}