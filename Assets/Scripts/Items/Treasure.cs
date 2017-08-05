﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Treasure : MonoBehaviour, IDamagable {


    protected Collider col;

    protected void Awake()
    {
        col = GetComponent<Collider>();
    }

    public Collider Collider
    {
        get
        {
            return col;
        }
    }
    public bool CanBeAttacked
    {
        get
        {
            return true;
        }
    }

    public int Health
    {
        get
        {
            return Player.Instance.Gold;
        }
    }


    public void TakeDamage(float Damage,Actor actor)
    {
       
        GameManager.Instance.AddTax((int)Damage);
        actor.Die();
    }
	public new Transform transform
    {
        get
        {
            if (gameObject)
                return base.transform;
            else
                return null;
        }
    }
}
