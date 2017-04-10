using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
[RequireComponent(typeof(Animator))]
public class ActorFSM : Actor {

    public enum FSMState //changed to fsm state
    {
        IDLE,
        PETROL,
        INTERACTION,
        INTRUSION,
        COMBAT
    }

    protected FSMState currentState;
    protected Rigidbody rigidBody;
    protected Animator animator;
    // Use this for initialization

    public FSMState State
    {
        get
        {
            return currentState;
        }
        set
        {
            currentState = value;
        }
    }
    
    protected virtual void Awake()
    {
        rigidBody = GetComponent<Rigidbody>();
        animator = GetComponent<Animator>();
    }

    protected virtual void Update()
    {
        switch (currentState)
        {
            case FSMState.IDLE:
                UpdateIdleState();
                break;
            case FSMState.PETROL:
                UpdatePetrolState();
                break;
            case FSMState.INTERACTION:
                UpdateInteractionState();
                break;
            case FSMState.INTRUSION:
                UpdateIntrusionState();
                break;
            case FSMState.COMBAT:
                UpdateCombatState();
                break;
        }
    }

    protected virtual void UpdateIdleState()
    {

    }

    protected virtual void UpdatePetrolState()
    {

    }

    protected virtual void UpdateInteractionState()
    {

    }

    protected virtual void UpdateIntrusionState()
    {

    }

    protected virtual void UpdateCombatState()
    {

    }
}
