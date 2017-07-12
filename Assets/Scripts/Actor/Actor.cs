using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public abstract class Actor : MonoBehaviour, IDamagable
{

    [SerializeField]
    protected int health = 100;
    [SerializeField]
    protected TempKillScript deathAnim;
    [SerializeField]
    protected EquipSlot leftHand, rightHand;

    protected ActorData actorData;

    public ActorData Data
    {
        get
        {
            return actorData;
        }
        set
        {
            
            actorData = value;
        }
    }


    public void AddJob(JobType newJobType)
    {
        Job newJob = new Job(newJobType);
        Debug.Log(actorData);
        actorData.JobList.Add(newJob);
    }

    public abstract bool CheckConversingWith(Actor target);

    protected virtual void Awake()
    {
        deathAnim = GetComponent<TempKillScript>();
    }
    public void GainExperience(JobType jobType, int value)
    {
        foreach (Job currentJob in actorData.JobList)
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
        foreach (Job job in actorData.JobList)
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
            return actorData.Health;
        }
    }
    public List<Job> JobListReference
    {
        get
        {
            return actorData.JobList;
        }
    }

    public abstract void Notify(AI ai);

    public virtual void TakeDamage(int damage, Actor attacker)
    {
        health -= (damage >= health) ? health : damage;
        health = health > actorData.Health ? actorData.Health : health;
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
                break;

            case EquipSlot.EquipmentSlotType.RIGHTHAND:
                if (rightHand != null)
                    return rightHand.Item;
                break;
            
        }
        return null;
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

    public void PlayDeath()
    {
        deathAnim.Kill();
    }
}
