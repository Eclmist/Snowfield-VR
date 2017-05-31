using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

[RequireComponent(typeof(Animator))]
public class Weapon : Equipment
{

    [SerializeField]
    protected float range;
    protected bool charging;
    [SerializeField]
    private float maxEmissiveValue;
    private float currentVal;
 
    protected override void Awake()
    {
        base.Awake();
        //trail = GetComponentInChildren<XftWeapon.XWeaponTrail>();
       
    }


    public float Range
    {
        get
        {
            return range;
        }
        set
        {
            range = value;
        }
    }

    private Action<IBlock> IfBlocked;

    public void SetBlockable(Action<IBlock> ifblocked = null)
    {
        IfBlocked = ifblocked;
    }

    protected override void UseItem()
    {
        base.UseItem();
    }

    protected virtual void Update()
    {
        HandleMaterial();
        //HandleTrail();
    }

    protected virtual void HandleMaterial()
    {
       

    }
    protected override void OnTriggerEnter(Collider collision)
    {
        base.OnTriggerEnter(collision);
        Debug.Log(IfBlocked);
        if (IfBlocked != null)
        {
            if(linkedController != null)
            {
                IDamagable target = collision.GetComponent<IDamagable>();
                if (target != null)
                    Player.Instance.Attack(this, target);
            }
            IBlock item = collision.GetComponentInParent<IBlock>();
            
            if (item != null && item.IsBlocking)
            {
                IfBlocked(item);
            }
        }
    }

  


    void OnDrawGizmos()
    {
        Gizmos.DrawSphere(transform.position, range);
    }

    public void SetCharge(bool val)
    {
        charging = val;
    }

}