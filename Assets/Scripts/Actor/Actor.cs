using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum EquipSlot
{
    LEFTHAND,
    RIGHTHAND,
}

public class Actor : MonoBehaviour, IDamagable
{

    protected int health;

    protected List<Job> jobList = new List<Job>();

    public void AddJob(JobType newJobType)
    {
        Job newJob = new Job(newJobType);
        jobList.Add(newJob);
    }

    public void GainExperience(JobType jobType,int value)
    {
        foreach(Job currentJob in jobList)
        {
            if(currentJob.Type == jobType)
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
            if(job.Type == type)
            {
                return job;
            }
        }
        return null;
    }

    protected IInteractable leftHand, rightHand;

    public int Health
    {
        get
        {
            return health;
        }

    }

    public List<Job> JobListReference
    {
        get
        {
            return jobList;
        }
    }
    public void TakeDamage(int damage)
    {
        health -= (damage >= health) ? 0 : health - damage;
    }


    public virtual void ChangeWield(EquipSlot slot, IInteractable item)
    {
        switch (slot)
        {
            case EquipSlot.LEFTHAND:
                leftHand = item;
                break;

            case EquipSlot.RIGHTHAND:
                rightHand = item;
                break;
        }
    }

    public IInteractable returnWield(EquipSlot slot)
    {
        switch (slot)
        {
            case EquipSlot.LEFTHAND:
                return leftHand;

            case EquipSlot.RIGHTHAND:
                return rightHand;
            default:
                return null;
        }
        
    }
}
