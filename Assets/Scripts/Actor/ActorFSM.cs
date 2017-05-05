using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
[RequireComponent(typeof(Animator))]
public abstract class ActorFSM : MonoBehaviour {

    public enum FSMState //changed to fsm state
    {
        IDLE,
        PETROL,
        INTERACTION,
        INTRUSION,
        COMBAT
    }

    protected Vector3[] path;
    protected bool requestedPath;
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
        UpdateFSMState();
    }

    protected virtual void UpdateFSMState()
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
            case FSMState.COMBAT:
                UpdateCombatState();
                break;
        }
    }

    protected abstract void UpdateIdleState();

    protected abstract void UpdatePetrolState();

    protected abstract void UpdateInteractionState();

    protected abstract void UpdateCombatState();

    protected void ChangePath(Vector3[] _path)
    {
        path = _path;
        requestedPath = false;
    }

}
