﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.Events;

public abstract class AI : CombatActor
{

    protected ActorFSM currentFSM;

    protected bool isInteracting = false;

    [SerializeField]
    protected ParticleSystem spawnPS, disablePS;


    public bool Interacting
    {
        get
        {
            return isInteracting;
        }
        set
        {
            isInteracting = value;
        }
    }

    public override void Notify(AI ai)
    {
        Interact(ai);
    }

    public override bool CheckConversingWith(Actor target)
    {
        return currentFSM.Target == target;
    }

    protected override void Awake()
    {
        base.Awake();
        currentFSM = GetComponent<ActorFSM>();
    }

    public void ChangeState(ActorFSM.FSMState state)
    {
        currentFSM.ChangeState(state);
    }

    public override void TakeDamage(int damage, Actor attacker)
    {
        base.TakeDamage(damage, attacker);
        if (variable.GetCurrentHealth() <= 0)
        {
            currentFSM.ChangeState(ActorFSM.FSMState.DEATH);
            UnEquipWeapons();
        }
        else
        {
            //currentFSM.DamageTaken(attacker);
        }
    }

    protected void UnEquipWeapons()
    {
        if (leftHand.Item != null)
            leftHand.Item.Unequip();
        if (rightHand.Item != null)
            rightHand.Item.Unequip();
    }

    public virtual void LookAtObject(Transform target,float time,float angle)
    {
        StartCoroutine(currentFSM.LookAtTransform(target, time, angle));
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
        if (disablePS)
        {
            Destroy(Instantiate(disablePS, transform.position, transform.rotation),3);
            gameObject.SetActive(true);
        }
    }

    public virtual void Despawn()
    {
        if (spawnPS)
        {
            Destroy(Instantiate(spawnPS, transform.position, transform.rotation),3);
            gameObject.SetActive(false);
        }
    }
}
