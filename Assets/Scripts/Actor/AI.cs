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
        currentFSM.State = ActorFSM.FSMState.INTERACTION;
    }
}
