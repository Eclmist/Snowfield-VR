using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MerchantAIFSM : FriendlyAIFSM
{

    protected bool goingHome = false;

    protected override void UpdateAnyState()
    {

        if (pathFound)
            ChangeState(FSMState.PETROL);
        else if (WaveManager.Instance.HasMonster && !requestedPath && !goingHome)
        {
            goingHome = true;
            requestedPath = true;
            AStarManager.Instance.RequestPath(transform.position, TownManager.Instance.GetRandomSpawnPoint().Position, ChangePath);
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
            Shop _targetShop = TownManager.Instance.GetSpecificShop(Player.Instance);

            if (_targetShop != null && !visitedShop.Contains(_targetShop))//And check for anymore shop
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
}
