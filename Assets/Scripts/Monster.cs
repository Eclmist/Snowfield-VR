using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Monster : AI {

    [SerializeField]
    protected ActorData data;

    public override ActorData Data
    {
        get
        {
            return data;
        }

        set
        {
            data = value;
        }
    }
    public override void Interact(Actor actor)
    {
        Debug.Log("Monsters cant interact");
        throw new NotImplementedException();//Monsters cant interact atm
    }

    public override void Despawn()
    {
        base.Despawn();
        Destroy(gameObject, 10);
    }




}
