using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Monster : AI {

    public override void Interact(Actor actor)
    {
        Debug.Log("Monsters cant interact");
        throw new NotImplementedException();//Monsters cant interact atm
    }


    
}
