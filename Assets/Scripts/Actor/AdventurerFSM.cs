using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AdventurerFSM : ActorFSM {

	protected virtual void UpdateIntrusionState()
    {

    }

    protected override void UpdateFSMState()
    {
        base.Update();
        switch (currentState)
        {
            case FSMState.INTRUSION:
                UpdateInteractionState();
                break;
        }
    }

    protected override void UpdateIdleState()
    {
        if (path != null && path.Length > 0)
            currentState = FSMState.PETROL;
        else if (requestedPath)
        {
            AStarManager.Instance.RequestPath(transform.position, TownManager.Instance.GetRandomShop().Location,ChangePath);
        }
    }

    protected override void UpdateCombatState()
    {
        // Fight Chen Xiang to a battle of death, BOOM, WHACK, POW,
        // You won against the battle with Chen Xiang.
        throw new NotImplementedException();
    }

    protected override void UpdateInteractionState()
    {
        throw new NotImplementedException();
    }

    protected override void UpdatePetrolState()
    {
        throw new NotImplementedException();
    }
}
