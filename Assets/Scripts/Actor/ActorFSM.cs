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
        COMBAT,
        DEATH
    }
    protected AI currentAI;
    protected List<Vector3> path;
    protected bool requestedPath;
    [SerializeField]
    protected FSMState currentState;
    protected Actor target;
    protected Animator animator;
    protected float timer = 0;
    protected Rigidbody rigidBody;
    [SerializeField]
    protected float detectionDistance;

    public virtual void ChangeState(FSMState state)
    {
        currentState = state;
        timer = 0;
        animator.speed = 1;
        switch (state)
        {
            case FSMState.DEATH:
                animator.SetBool("Death", true);
                break;
            default:
                break;
        }
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
        rigidBody.velocity = Vector3.zero;
        rigidBody.angularVelocity = Vector3.zero;
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


    protected EquipSlot.EquipmentSlotType animUseSlot;
    protected void ChangePath(List<Vector3> _path)
    {
        path = _path;
        requestedPath = false;
    }

    public virtual void CheckHit()
    {
        Vector3 dir = target.transform.position - transform.position;
        dir.y = 0;
        dir.Normalize();
        float angle = Vector3.Angle(transform.forward, dir);
        if (target != null)
        {
            Weapon currentWeapon = (Weapon)currentAI.returnEquipment(animUseSlot);

            if (currentWeapon != null)
            {
                if (Mathf.Abs(angle) < 90 && Vector3.Distance(transform.position, target.transform.position) < currentWeapon.Range)
                {
                    currentAI.Attack(currentWeapon, target);
                }
            }
        }
    }

    public virtual void SetAnimUseSlot(int i)
    {
        switch (i)
        {
            case 0:
                animUseSlot = EquipSlot.EquipmentSlotType.LEFTHAND;
                break;
            case 1:
                animUseSlot = EquipSlot.EquipmentSlotType.RIGHTHAND;
                break;

        }
    }

}
