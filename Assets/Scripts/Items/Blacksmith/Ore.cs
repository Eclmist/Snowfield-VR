using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ore : GenericItem
{
    [SerializeField]
    private TYPE type;

    public TYPE Type
    {
        get { return this.type; }

    }

}


