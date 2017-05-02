using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlacksmithItem : GenericItem{

    [SerializeField]
    protected PhysicalMaterial physicalMaterial;
    

    public override void UpdatePosition()
    {
        throw new NotImplementedException();
    }
}
