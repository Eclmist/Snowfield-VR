using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//[RequireComponent] combatfsm
[RequireComponent(typeof(CombatVariable))]
public abstract class CombatActor : Actor, IDamagable
{

    protected CombatVariable variable;
    public abstract CombatActorData Data
    {
        get;
        set;
    }

    protected void GainExperience(int value)
    {
        Data.CurrentJob.GainExperience(value);
    }

    public int Health
    {
        get
        {
            return variable.GetCurrentHealth();
        }
    }
    protected virtual void Awake()
    {

    }

    protected virtual void Start()
    {
        variable = GetComponent<CombatVariable>();
    }

    public virtual void TakeDamage(int value, Actor attacker)
    {
        variable.ReduceHealth(value);
    }

    public int MovementSpeed
    {
        get
        {
            return Data.CurrentJob.MovementSpeed;
        }
    }
    

    public int HealthRegen
    {
        get
        {
            return Data.CurrentJob.Level * Data.CurrentJob.HRPL;
        }
    }
    
    public virtual int AttackValue
    {
        get
        {
            return Data.CurrentJob.Level * Data.CurrentJob.DPL;
        }
    }

    public override int MaxHealth
    {
        get
        {
            return Data.CurrentJob.HPL * Data.CurrentJob.Level;
        }
    }

   

    public virtual void Attack(IDamage item, IDamagable target)
    {
        target.TakeDamage(item.Damage + AttackValue, this);
    }
}
