
using System;
using System.Collections;
using System.Collections.Generic;
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
        switch (state)
        {
            case FSMState.IDLE:
                timer = UnityEngine.Random.Range(1.0f, 5.0f);
                animator.SetFloat("Speed", 0);
                break;
            case FSMState.PETROL:
                animator.SetFloat("Speed", 2);
                break;
            case FSMState.INTERACTION:
                break;
            case FSMState.COMBAT:
                animator.SetBool("KnockBack", true);
                animator.speed = 1 + (currentAI.GetJob(JobType.ADVENTURER).Level * .1f);
                timer = 5;
                break;

        }
    }

    protected override void UpdateIdleState()
    {
        if (timer <= 0)
        {
            if (path != null && path.Count > 0)
            {
                ChangeState(FSMState.PETROL);
            }
            else if (!requestedPath)
            {
                AStarManager.Instance.RequestPath(transform.position, TownManager.Instance.GetRandomShop().Location.position, ChangePath);

                requestedPath = true;
            }
        }
        else
        {
            timer -= Time.deltaTime;
        }
    }



    protected override void UpdateCombatState()
    {
        if (timer > 0)
        {
            Vector3 _direction = (target.transform.position - transform.position).normalized;
            float distance = Vector3.Distance(target.transform.position, transform.position);
            Vector3 dir = target.transform.position - transform.position;
            dir.y = 0;
            dir.Normalize();
            float angle = Vector3.Angle(transform.forward, dir);


            //Debug.DrawRay(head.transform.position - head.right, _direction);
            if (distance < detectionDistance)
            {

                if (animator.GetCurrentAnimatorStateInfo(0).IsName("Petrol"))
                {
                    _direction = (target.transform.position - transform.position).normalized;
                    _direction.y = 0;
                    Quaternion _lookRotation = Quaternion.LookRotation(_direction);
                    transform.rotation = Quaternion.Slerp(transform.rotation, _lookRotation, Time.deltaTime * 10);
                    rigidBody.velocity = _direction * currentAI.MovementSpeed;
                } else if (animator.GetCurrentAnimatorStateInfo(0).IsName("Attack"))
                {
                    IBlockable item = (Weapon)currentAI.returnEquipment(animUseSlot);
                    Debug.Log(animUseSlot);
                    if (item != null && item.Blocked)
                        animator.SetBool("KnockBack", true);
                }else if (animator.GetCurrentAnimatorStateInfo(0).IsName("KnockBack"))
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

    }

    protected override void UpdatePetrolState()
    {
        if (path.Count == 0)
        {
            ChangeState(FSMState.IDLE);
            rigidBody.angularVelocity = Vector3.zero;
            rigidBody.velocity = Vector3.zero;
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
            if (Vector3.Distance(transform.position, targetPoint) < 1f)
                path.RemoveAt(0);
            

        }
    }
}
