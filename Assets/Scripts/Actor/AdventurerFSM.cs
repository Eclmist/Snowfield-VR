
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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
                AStarManager.Instance.RequestPath(transform.position, TownManager.Instance.GetRandomShop().Location, ChangePath);
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
            Vector3 _direction = (target.transform.position - head.position).normalized;
            float distance = Vector3.Distance(target.transform.position, transform.position);
            Vector3 dir = target.transform.position - transform.position;
            dir.y = 0;
            dir.Normalize();
            float angle = Vector3.Angle(transform.forward, dir);
            //RaycastHit hit1;
            //Physics.Raycast(head.transform.position, _direction, out hit1, detectionDistance);

            //Physics.Raycast(head.transform.position - head.right, _direction, out hit2);
            //Debug.DrawRay(head.transform.position - head.right, _direction);
            if (distance < detectionDistance) /*&& hit1.transform == target.transform*/
            {

                if (animator.GetCurrentAnimatorStateInfo(0).IsName("Petrol"))
                {
                    _direction = (target.transform.position - transform.position).normalized;
                    _direction.y = 0;
                    Quaternion _lookRotation = Quaternion.LookRotation(_direction);
                    transform.rotation = Quaternion.Slerp(transform.rotation, _lookRotation, Time.deltaTime * 10);
                }


                if (distance < detectionDistance/4 && Mathf.Abs(angle) < 30)//HardCoded
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
        }
        else
        {
            Vector3 targetPoint = new Vector3(path[0].x, transform.position.y, path[0].z);
            Vector3 _direction = (targetPoint - transform.position).normalized;

            //create the rotation we need to be in to look at the target
            Quaternion _lookRotation = Quaternion.LookRotation(_direction);

            //rotate us over time according to speed until we are in the required rotation
            transform.rotation = Quaternion.Slerp(transform.rotation, _lookRotation, Time.deltaTime * 10);

            if (Vector3.Distance(transform.position, targetPoint) < 1f)
                path.RemoveAt(0);

        }
    }
}
