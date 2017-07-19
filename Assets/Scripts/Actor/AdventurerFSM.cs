
using System.Collections.Generic;
using UnityEngine;

public class AdventurerFSM : ActorFSM
{
    private List<Shop> visitedShop = new List<Shop>();

    private AdventurerAI currentAdventurerAI;

    private Shop targetShop;

    private bool inTown = true;

    public void NewVisitToTown()
    {
        visitedShop.Clear();
    }

    protected override void Awake()
    {
        base.Awake();
        currentAdventurerAI = GetComponent<AdventurerAI>();
    }
    public override AI CurrentAI
    {
        get
        {
            return currentAdventurerAI;
        }
    }

    protected override void UpdateFSMState()
    {
        base.UpdateFSMState();
        if (GameManager.Instance.State == GameManager.GameState.NIGHTMODE)
        {
            switch (currentState)
            {
                case FSMState.IDLE:
                    UpdateIdleStateNight();
                    return;
            }
        }
        else if (GameManager.Instance.State == GameManager.GameState.DAYMODE)
        {
            switch (currentState)
            {
                case FSMState.IDLE:
                    UpdateIdleStateDay();
                    return;

                case FSMState.SHOPPING:
                    UpdateShoppingState();
                    return;
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
            if (!(targetShop.Owner is Player) || ((targetShop.Owner is Player) && currentAdventurerAI.GotLobang() && !targetShop.InteractionNode.Occupied))
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
        currentAdventurerAI.Interacting = false;
        base.ChangeState(state);
        if (previousState != currentState)
        {
            switch (state)
            {
                case FSMState.PETROL:
                    animator.SetFloat("Speed", 2);
                    return;

                case FSMState.COMBAT:
                    currentAdventurerAI.EquipRandomWeapons();
                    //animator.SetBool("KnockBack", true);//Wrong place
                    //timer = 5;
                    return;

            }
        }
    }

    protected override void UpdateCombatState()
    {
        animator.SetBool("Cast", false);
        base.UpdateCombatState();
    }

    protected override void HandleCombatAction()
    {
        currentUseWeapon = (Weapon)currentAdventurerAI.returnEquipment(animUseSlot);
       
        animator.SetBool("Attacking", true);

        if (currentUseWeapon == null || !currentUseWeapon.Powered)
            animator.SetBool("Cast", true);
        else
        {
            animator.SetBool("Cast", false);
        }

    }

    public void StartCharge()
    {

        currentUseWeapon = (Weapon)currentAdventurerAI.returnEquipment(animUseSlot);

        if (currentUseWeapon != null)
        {
            currentUseWeapon.StartCharge();
        }
    }

    public System.Collections.IEnumerator Interact(Actor actor)
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
                if (currentAdventurerAI.GotLobang() || currentAdventurerAI.Interacting)
                {
                    if (player.CheckConversingWith(currentAdventurerAI))
                    {
                        waitTimer = 5;
                        if (!currentAdventurerAI.Interacting)
                        {
                            currentAdventurerAI.GetLobang();
                        }
                    }
                    else
                    {
                        waitTimer -= Time.deltaTime;
                        if (waitTimer <= 0)
                        {
                            currentAdventurerAI.Interacting = false;
                            break;
                        }
                    }
                }
                else
                {
                    currentAdventurerAI.Interacting = false;
                    break;
                }
            }
            else
            {
                currentAdventurerAI.Interacting = false;
                break;
            }
            yield return new WaitForEndOfFrame();

        }

        isHandlingAction = false;
    }
}