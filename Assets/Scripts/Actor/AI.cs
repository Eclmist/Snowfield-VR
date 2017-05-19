using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public abstract class AI : Actor
{

    private ActorFSM currentFSM;
    protected bool isConversing;

    [SerializeField] protected float movementSpeed = 3;

    public float MovementSpeed
    {
        get
        {
            return movementSpeed;
        }
        set
        {
            movementSpeed = value;
        }
    }

    protected override void Awake()
    {
        base.Awake();
        currentFSM = GetComponent<ActorFSM>();
        if (jobList.Count != 0)
            jobList.Clear();
    }

    public bool IsConversing
    {
        get { return this.isConversing; }
        set { this.isConversing = true; }
    }


    public override void Notify()
    {
        currentFSM.ChangeState(ActorFSM.FSMState.INTERACTION);
    }

    public override void TakeDamage(int damage, Actor attacker)
    {
        base.TakeDamage(damage, attacker);
        if(health <= 0)
        {
            currentFSM.ChangeState(ActorFSM.FSMState.DEATH);
        }
        else if (Mathf.Sign(damage) == 1)
        {
            currentFSM.ChangeState(ActorFSM.FSMState.COMBAT);
            currentFSM.Target = attacker;
        }
    }

   


}
