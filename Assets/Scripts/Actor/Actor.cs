using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Actor : MonoBehaviour, IDamagable
{
    [SerializeField]
    protected int health = 100;

    [SerializeField]
    protected EquipSlot leftHand, rightHand;

    protected int maxHealth;

    protected List<Job> jobList = new List<Job>();

    public void AddJob(JobType newJobType)
    {
        Job newJob = new Job(newJobType);
        jobList.Add(newJob);
    }

    protected virtual void Awake()
    {
        maxHealth = health;
    }
    public void GainExperience(JobType jobType, int value)
    {
        foreach (Job currentJob in jobList)
        {
            if (currentJob.Type == jobType)
            {
                currentJob.GainExperience(value);
                break;
            }
        }
    }

    public Job GetJob(JobType type)
    {
        foreach (Job job in jobList)
        {
            if (job.Type == type)
            {
                return job;
            }
        }
        return null;
    }

    public int Health
    {
        get
        {
            return health;
        }

    }

    public virtual void Attack(IDamage item, IDamagable target)
    {
        target.TakeDamage(item.Damage, this);
    }

    public int MaxHealth
    {
        get
        {
            return maxHealth;
        }
    }
    public List<Job> JobListReference
    {
        get
        {
            return jobList;
        }
    }
    public virtual void TakeDamage(int damage, Actor attacker)
    {
        health -= (damage >= health) ? health : damage;
        health = health > maxHealth ? maxHealth : health;
    }

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
                else return null;

            case EquipSlot.EquipmentSlotType.RIGHTHAND:
                if (rightHand != null)
                    return rightHand.Item;
                else
                    return null;
            default:
                return null;
        }

    }

    public Weapon GetLongestWeapon()
    {
        float LongestRange = 0;
        Weapon LongestWeapon = null;
        if (leftHand != null && leftHand.Item is Weapon)
        {
            LongestRange = (leftHand.Item as Weapon).Range;
            LongestWeapon = leftHand.Item as Weapon;
        }
        if (rightHand != null && rightHand.Item is Weapon)
        {
            if ((rightHand.Item as Weapon).Range > LongestRange)
                LongestWeapon = rightHand.Item as Weapon;
        }
        return LongestWeapon;
    }
  

   

    public new virtual Transform transform
    {
        get
        {
            return base.transform;
        }
        
    }

}
