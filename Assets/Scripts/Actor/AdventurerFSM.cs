
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(AdventurerAI))]

public class AdventurerFSM : ActorFSM
{
    private AdventurerAI currentAI;

    [SerializeField]
    private float detectionDistance;




    protected override void Awake()
    {
        base.Awake();
        currentAI = GetComponent<AdventurerAI>();
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
        if (Vector3.Distance(target.transform.position, transform.position) > detectionDistance)
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
