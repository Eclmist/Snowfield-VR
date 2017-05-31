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
        QUESTING,
        INTERACTION,
        INTRUSION,
        COMBAT,
        DEATH
    }
    [SerializeField]
    protected Transform eye;
    [SerializeField]
    protected float detectionDistance = 10;
    protected AI currentAI;
    protected List<Vector3> path;
    protected bool requestedPath, pathFound;
    [SerializeField]
    protected FSMState currentState;
    protected Actor target;
    protected Animator animator;
    [SerializeField]
    protected float timer = 0;
    protected Rigidbody rigidBody;
    protected FSMState nextState;
    protected Weapon currentUseWeapon;
    #region Avoidance
    [Header("Avoidance")]
    [SerializeField]
    protected float minimumDistToAvoid;
    [SerializeField]
    protected LayerMask avoidanceIgnoreMask;
    #endregion

    public virtual void ChangeState(FSMState state)
    {

        pathFound = false;
        currentState = state;
        rigidBody.isKinematic = false;
        animator.speed = 1;
        animator.SetFloat("Speed", 0);

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
        if (!animator.GetCurrentAnimatorStateInfo(0).IsName("Death"))
        {
            UpdateFSMState();
            UpdateAnimatorState();
        }
        else if(animator.GetCurrentAnimatorStateInfo(0).IsName("Death") && animator.GetCurrentAnimatorStateInfo(0).normalizedTime > 1)
        {
            currentAI.PlayDeath();
        }
    }

    protected void UpdateAnimatorState()
    {
        if (currentState != FSMState.INTERACTION)
        {
            animator.SetBool("Interaction", false);
        }
        if (animator.GetCurrentAnimatorStateInfo(0).IsName("KnockBack"))
            animator.SetBool("KnockBack", false);
        if (!animator.GetCurrentAnimatorStateInfo(0).IsName("Attack") && !animator.GetCurrentAnimatorStateInfo(0).IsName("Cast"))
        {
            animator.SetBool("Attacking", false);
        }
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
        if (_path.Count > 0)
        {
            pathFound = true;
            path = _path;
            requestedPath = false;
        }
        else
        {
            pathFound = false;
            requestedPath = false;
        }
    }

    protected void ChangePath(Vector3 _path)
    {
        List<Vector3> newPath = new List<Vector3>();
        newPath.Add(_path);
        ChangePath(newPath);
    }

    public virtual void CheckHit()
    {
        Vector3 dir = target.transform.position - transform.position;
        dir.y = 0;
        dir.Normalize();
        float angle = Vector3.Angle(transform.forward, dir);
        if (target != null)
        {
            
            if (currentUseWeapon != null)
            {
                Vector3 temptarget = target.transform.position;
                temptarget.y = transform.position.y;
                if (Mathf.Abs(angle) < 45 && Vector3.Distance(transform.position, temptarget) < currentUseWeapon.Range)
                {
                    currentAI.Attack(currentUseWeapon, target);
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

    private void IfBlocked(IBlock block)
    {
        //if (block.Owner == target)
        //{
        animator.SetBool("KnockBack", true);
        EndAttack();
        //}
    }
    public virtual void StartAttack()
    {

        if (currentUseWeapon != null)
        {
            Debug.Log(currentUseWeapon);
            currentUseWeapon.SetBlockable(IfBlocked);
        }
    }

    public virtual void EndAttack()
    {
        if (currentUseWeapon != null)
        {
            Debug.Log("EndCharge");
            currentUseWeapon.EndCharge();
            currentUseWeapon.SetBlockable();
        }
    }

    protected bool CheckObstacles()
    {
        RaycastHit Hit;
        //Check that the vehicle hit with the obstacles within it's minimum distance to avoid
        Vector3 right45 = (eye.forward + eye.right).normalized;
        Vector3 left45 = (eye.forward - eye.right).normalized;

        if (Physics.Raycast(eye.position, right45, out Hit,
            minimumDistToAvoid, ~avoidanceIgnoreMask) || Physics.Raycast(eye.position, left45, out Hit,
            minimumDistToAvoid, ~avoidanceIgnoreMask))
        {

            return true;
        }
        else
            return false;
    }

    protected Vector3 AvoidObstacles(Vector3 endPoint)
    {
        RaycastHit Hit;
        //Check that the vehicle hit with the obstacles within it's minimum distance to avoid
        Vector3 right45 = (eye.forward + eye.right).normalized;
        Vector3 left45 = (eye.forward - eye.right).normalized;

        if (Physics.Raycast(eye.position, right45, out Hit,
            minimumDistToAvoid, ~avoidanceIgnoreMask))
        {
            // 0 if near, 1 if far
            float distanceExp = Vector3.Distance(Hit.point, eye.position) / minimumDistToAvoid;
            // 5 if near, 0 if far
            distanceExp = 5 - distanceExp * 5;
            return transform.forward - transform.right * distanceExp * currentAI.MovementSpeed;
        }
        else if (Physics.Raycast(eye.position, left45, out Hit,
            minimumDistToAvoid, ~avoidanceIgnoreMask))
        {
            // 0 if near, 1 if far
            float distanceExp = Vector3.Distance(Hit.point, eye.position) / minimumDistToAvoid;
            // 5 if near, 0 if far
            distanceExp = 5 - distanceExp * 5;
            return transform.forward + transform.right * currentAI.MovementSpeed * distanceExp;
        }

        else
        {
            Vector3 dir = (endPoint - transform.position).normalized;
            dir.y = 0;
            return dir * currentAI.MovementSpeed;
        }
    }

    public void DamageTaken(bool knockbacked,Actor attacker)
    {
        if (knockbacked)
        {
            animator.SetBool("KnockBack", true);
            EndAttack();
        }
        target = attacker;
        ChangeState(FSMState.COMBAT);
    }


    protected void LookAtPlayer(Vector3 endPoint)
    {
        float distance = (endPoint - transform.position).magnitude / 60;
        float angle = Vector3.Angle(endPoint - transform.position, transform.forward) / 180;
        //float rotationSpeed = turnRateOverAngle.Evaluate(angle);

        Vector3 lookAtTarget = AvoidObstacles(endPoint);
        lookAtTarget.y = 0; //Force no y change;
   
        
        transform.rotation = Quaternion.Slerp(transform.rotation,
            Quaternion.LookRotation(lookAtTarget, Vector3.up),
            5 * Time.deltaTime);//hardcorded 10
    }

    protected Vector3 GetReversePoint()
    {
        RaycastHit Hit;
        //Check that the vehicle hit with the obstacles within it's minimum distance to avoid
        Vector3 dir = -transform.forward;

        if (Physics.Raycast(transform.position, dir, out Hit,
            minimumDistToAvoid, ~avoidanceIgnoreMask))
        {

            return Hit.point;
        }
        else
            return (-transform.forward * minimumDistToAvoid) + transform.position;
    }

    public void StartCharge()
    {
        
        currentUseWeapon = (Weapon)currentAI.returnEquipment(animUseSlot);
        
        if (currentUseWeapon != null)
        {
            currentUseWeapon.StartCharge();
        }
    }
}
