using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.Events;

public abstract class AI : Actor
{

    protected ActorFSM currentFSM;


    [SerializeField]
    protected ParticleSystem spawnPS, disablePS;


    public override void Notify(AI ai)
    {
        Interact(ai);
    }

    public override bool CheckConversingWith(Actor target)//switch to iinteractable
    {
        if (currentFSM.Target is Actor)
            return (currentFSM.Target as Actor) == target;
        else
            return false;
    }

    protected override void Awake()
    {
        base.Awake();
        currentFSM = GetComponent<ActorFSM>();
    }

    public void ChangeState(ActorFSM.FSMState state)
    {
        if (currentFSM)
            currentFSM.ChangeState(state);
    }

    public override void TakeDamage(float damage, Actor attacker)
    {
        base.TakeDamage(damage, attacker);
        if (variable.GetStat(Stats.StatsType.HEALTH).Current <= 0)
        {
            currentFSM.ChangeState(ActorFSM.FSMState.DEATH);
            UnEquipWeapons();
        }
        else
        {
            currentFSM.DamageTaken(attacker);
        }
    }

    public void UnEquipWeapons()
    {
        if (leftHand != null && leftHand.Item != null)
        {
            if (variable.GetStat(Stats.StatsType.HEALTH).Current > 0)
                Destroy(leftHand.Item.gameObject);
            else
                leftHand.Item.Unequip();
        }
        if (rightHand != null && rightHand.Item != null)
        {
            if (variable.GetStat(Stats.StatsType.HEALTH).Current > 0)
                Destroy(rightHand.Item.gameObject);
            else
                rightHand.Item.Unequip();
        }
    }

    public virtual void LookAtObject(Transform target, float time, float angle)
    {
		currentFSM.StartLookAtRoutine (target, time, angle);
    }

    public void SetNode(Node n)
    {
        currentFSM.SetNode(n);
    }
    public abstract void Interact(Actor actor);

    public virtual float GetOutOfTimeDuration()
    {
        return 0;
    }

    public virtual void OutOfTownProgress()
    {

    }

    public virtual void Spawn()
    {

        gameObject.SetActive(true);

    }

    public virtual void Despawn()
    {


        if (disablePS)
        {
            Destroy(Instantiate(disablePS, transform.position, transform.rotation), 3);
            gameObject.SetActive(false);
        }
    }
}
