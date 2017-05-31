using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class Weapon : Equipment
{

    [SerializeField]
    protected float range;
    private Animator animator;

    protected override void Awake()
    {
        base.Awake();
        animator = GetComponent<Animator>();
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

    protected override void OnTriggerEnter(Collider collision)
    {
        base.OnTriggerEnter(collision);
        if (IfBlocked != null)
        {
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

    public void StartCharge()
    {
        if(animator != null)
        {
            animator.SetTrigger("Charge");
        }
    }

}