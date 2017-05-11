using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Animator))]
[RequireComponent(typeof(Rigidbody))]
public abstract class ActorFSM : MonoBehaviour
{

    public enum FSMState //changed to fsm state
    {
        IDLE,
        PETROL,
        INTERACTION,
        INTRUSION,
        COMBAT
    }
    private AI currentAI;
    protected List<Vector3> path;
    protected bool requestedPath;
    [SerializeField]
    protected FSMState currentState;
    protected Actor target;
    protected Animator animator;
    protected float timer = 0;
    protected Rigidbody rigidBody;
    [SerializeField]
    protected Transform head;
    [SerializeField]
    protected float detectionDistance;

    public virtual void ChangeState(FSMState state)
    {
        currentState = state;
        timer = 0;
        animator.speed = 1;
    }

    protected virtual void Awake()
    {
        animator = GetComponent<Animator>();
        rigidBody = GetComponent<Rigidbody>();
        currentAI = GetComponent<AI>();
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

    public virtual void CheckHit(int index)
    {
        Debug.Log("Thrown");
        EquipSlot slot = EquipSlot.LEFTHAND;
        if (index == 0)
            slot = EquipSlot.LEFTHAND;
        else if (index == 1)
        {
            slot = EquipSlot.RIGHTHAND;
        }

        Vector3 dir = (target.transform.position - transform.position).normalized;
        float angle = Vector3.Angle(transform.forward, dir);
        if (target != null)
        {
            GenericItem currentItem = currentAI.returnWield(slot);
            
            if (currentItem != null)
            {
                Debug.Log("Thrown1");
                if (Mathf.Abs(angle) < 90 && Vector3.Distance(transform.position, target.transform.position) < currentAI.returnWield(slot).Range * 2)
                {
                    Debug.Log("Thrown2");
                    currentAI.Attack(currentItem, target);
                }
            }
        }
    }

}
