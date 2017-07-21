using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ore : GenericItem
{
    [SerializeField]
    private PhysicalMaterial.Type type;

    public PhysicalMaterial.Type Type
    {
        get { return this.type; }

    }

}


