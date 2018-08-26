using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterFSM : ActorFSM
{

    protected Monster currentMonster;


    public override AI CurrentAI
    {
        get
        {
            return currentMonster;
        }
    }

    protected override void HandleCombatAction()
    {
        animator.SetBool("Attacking", true);
    }


    protected override void Awake()
    {
        base.Awake();
        currentMonster = GetComponent<Monster>();
    }

    protected override bool CheckForTargets()
    {
        bool hasTarget = base.CheckForTargets();
        if (!hasTarget)
        {
            Vector3 playerPos = Player.Instance.transform.position;

            if (Vector3.Distance(transform.position, playerPos) <= detectionDistance && Player.Instance.CanBeAttacked)
            {
                hasTarget = true;
                target = Player.Instance;
            }

        }
        return hasTarget;
    }
    protected override void UpdateIdleState()
    {
        if (pathFound)
        {
            ChangeState(FSMState.PETROL);
        }
        else if (!requestedPath)
        {
            Transform targetTrans = TownManager.Instance.CurrentTown.Treasure;
            if (targetTrans != null)
            {
                target = targetTrans.GetComponent<IDamagable>();
                AStarManager.Instance.RequestPath(transform.position, target.transform.position, ChangePath);
                requestedPath = true;
                nextState = FSMState.COMBAT;
            }

        }
    }


}
