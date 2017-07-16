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
    protected float detectionDistance = 10;
    protected AI currentAI;
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

    public virtual void ChangeState(FSMState state)
    {
        currentAI.Interacting = false;
        isHandlingAction = false;
        StopAllCoroutines();
        pathFound = false;
        currentState = state;
        rigidBody.isKinematic = false;
        animator.speed = 1;
        animator.SetFloat("Speed", 0);

        if (state != FSMState.EVENTHANDLING)
        {
            foreach(NodeEvent e in handledEvents)
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
        currentAI = GetComponent<AI>();
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
            UpdateAnimatorState();
        }
        else if (animator.GetCurrentAnimatorStateInfo(0).IsName("Death") && animator.GetCurrentAnimatorStateInfo(0).normalizedTime > 1)
        {
            currentAI.gameObject.SetActive(false);
        }
    }

    protected void UpdateAnimatorState()
    {

        if (animator.GetCurrentAnimatorStateInfo(0).IsName("KnockBack"))
            animator.SetBool("KnockBack", false);
        if (!animator.GetCurrentAnimatorStateInfo(0).IsName("Attack") && !animator.GetCurrentAnimatorStateInfo(0).IsName("Cast"))
        {
            animator.SetBool("Attacking", false);
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
                break;
            case FSMState.PETROL:
                UpdatePetrolState();
                break;
        }
    }

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
                if(firstEvent.Activated && (!firstEvent.Final || (firstEvent.Final && path.Count == 1)))
                    firstEvent.HandleEvent(currentAI);
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
                animator.SetFloat("Speed", 2);
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

    protected abstract void UpdateCombatState();


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
            //distanceExp = 5 - distanceExp * 5;
            return transform.forward - transform.right * distanceExp * currentAI.MovementSpeed;
        }
        else if (Physics.Raycast(eye.position, left45, out Hit,
            minimumDistToAvoid, ~avoidanceIgnoreMask))
        {
            // 0 if near, 1 if far
            float distanceExp = Vector3.Distance(Hit.point, eye.position) / minimumDistToAvoid;
            // 5 if near, 0 if far
            //distanceExp = 5 - distanceExp * 5;
            return transform.forward + transform.right * currentAI.MovementSpeed * distanceExp;
        }

        else
        {
            Vector3 dir = (endPoint - transform.position).normalized;
            dir = dir * currentAI.MovementSpeed;
            dir.y = rigidBody.velocity.y;
            return dir;        }
    }

    public void DamageTaken( Actor attacker)
    {
        ChangeState(FSMState.COMBAT);
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

    public void StartCharge()
    {

        currentUseWeapon = (Weapon)currentAI.returnEquipment(animUseSlot);

        if (currentUseWeapon != null)
        {
            currentUseWeapon.StartCharge();
        }
    }

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
