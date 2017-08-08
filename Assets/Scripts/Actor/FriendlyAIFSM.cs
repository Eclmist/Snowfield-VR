using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FriendlyAiFSM : ActorFSM
{

    protected List<Shop> visitedShop = new List<Shop>();

    protected Shop targetShop;

    protected FriendlyAI currentFriendlyAI;

    public override AI CurrentAI
    {
        get
        {
            return currentFriendlyAI;
        }
    }

    protected override void Awake()
    {
        currentFriendlyAI = GetComponent<FriendlyAI>();
        base.Awake();
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
            if ((!(targetShop.Owner is Player) || ((targetShop.Owner is Player) && currentFriendlyAI.IsInteractionAvailable())) && !targetShop.InteractionNode.Occupied)
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

    public override void NewSpawn()
    {
        base.NewSpawn();
        visitedShop.Clear();
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
            Vector3 endPos = target.transform.position;
            endPos.y = transform.position.y;
            Vector3 dir = (target.transform.position - transform.position).normalized;
            dir.y = 0;

            float angle = Mathf.Abs(Vector3.Angle(transform.forward, dir));

            if (angle < 30)
            {
                if (target is Player)
                {
                    Player player = target as Player;
                    if (player.CheckConversingWith(currentFriendlyAI))
                    
                    if (currentFriendlyAI.IsInteractionAvailable() || currentFriendlyAI.Interacting)
                    {
                        if (player.CheckConversingWith(currentFriendlyAI))
                        {
                            waitTimer = 5;

                            if (!currentFriendlyAI.Interacting)
                            {
                                currentFriendlyAI.StartInteraction();
                            }
                        }
                        else
                        {
                            waitTimer -= Time.deltaTime;
                            if (waitTimer <= 0)
                            {
                                currentFriendlyAI.StopAllInteractions();
                                break;
                            }

                        }
                    }
                    else
                    {
                        currentFriendlyAI.StopAllInteractions();
                        break;
                    }
                }
                else
                {
                    currentFriendlyAI.StopAllInteractions();
                    break;
                }
            }
            yield return new WaitForEndOfFrame();

        }

        isHandlingAction = false;
    }
}
