using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Player : Actor
{

    [SerializeField]
    public AudioClip dink;

    [SerializeField]
    private Transform vivePosition;

    public static Player Instance;
    [SerializeField]
    protected PlayerData actorData;
    public int Gold
    {
        get
        {
            return actorData.Gold;
        }
    }

    public override int AttackValue
    {
        get
        {
            
            return actorData.CurrentJob.Level * actorData.CurrentJob.DPL;
        }
    }

    public override int MaxHealth
    {
        get
        {
            return actorData.CurrentJob.HPL * actorData.CurrentJob.Level;
        }
    }
    public override void Notify(AI ai)
    {//Unimplemented .. test code
        AudioSource ad = GetComponent<AudioSource>();
        ad.Play();
    }

    public void AddJob(JobType newJobType)
    {
        Job newJob = new Job(newJobType);
        actorData.JobList.Add(newJob);
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

    public List<Job> JobList
    {
        get
        {
            return actorData.JobList;
        }
    }
    public override bool CheckConversingWith(Actor target)
    {
        Vector3 rotation1 = transform.forward;
        Vector3 rotation2 = target.transform.forward;
        rotation1.y = rotation2.y = 0;
        return Mathf.Abs(Vector3.Angle(rotation1, rotation2) - 180) < 30;
    }

    protected override void Awake()
    {
        base.Awake();
        if (!Instance)
        {
            Instance = this;
            PlayerData data = (PlayerData)SerializeManager.Load("PlayerData");
            if (data != null)
            {
                actorData = data;
            }
            else
            {
                AddJob(JobType.BLACKSMITH);
            }
        }
        else
        {
            Debug.Log("There should only be one instanc of Player.cs in the scene!");
            Destroy(this);
        }
    }

    protected void OnDisable()
    {
        SerializeManager.Save("PlayerData",actorData);
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

    public void AddGold(int value)
    {
        (actorData as PlayerData).Gold += value;
        if ((actorData as PlayerData).Gold < 0)
            ;//lose
    }


    //void OnDrawGizmos()
    //{
    //    Gizmos.DrawSphere(interactableArea.position, 1);
    //}

    public override Transform transform
    {
        get
        {
            return vivePosition;
        }
    }

    public override ActorData Data
    {
        get
        {
            return actorData;
        }

        set
        {
            actorData = (PlayerData)value;
        }
    }
}