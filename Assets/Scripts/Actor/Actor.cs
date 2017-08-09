using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
[RequireComponent(typeof(StatsContainer))]
public abstract class Actor : MonoBehaviour, IHaveStats, IDamagable
{

    [SerializeField]
    protected EquipSlot leftHand, rightHand;

    public abstract bool CheckConversingWith(Actor target);

    protected Collider thisCollider;


    protected StatsContainer variable;

    protected virtual void Awake()
    {
        variable = GetComponent<StatsContainer>();
        thisCollider = GetComponent<Collider>();
    }

    public Collider Collider
    {
        get
        {
            return thisCollider;
        }
    }
    public virtual StatsContainer StatContainer
    {
        get
        {
            return variable;
        }
    }

    public float GetBonusStatValueFromJob(Stats.StatsType s)
    {
        float bonusValue = 0;
        foreach (Job job in Data.ListOfJobs)
        {

            foreach (Stats bonusStat in job.BonusStats)
            {
                if (bonusStat.Type == s)
                {
                    bonusValue += bonusStat.Max * job.Level;
                }
            }
        }

        return bonusValue;
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
            return gameObject.activeSelf && variable.GetStat(Stats.StatsType.HEALTH).Current > 0;
        }
    }

    public virtual void TakeDamage(float value, Actor attacker)
    {
        variable.ReduceHealth(value);
        if (variable.GetStat(Stats.StatsType.HEALTH).Current <= 0)
            Die();
    }

    public virtual void Attack(IDamage item, IDamagable target, float scale = 1)
    {
        if (target != null && target.CanBeAttacked)
        {
            float damage = item != null ? item.Damage : 0;
        
            damage = damage + variable.GetStat(Stats.StatsType.ATTACK).Current * scale;
            float randomVal = Random.Range(0.8f, 1.2f);
            target.TakeDamage(damage * randomVal, this);
        }
    }

    public virtual void Attack(float damage, IDamagable target)
    {
        if (target != null && target.CanBeAttacked)
        {
            target.TakeDamage(damage, this);
            
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
            if (gameObject)
                return base.transform;
            else
                return null;
        }
    }


    public abstract void Die();

    public virtual void GainExperience(JobType jobType, int value)
    {
        foreach (Job currentJob in Data.ListOfJobs)
        {
            if (currentJob.Type == jobType)
            {
                int level = currentJob.Level;
                currentJob.GainExperience(value);
                if (currentJob.Level != level)
                    variable.UpdateVariables();
                break;
            }
        }
    }


    public List<Job> JobList
    {
        get
        {
            return Data.ListOfJobs;
        }
    }

}
