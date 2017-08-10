﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.Events;

public abstract class AI : Actor
{

    protected ActorFSM currentFSM;

    protected GameObject spawnPS, disablePS;


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
        spawnPS = transform.Find("SpawnParticle").gameObject;
        disablePS = transform.Find("DeathParticle").gameObject;
    }

    public void ChangeState(ActorFSM.FSMState state)
    {
        if (currentFSM)
            currentFSM.ChangeState(state);
    }

    public override void TakeDamage(float damage, Actor attacker)
    {


        damage = CombatManager.Instance.GetDamageDealt(damage,transform);

        base.TakeDamage(damage, attacker);

        currentFSM.DamageTaken(attacker);

    }

    public override void Die()
    {
        currentFSM.ChangeState(ActorFSM.FSMState.DEATH);
        float rng = UnityEngine.Random.Range(0, 1f);
        if (rng > 1 - GameConstants.Instance.ItemDropRate)
            DropItem();
    }

    public virtual void LookAtObject(Transform target, float time, float angle)
    {
        currentFSM.StartLookAtRoutine(target, time, angle);
    }

    public void SetNode(Node n)
    {
        currentFSM.SetNode(n);
    }
    public abstract void Interact(Actor actor);

    public void Stun()
    {
        currentFSM.SetStun(1);
    }

    public void SetVelocity(Vector3 vel)
    {
        currentFSM.SetVelocity(vel);
    }

   
    public virtual void Spawn()
    {
        gameObject.SetActive(true);
        if (spawnPS)
        {
            GameObject ps = Instantiate(spawnPS, spawnPS.transform.position, spawnPS.transform.rotation);
            ps.SetActive(true);
            Destroy(ps, 3);
        }
    }

    public virtual void Despawn()
    {

        if (disablePS)
        {
            GameObject ps = Instantiate(disablePS, disablePS.transform.position, disablePS.transform.rotation);
            ps.SetActive(true);
            Destroy(ps, 3);

        }
        gameObject.SetActive(false);
    }

    protected virtual void DropItem()
    {
        ItemData data = ItemManager.Instance.GetRandomItemByLevel(Data.GetJob(JobType.COMBAT).Level);
        if(data != null)
        {
            GameObject obj = Instantiate(data.ObjectReference, transform.position, transform.rotation);
            obj.AddComponent<DroppedItem>();
        }
    }
}
