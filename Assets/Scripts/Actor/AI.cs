using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.Events;

public abstract class AI : Actor
{

    protected ActorFSM currentFSM;

    [SerializeField]
    protected float movementSpeed = 3;//replacable by generic script

    protected bool isInteracting = false;

    public float MovementSpeed
    {
        get
        {
            return movementSpeed;
        }
        set
        {
            movementSpeed = value;
        }
    }

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
        if (health <= 0)
        {
            currentFSM.ChangeState(ActorFSM.FSMState.DEATH);
            UnEquipWeapons();
            

        }
        else if (Mathf.Sign(damage) == 1)
        {
            bool knockedBack = false;
            if (damage / (float)actorData.Health > .2)
                knockedBack = true;
            currentFSM.DamageTaken(knockedBack,attacker);
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

    public void Warp(Vector3 position)
    {
        transform.position = position;
    }

}
