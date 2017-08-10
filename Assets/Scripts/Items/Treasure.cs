using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(SphereCollider))]
public class Treasure : MonoBehaviour, IDamagable {


    protected Collider col;

    protected void Awake()
    {
        col = GetComponent<Collider>();
        Debug.Log(col);
        col.isTrigger = true;
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


    public void TakeDamage(float Damage,Actor actor, JobType damageType)
    {
       
        GameManager.Instance.AddTax((int)Damage);
        if(actor is Monster)
        {
            (actor as Monster).Despawn();
        }
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
