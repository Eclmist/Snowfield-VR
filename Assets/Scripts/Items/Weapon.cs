using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Weapon : Equipment {

    [SerializeField]
    protected float range;

    public float Range
    {
        get
        {
            return range;
        }
        set
        {
            range = value;
        }
    }

    protected override void UseItem()
    {
        base.UseItem();
    }




}
