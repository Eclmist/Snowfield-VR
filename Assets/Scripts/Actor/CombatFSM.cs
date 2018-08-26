using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public abstract class CombatFSM : MonoBehaviour {

    protected ActorFSM actorFSMReference;

    public abstract void UpdateCombatState();

    protected Animator animator;

    protected virtual void Awake()
    {
        animator = GetComponent<Animator>();
        actorFSMReference = GetComponent<ActorFSM>();
    }
}
