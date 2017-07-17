using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Player : CombatActor
{

    [SerializeField]
    public AudioClip dink;

    [SerializeField]
    private Transform vivePosition;

    public static Player Instance;

    [SerializeField]
    protected PlayerData data;

    public override CombatActorData Data
    {
        get
        {
            return data;
        }

        set
        {
            data = (PlayerData)value;
        }
    }
    public int Gold
    {
        get
        {
            return data.Gold;
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
        data.JobList.Add(newJob);
    }

    public void GainExperience(JobType jobType, int value)
    {
        foreach (Job currentJob in data.JobList)
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
            return data.JobList;
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
            PlayerData _data = (PlayerData)SerializeManager.Load("PlayerData");
            
            if (_data != null)
            {
                data = _data;
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
        SerializeManager.Save("PlayerData", data);
    }

    public Job GetJob(JobType type)
    {
        foreach (Job job in data.JobList)
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
        data.Gold += value;
        if (data.Gold < 0)
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

    
}