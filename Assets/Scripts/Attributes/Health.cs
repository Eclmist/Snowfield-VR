using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Health : Attribute {

    public Health()
    {
        name = "Health";
        baseColor = Color.red;
    }

    protected override void ApplyAttributeEffect()
    {
        
    }
}
