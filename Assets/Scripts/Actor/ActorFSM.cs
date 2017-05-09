using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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

    protected List<Vector3> path;
    protected bool requestedPath;
    protected FSMState currentState;
    protected Actor target;
    protected Animator animator;
    protected float timer = 0;

    [SerializeField] protected Transform head;


    public virtual void ChangeState(FSMState state)
    {
        currentState = state;
        timer = 0;
        animator.speed = 1;
    }

    protected virtual void Awake()
    {
        animator = GetComponent<Animator>();
    }
    public Actor Target
    {
        get
        {
            return target;
        }
        set
        {
            target = value;
        }
    }
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

    protected void ChangePath(List<Vector3> _path)
    {
        path = _path;
        requestedPath = false;
    }

}
