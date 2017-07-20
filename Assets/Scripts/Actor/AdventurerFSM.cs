using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class AdventurerFSM : ActorFSM
{
    private List<Shop> visitedShop = new List<Shop>();

    private Shop targetShop;

    private bool inTown = true;

    public void NewVisitToTown()
    {
        visitedShop.Clear();
    }


    protected override void UpdateFSMState()
    {
        base.UpdateFSMState();
        if (GameManager.Instance.State == GameManager.GameState.NIGHTMODE)
        {
            switch (currentState)
            {
                case FSMState.COMBAT:
                    UpdateCombatState();
                    break;
                case FSMState.IDLE:
                    UpdateIdleStateNight();
                    break;
            }
        }
        else if (GameManager.Instance.State == GameManager.GameState.DAYMODE)
        {
            switch (currentState)
            {
                case FSMState.IDLE:
                    UpdateIdleStateDay();
                    break;

                case FSMState.SHOPPING:
                    UpdateShoppingState();
                    break;
            }
        }
    }

    protected virtual void UpdateShoppingState()
    {
        float val = Random.value;
        Node shopPoint = targetShop.GetRandomPoint(transform.position);
        if (val > .5 && shopPoint != null)
        {

            ChangePath(shopPoint);
            nextState = FSMState.SHOPPING;
            ChangeState(FSMState.PETROL);

        }
        else
        {
            visitedShop.Add(targetShop);
            if (!(targetShop.Owner is Player) || ((targetShop.Owner is Player) && (currentAI as AdventurerAI).IsInteractionAvailable() && !targetShop.InteractionNode.Occupied))
            {
                ChangePath(targetShop.InteractionNode);
                ChangeState(FSMState.PETROL);
                nextState = FSMState.IDLE;
            }
            else
            {
                ChangeState(FSMState.IDLE);
            }
        }
    }

    protected virtual void UpdateIdleStateNight()
    {
        if (pathFound)
        {
            ChangeState(FSMState.PETROL);
        }
        else if (!requestedPath)
        {
            Node endPoint = TownManager.Instance.CurrentTown.WavePoint;
            AStarManager.Instance.RequestPath(transform.position, endPoint.Position, ChangePath);
            requestedPath = true;
            nextState = FSMState.IDLE;
        }
    }

    protected virtual void UpdateIdleStateDay()
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
                //if (_targetShop.Owner == Player.Instance && !(currentAI as AdventurerAI).GotLobang())
                //{
                //    if (TownManager.Instance.NumberOfShopsUnvisited(visitedShop) == 1)
                //        visitedShop.Add(_targetShop);
                //    return;
                //}
                targetShop = _targetShop;
                AStarManager.Instance.RequestPath(transform.position, _targetShop.Location.Position, ChangePath);
                requestedPath = true;
                nextState = FSMState.SHOPPING;

            }
            else
            {
                Node endPoint = TownManager.Instance.GetRandomSpawnPoint();
                AStarManager.Instance.RequestPath(transform.position, endPoint.Position, ChangePath);
                requestedPath = true;
            }
        }
    }


    public override void ChangeState(FSMState state)
    {

        FSMState previousState = currentState;
        (currentAI as AdventurerAI).Interacting = false;
        base.ChangeState(state);
        if (previousState != currentState)
        {
            switch (state)
            {
                case FSMState.PETROL:
                    animator.SetFloat("Speed", 2);
                    break;

                case FSMState.COMBAT:
                    (currentAI as AdventurerAI).EquipRandomWeapons();
                    //animator.SetBool("KnockBack", true);//Wrong place
                    //timer = 5;
                    break;

            }
        }
    }



    protected override void UpdateCombatState()
    {
        if (/*timer > 0 &&*/ target != null)
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
                    animator.SetBool("Attacking", true);

                Weapon longestWeapon = currentAI.GetLongestWeapon();
                if (longestWeapon != null && distance <= longestWeapon.Range && Mathf.Abs(angle) < 45)
                {
                    if (!isAttacking)
                        animator.SetTrigger("Cast");
                }
                else
                {
                    animator.SetBool("Attacking", false);
                    animator.SetFloat("Speed", 2);//HardCoded
                }
                //timer = 5f;//HardCoded
            }
            else
            {
                //timer -= Time.deltaTime;
                animator.SetFloat("Speed", 0);
            }
        }
        else
            ChangeState(FSMState.IDLE);
    }

    public System.Collections.IEnumerator Interact(Actor actor)
    {
        target = actor;
        isHandlingAction = true;
        float waitTimer = 5;
        
        while (isHandlingAction) { 

            LookAtPlayer(actor.transform.position);

            if (target is Player)
            {
                Player player = target as Player;
                AdventurerAI currentAdventurer = currentAI as AdventurerAI;
                if (currentAdventurer.IsInteractionAvailable() || currentAdventurer.Interacting)
                {
                    if (player.CheckConversingWith(currentAdventurer))
                    {
                        waitTimer = 5;
                        if (!currentAdventurer.Interacting)
                        {
                            Debug.Log("hut");
                            currentAdventurer.StartInteraction();
                        }
                    }
                    else
                    {
                        waitTimer -= Time.deltaTime;
                        if (waitTimer <= 0)
                        {
                            currentAI.Interacting = false;
                            break;
                        }
                    }
                }
                else
                {
                    currentAI.Interacting = false;
                    break;
                }
            }
            else
            {
                currentAI.Interacting = false;
                break;
            }
            yield return new WaitForEndOfFrame();

        } 

        isHandlingAction = false;
    }
}