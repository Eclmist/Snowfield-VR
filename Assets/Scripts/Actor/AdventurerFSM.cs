using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class AdventurerFSM : ActorFSM
{
    private List<Shop> visitedShop = new List<Shop>();

    private Shop targetShop;

    private void NewVisitToTown()
    {
        visitedShop.Clear();
    }

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

            case FSMState.QUESTING:
                UpdateQuestingState();
                break;
        }
    }

    protected virtual void UpdateQuestingState()
    {
        bool progress = (currentAI as AdventurerAI).QuestProgress();
        if (!progress)
        {
            ChangeState(FSMState.IDLE);
            NewVisitToTown();
        }
    }

    public override void ChangeState(FSMState state)
    {
        FSMState previousState = currentState;
        base.ChangeState(state);
        if (previousState != currentState)
        {
            switch (state)
            {
                case FSMState.PETROL:
                    animator.SetFloat("Speed", 2);
                    break;

                case FSMState.INTERACTION:
                    rigidBody.isKinematic = true;
                    visitedShop.Add(targetShop);
                    break;

                case FSMState.COMBAT:
                    (currentAI as AdventurerAI).EquipRandomWeapons();
                    //animator.SetBool("KnockBack", true);//Wrong place
                    animator.speed = 1 + (currentAI.GetJob(JobType.ADVENTURER).Level * .1f);
                    timer = 5;
                    break;

            }
        }
    }

    protected override void UpdateIdleState()
    {
        if (pathFound)
        {
            ChangeState(FSMState.PETROL);
        }
        else if (!requestedPath)
        {
            Shop _targetShop = TownManager.Instance.GetRandomShop(visitedShop);
            if (_targetShop != null)//And check for anymore shop
            {
                if (_targetShop.Owner == Player.Instance && !(currentAI as AdventurerAI).GotLobang())
                {
                    if (TownManager.Instance.NumberOfShopsUnvisited(visitedShop) == 1)
                        visitedShop.Add(_targetShop);
                    return;
                }
                targetShop = _targetShop;
                AStarManager.Instance.RequestPath(transform.position, _targetShop.GetNextLocation(), ChangePath);
                requestedPath = true;
                nextState = FSMState.INTERACTION;
            }
            else
            {
                Transform endPoint = TownManager.Instance.GetRandomSpawnPoint();
                AStarManager.Instance.RequestPath(transform.position, endPoint.position, ChangePath);
                requestedPath = true;
                nextState = FSMState.QUESTING;
            }
        }
    }

    protected override void UpdateCombatState()
    {
        if (timer > 0 && target != null)
        {
            
            Vector3 temptarget = target.transform.position;
            temptarget.y = transform.position.y;
            Vector3 dir = temptarget - transform.position;
            float distance = Vector3.Distance(temptarget, transform.position);
            //dir.Normalize();

            float angle = Vector3.Angle(transform.forward, dir);

            //Debug.DrawRay(head.transform.position - head.right, _direction);
            if (distance < detectionDistance)
            {
                if (animator.GetCurrentAnimatorStateInfo(0).IsName("Petrol"))
                {
                    rigidBody.velocity = AvoidObstacles(target.transform.position);

                    LookAtPlayer(target.transform.position);
                }

                bool isAttacking = animator.GetCurrentAnimatorStateInfo(0).IsName("Attack") || animator.GetCurrentAnimatorStateInfo(0).IsName("Cast");

                if (isAttacking)
                    animator.SetBool("Attacking",true);

                Weapon longestWeapon = currentAI.GetLongestWeapon();
                if (longestWeapon != null && distance <= longestWeapon.Range && Mathf.Abs(angle) < 45)
                {
                    if(!isAttacking)
                    animator.SetTrigger("Cast");
                }
                else
                {
                    animator.SetBool("Attacking", false);
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

        //if (CheckObstacles())
        //{
        //    ChangePath(GetReversePoint());
        //    nextState = FSMState.INTERACTION;
        //    ChangeState(FSMState.PETROL);
        //    return;
        //}

        LookAtPlayer(targetShop.Location);
        if (!currentAI.IsConversing)
        {
            if (targetShop.Owner is Player)
            {
                if ((currentAI as AdventurerAI).GotLobang())
                    (currentAI as AdventurerAI).GetLobang();
                else
                    ChangeState(FSMState.IDLE);
            }

        }
        else
        {
            LookAtPlayer(targetShop.Owner.transform.position);
        }

    }



    protected override void UpdatePetrolState()
    {
        if (path.Count == 0)
        {
            ChangeState(nextState);
        }
        else
        {
            Vector3 targetPoint = path[0];
            rigidBody.velocity = AvoidObstacles(targetPoint);
            LookAtPlayer(targetPoint);
            Collider[] col = Physics.OverlapSphere(path[0], 1f, ~avoidanceIgnoreMask);


            bool hasThings = Vector3.Distance(transform.position, targetPoint) < .5f && col.Length != 0;
            if (Vector3.Distance(transform.position, targetPoint) < .1f || hasThings)
            {
                path.RemoveAt(0);
            }

        }
    }
}