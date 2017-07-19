using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Animator))]
[RequireComponent(typeof(Rigidbody))]
[RequireComponent(typeof(CapsuleCollider))]

public abstract class ActorFSM : MonoBehaviour
{

    public enum FSMState //changed to fsm state
    {
        IDLE,
        PETROL,
        COMBAT,
        EVENTHANDLING,
        SHOPPING,
        DEATH
    }
    protected CapsuleCollider capsuleCollider;
    protected bool isHandlingAction = false;
    [SerializeField]
    protected Transform eye;

    [SerializeField]
    protected float detectionDistance = 10, attackRange = 0;
    protected Vector3 targetPoint = Vector3.zero;
    protected List<Node> path;
    protected bool requestedPath, pathFound;
    [SerializeField]
    protected FSMState currentState;
    protected Actor target;
    protected Animator animator;
    protected Rigidbody rigidBody;
    protected FSMState nextState;
    protected Weapon currentUseWeapon;
    protected List<NodeEvent> handledEvents = new List<NodeEvent>();


    #region Avoidance
    [Header("Avoidance")]
    [SerializeField]
    protected float minimumDistToAvoid;
    [SerializeField]
    protected LayerMask avoidanceIgnoreMask;
    #endregion

    public abstract AI CurrentAI
    {
        get;
    }
    public virtual void ChangeState(FSMState state)
    {
        CurrentAI.Interacting = false;
        isHandlingAction = false;
        StopAllCoroutines();
        pathFound = false;
        currentState = state;
        rigidBody.isKinematic = false;
        animator.speed = 1;
        animator.SetFloat("Speed", 0);
        animator.SetBool("Attacking", false);

        if (state != FSMState.EVENTHANDLING)
        {
            foreach (NodeEvent e in handledEvents)
            {
                e.CurrentNode.Occupied = false;
            }
            handledEvents.Clear();
        }

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
        capsuleCollider = GetComponent<CapsuleCollider>();
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
        }
        else if (animator.GetCurrentAnimatorStateInfo(0).IsName("Death") && animator.GetCurrentAnimatorStateInfo(0).normalizedTime > 1)
        {
            CurrentAI.Despawn();
        }
    }

    protected virtual void UpdateFSMState()
    {
        rigidBody.velocity = new Vector3(0, rigidBody.velocity.y, 0);
        rigidBody.angularVelocity = Vector3.zero;
        switch (currentState)
        {
            case FSMState.EVENTHANDLING:
                UpdateEventHandling();
                return;
            case FSMState.PETROL:
                UpdatePetrolState();
                return;
            case FSMState.COMBAT:
                UpdateCombatState();
                return;
        }
    }

    private bool backing = false;

    protected virtual void UpdateCombatState()
    {
        if (target != null)
        {

            Vector3 temptarget = target.transform.position;
            temptarget.y = transform.position.y;
            Vector3 dir = temptarget - transform.position;
            float distance = Vector3.Distance(temptarget, transform.position);
            float angle = Vector3.Angle(transform.forward, dir);


            //Debug.DrawRay(head.transform.position - head.right, _direction);
            if (distance < detectionDistance)
            {
                Debug.Log(distance);
                if (backing)
                {
                    animator.SetFloat("Speed", CurrentAI.MovementSpeed);
                    targetPoint = transform.position - dir;
                    if (distance > attackRange / 2.0)
                        backing = false;
                }
                else if (!backing && distance < attackRange / 5.0)
                {
                    animator.SetBool("Attacking", false);
                    animator.SetFloat("Speed", CurrentAI.MovementSpeed);
                    targetPoint = transform.position - dir;
                    backing = true;
                }
                else if (distance > attackRange || Mathf.Abs(angle) > 45)
                {
                    animator.SetBool("Attacking", false);
                    animator.SetFloat("Speed", CurrentAI.MovementSpeed);
                    targetPoint = target.transform.position;
                }
                else
                {
                    animator.SetFloat("Speed", 0);
                    HandleCombatAction();

                }

            }
            else
            {
                ChangeState(FSMState.IDLE);
                return;
            }
        }
        else
        {
            ChangeState(FSMState.IDLE);
            return;
        }

        if (animator.GetCurrentAnimatorStateInfo(0).IsName("Petrol"))
        {
            rigidBody.velocity = AvoidObstacles(targetPoint);
            LookAtPlayer(targetPoint);
            return;
        }
    }

    protected abstract void HandleCombatAction();

    protected virtual void UpdateEventHandling()
    {

        if (!isHandlingAction)
        {
            if (handledEvents.Count <= 0)
            {
                path[0].Occupied = false;
                path.RemoveAt(0);
                ChangeState(FSMState.PETROL);
                return;
            }
            else
            {
                NodeEvent firstEvent = handledEvents[0];
                handledEvents.RemoveAt(0);
                if (firstEvent.Activated && (!firstEvent.Final || (firstEvent.Final && path.Count == 1)))
                    firstEvent.HandleEvent(CurrentAI);
            }
        }
    }

    protected virtual void UpdatePetrolState()
    {
        if (path.Count == 0)
        {
            ChangeState(nextState);
        }
        else
        {
            Vector3 targetPoint = path[0].Position;
            targetPoint.y = transform.position.y;
            if (path.Count == 1 && path[0].Occupied)
            {
                animator.SetFloat("Speed", 0);
            }
            else
            {
                animator.SetFloat("Speed", CurrentAI.MovementSpeed);
                if (animator.GetCurrentAnimatorStateInfo(0).IsName("Petrol") || animator.GetAnimatorTransitionInfo(0).IsName("Idle -> Petrol"))
                {
                    rigidBody.velocity = AvoidObstacles(targetPoint);
                }
                LookAtPlayer(targetPoint);

                if (Vector3.Distance(transform.position, targetPoint) < .25f)
                {

                    handledEvents.AddRange(path[0].Events);
                    if (handledEvents.Count > 0)
                    {
                        path[0].Occupied = true;
                        ChangeState(FSMState.EVENTHANDLING);
                    }
                    else
                        path.RemoveAt(0);
                }
            }
        }
    }

    protected EquipSlot.EquipmentSlotType animUseSlot;

    protected void ChangePath(List<Node> _path)
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

    protected void ChangePath(Node _path)
    {
        List<Node> newPath = new List<Node>();
        newPath.Add(_path);
        ChangePath(newPath);
    }

    //public virtual void CheckHit()
    //{
    //    Vector3 dir = target.transform.position - transform.position;
    //    dir.y = 0;
    //    dir.Normalize();
    //    float angle = Vector3.Angle(transform.forward, dir);
    //    if (target != null)
    //    {

    //        if (currentUseWeapon != null)
    //        {
    //            Vector3 temptarget = target.transform.position;
    //            temptarget.y = transform.position.y;
    //            if (Mathf.Abs(angle) < 45 && Vector3.Distance(transform.position, temptarget) < currentUseWeapon.Range)
    //            {
    //                currentAI.Attack(currentUseWeapon, target);
    //            }
    //        }
    //    }
    //}

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
            //distanceExp = 5 - distanceExp * 5;
            return transform.forward - transform.right * distanceExp * CurrentAI.MovementSpeed;
        }
        else if (Physics.Raycast(eye.position, left45, out Hit,
            minimumDistToAvoid, ~avoidanceIgnoreMask))
        {
            // 0 if near, 1 if far
            float distanceExp = Vector3.Distance(Hit.point, eye.position) / minimumDistToAvoid;
            // 5 if near, 0 if far
            //distanceExp = 5 - distanceExp * 5;
            return transform.forward + transform.right * CurrentAI.MovementSpeed * distanceExp;
        }

        else
        {
            Vector3 dir = (endPoint - transform.position).normalized;
            dir = dir * CurrentAI.MovementSpeed;
            dir.y = rigidBody.velocity.y;
            return dir;
        }
    }

    public void DamageTaken(Actor attacker)
    {
        ChangeState(FSMState.COMBAT);
        target = attacker;
        Debug.Log("DamageTaken");
    }


    protected void LookAtPlayer(Vector3 endPoint)
    {
        //float distance = (endPoint - transform.position).magnitude / 60;
        //float angle = Vector3.Angle(endPoint - transform.position, transform.forward) / 180;
        //float rotationSpeed = turnRateOverAngle.Evaluate(angle);

        Vector3 lookAtTarget = AvoidObstacles(endPoint);
        lookAtTarget.y = 0; //Force no y change;

        if (lookAtTarget != Vector3.zero)
            transform.rotation = Quaternion.Slerp(transform.rotation,
                Quaternion.LookRotation(lookAtTarget, Vector3.up),
                5 * Time.deltaTime);//hardcorded 10
    }

    //protected Vector3 GetReversePoint()
    //{
    //    RaycastHit Hit;
    //    //Check that the vehicle hit with the obstacles within it's minimum distance to avoid
    //    Vector3 dir = -transform.forward;

    //    if (Physics.Raycast(transform.position, dir, out Hit,
    //        minimumDistToAvoid, ~avoidanceIgnoreMask))
    //    {

    //        return Hit.point;
    //    }
    //    else
    //        return (-transform.forward * minimumDistToAvoid) + transform.position;
    //}



    public IEnumerator LookAtTransform(Transform targetTransform, float seconds, float angle)
    {
        while (seconds >= 0)
        {
            isHandlingAction = true;

            Vector3 dir = targetTransform.position - transform.position;
            dir.y = 0;
            transform.rotation = Quaternion.Slerp(transform.rotation,
                Quaternion.LookRotation(dir, Vector3.up),
                5 * Time.deltaTime);//hardcorded 10
            yield return new WaitForEndOfFrame();
            seconds -= Time.deltaTime;
        }
        isHandlingAction = false;

    }

}
