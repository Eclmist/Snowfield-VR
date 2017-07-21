using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
[RequireComponent(typeof(CombatVariable))]
public abstract class Actor : MonoBehaviour, IDamagable, IHasVariable
{

    [SerializeField]
    protected EquipSlot leftHand, rightHand;

    public abstract bool CheckConversingWith(Actor target);

    protected CombatVariable variable;

    protected virtual void Awake()
    {
        variable = GetComponent<CombatVariable>();
    }
    public abstract ActorData Data
    {
        get;
        set;
    }

    public virtual bool CanBeAttacked
    {
        get
        {
            return gameObject.activeSelf && Health > 0;
        }
    }
    public int MaxHealth
    {
        get
        {
            return Data.Health;
        }
    }

    public int Health
    {
        get
        {
            Debug.Log(variable);
            return variable.GetCurrentHealth();
        }
    }

    public virtual void TakeDamage(int value, Actor attacker)
    {
        variable.ReduceHealth(value);
    }

    public virtual void Attack(IDamage item, IDamagable target)
    {
        int damage = item != null ? item.Damage : 0;
        if(target != null)
            target.TakeDamage(damage + AttackDamage, this);
    }

    public virtual void Attack(int damage,IDamagable target)
    {
        target.TakeDamage(damage, this);
    }

    
    public int AttackDamage
    {
        get
        {
            return Data.Attack;
        }
    }

    public int HealthRegeneration
    {
        get
        {
            return Data.HealthRegeneration;
        }
    }

    public int MovementSpeed
    {
        get
        {
            return Data.MovementSpeed;
        }
    }

    public abstract void Notify(AI ai);

  

    public virtual void ChangeWield(Equipment item)
    {
        switch (item.Slot)
        {
            case EquipSlot.EquipmentSlotType.LEFTHAND:
                leftHand.Item = item;
                break;

            case EquipSlot.EquipmentSlotType.RIGHTHAND:
                rightHand.Item = item;
                break;
        }
    }


    public Equipment returnEquipment(EquipSlot.EquipmentSlotType slot)
    {
        switch (slot)
        {
            case EquipSlot.EquipmentSlotType.LEFTHAND:
                if (leftHand != null)
                    return leftHand.Item;
                break;

            case EquipSlot.EquipmentSlotType.RIGHTHAND:
                if (rightHand != null)
                    return rightHand.Item;
                break;
            
        }
        return null;
    }


    public new virtual Transform transform
    {
        get
        {
            return base.transform;
        }
        
    }

}
