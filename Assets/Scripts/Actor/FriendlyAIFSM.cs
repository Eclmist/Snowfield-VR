using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class FriendlyAIFSM : ActorFSM {

    protected List<Shop> visitedShop = new List<Shop>();

    protected Shop targetShop;


    protected AdventurerAI currentAdventurerAI;

    public override AI CurrentAI
    {
        get
        {
            return currentAdventurerAI;
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
            if ((!(targetShop.Owner is Player) || ((targetShop.Owner is Player) && currentAdventurerAI.IsInteractionAvailable())) && !targetShop.InteractionNode.Occupied)
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

    public void StartInteractRoutine(Actor actor)
    {
        StartCoroutine(Interact(actor));
    }


    protected override void UpdateFSMState()
    {
        base.UpdateFSMState();

        switch (currentState)
        {
            case FSMState.SHOPPING:
                UpdateShoppingState();
                return;
        }

    }

    public override void DamageTaken(Actor attacker)
    {
        if (attacker is Monster)
        {
            base.DamageTaken(attacker);
        }
    }
    public IEnumerator Interact(Actor actor)
    {
        target = actor;
        isHandlingAction = true;
        float waitTimer = 5;

        while (isHandlingAction)
        {
            LookAtPlayer(actor.transform.position);

            if (target is Player)
            {
                Player player = target as Player;
                if (currentAdventurerAI.IsInteractionAvailable() || currentAdventurerAI.Interacting)
                {
                    if (player.CheckConversingWith(currentAdventurerAI))
                    {
                        waitTimer = 5;

                        if (!currentAdventurerAI.Interacting)
                        {
                            currentAdventurerAI.StartInteraction();
                        }
                    }
                    else
                    {
                        waitTimer -= Time.deltaTime;
                        if (waitTimer <= 0)
                        {
                            currentAdventurerAI.StopAllInteractions();
                            break;
                        }

                    }
                }
                else
                {
                    currentAdventurerAI.StopAllInteractions();
                    break;
                }
            }
            else
            {
                currentAdventurerAI.StopAllInteractions();
                break;
            }
            yield return new WaitForEndOfFrame();

        }

        isHandlingAction = false;
    }
}
