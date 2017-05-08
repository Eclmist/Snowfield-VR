using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public abstract class AI : Actor {

    private ActorFSM currentFSM;

    protected virtual void Awake()
    {
        currentFSM = GetComponent<ActorFSM>();
        if (jobList.Count != 0)
            jobList.Clear();
    }

    public override void Notify()
    {
        currentFSM.ChangeState(ActorFSM.FSMState.INTERACTION);
    }

    public override void TakeDamage(int damage,Actor attacker)
    {
        base.TakeDamage(damage,attacker);
        if(Mathf.Sign(damage) == -1)
        {
            currentFSM.ChangeState(ActorFSM.FSMState.COMBAT);
            currentFSM.Target = attacker;
        }
    }
}
