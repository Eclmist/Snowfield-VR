using UnityEngine;

[RequireComponent(typeof(AdventurerAI))]
public class AdventurerFSM : ActorFSM
{

    protected virtual void UpdateIntrusionState()
    {
    }

    protected override void UpdateFSMState()
    {
        base.UpdateFSMState();
        switch (currentState)
        {
            case FSMState.INTRUSION:
                UpdateInteractionState();
                break;
        }
    }

    public override void ChangeState(FSMState state)
    {
        base.ChangeState(state);

        if (state != FSMState.COMBAT)
            (currentAI as AdventurerAI).UnEquipWeapons();

        switch (state)
        {
            case FSMState.IDLE:
                timer = UnityEngine.Random.Range(1.0f, 5.0f);

                break;

            case FSMState.PETROL:
                animator.SetFloat("Speed", 2);
                break;

            case FSMState.INTERACTION:
                animator.SetBool("Interaction", true);
                break;

            case FSMState.COMBAT:
                (currentAI as AdventurerAI).EquipRandomWeapons();
                animator.SetBool("KnockBack", true);//Wrong place
                animator.speed = 1 + (currentAI.GetJob(JobType.ADVENTURER).Level * .1f);
                timer = 5;
                break;

        }
    }

    protected override void UpdateIdleState()
    {
        if (timer <= 0)
        {

            if (pathFound)
            {
                ChangeState(FSMState.PETROL);

            }
            else if (!requestedPath)
            {
                Shop TargetShop = TownManager.Instance.GetRandomShop();
                if (TargetShop != null && TargetShop.Owner != null)
                {

                    target = TargetShop.Owner;
                    AStarManager.Instance.RequestPath(transform.position, TargetShop.Location.position, ChangePath);
                    requestedPath = true;
                }
            }
        }
        else
        {
            timer -= Time.deltaTime;
        }
    }

    protected override void UpdateCombatState()
    {
        if (timer > 0 && target != null)
        {
            Vector3 dir = target.transform.position - transform.position;
            float distance = Vector3.Distance(target.transform.position, transform.position);
            dir.y = 0;
            dir.Normalize();


            float angle = Vector3.Angle(transform.forward, dir);

            //Debug.DrawRay(head.transform.position - head.right, _direction);
            if (distance < detectionDistance)
            {
                if (animator.GetCurrentAnimatorStateInfo(0).IsName("Petrol"))
                {
                    Quaternion _lookRotation = Quaternion.LookRotation(dir);
                    _lookRotation.z = _lookRotation.x = 0;
                    float y = transform.rotation.y;
                    transform.rotation = Quaternion.Slerp(transform.rotation, _lookRotation, Time.deltaTime * 10);
                    Debug.Log(y - transform.rotation.y);
                    rigidBody.velocity = dir * currentAI.MovementSpeed;
                }

                else if (animator.GetCurrentAnimatorStateInfo(0).IsName("Attack"))
                {
                    IBlockable item = (Weapon)currentAI.returnEquipment(animUseSlot);
                    if (item != null && item.Blocked)
                        animator.SetBool("KnockBack", true);
                }
                else if (animator.GetCurrentAnimatorStateInfo(0).IsName("KnockBack"))
                {
                    IBlockable item = (Weapon)currentAI.returnEquipment(animUseSlot);
                    if (!(item != null && item.Blocked))
                        animator.SetBool("KnockBack", false);
                }

                if (distance < currentAI.GetLongestRange() && Mathf.Abs(angle) < 30)//HardCoded
                    animator.SetBool("Attack", true);
                else
                {
                    animator.SetBool("Attack", false);
                    animator.SetFloat("Speed", 2);//HardCoded
                }
                timer = 5f;//HardCoded
            }
            else
            {
                timer -= Time.deltaTime;
                animator.SetFloat("Speed", 0);
            }
        }
        else
            ChangeState(FSMState.IDLE);
    }


    protected override void UpdateInteractionState()
    {
        if (target != null)
        {
            Vector3 _direction = (target.transform.position - transform.position).normalized;
            Quaternion _lookRotation = Quaternion.LookRotation(_direction);
            _lookRotation.z = _lookRotation.x = 0;
            transform.rotation = Quaternion.Slerp(transform.rotation, _lookRotation, Time.deltaTime * 10);

            if (Vector3.Distance(transform.position, target.transform.position) < 3 && !currentAI.IsConversing)
            {
                currentAI.IsConversing = true;
                target.Interact(currentAI);
            }
        }
    }

    protected override void UpdatePetrolState()
    {
        if (path.Count == 0)
        {
            ChangeState(FSMState.INTERACTION);

        }
        else
        {
            Vector3 targetPoint = path[0];
            Vector3 _direction = (targetPoint - transform.position).normalized;
            _direction.y = 0;
            //create the rotation we need to be in to look at the target
            Quaternion _lookRotation = Quaternion.LookRotation(_direction);

            //rotate us over time according to speed until we are in the required rotation
            transform.rotation = Quaternion.Slerp(transform.rotation, _lookRotation, Time.deltaTime * 10);

            rigidBody.velocity = _direction * currentAI.MovementSpeed;
            if (Vector3.Distance(transform.position, targetPoint) < .25f)
                path.RemoveAt(0);
        }
    }
}