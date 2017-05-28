using System.Collections.Generic;
using UnityEngine;

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
        base.ChangeState(state);
        

        switch (state)
        {
            case FSMState.IDLE:
                timer = UnityEngine.Random.Range(1.0f, 5.0f);

                break;

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
                target = targetShop.Owner;
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
            Vector3 dir = target.transform.position - transform.position;
            float distance = Vector3.Distance(target.transform.position, transform.position);
            //dir.y = 0;
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
                targetShop.Owner.Interact(currentAI);
            }
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