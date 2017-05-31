﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.Events;

public class AI : Actor
{

    protected ActorFSM currentFSM;
    protected bool isConversing;

    [SerializeField]
    protected float movementSpeed = 3;


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



    protected override void Awake()
    {
        base.Awake();
        currentFSM = GetComponent<ActorFSM>();
        if (jobList.Count != 0)
            jobList.Clear();
    }

    public bool IsConversing
    {
        get { return this.isConversing; }
        set { isConversing = value; }
    }


    //public override void Interact(Actor actor)
    //{
    //    currentFSM.ChangeState(ActorFSM.FSMState.INTERACTION);
    //}

    public override void TakeDamage(int damage, Actor attacker)
    {
        base.TakeDamage(damage, attacker);
        if (health <= 0)
        {
            currentFSM.ChangeState(ActorFSM.FSMState.DEATH);
            UnEquipWeapons();
            Destroy(gameObject, 3f);

        }
        else if (Mathf.Sign(damage) == 1)
        {
            bool knockedBack = false;
            if (damage / (float)maxHealth > .2)
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

    public virtual void StopInteraction()
    {
        isConversing = false;
    }

}
