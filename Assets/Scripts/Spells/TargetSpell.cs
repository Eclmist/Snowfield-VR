using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Target 


public class TargetSpell : Spell {




    private int castCount;

    protected override void Start()
    {
        castCount = 3;
    }

    protected override void Cast()
    {
        if (castCount > 0)
        {

            Debug.Log("Hello");
            Instantiate(spellPrefab);
        }
        else
        {
            //Stop Charge
        }
    }




}
