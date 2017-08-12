using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Player : Actor,ICanSerialize
{

    public float height = 1.75f;//Default hardcoded player heigh

    [SerializeField]
    public AudioClip dink;

    [SerializeField]
    private Transform vivePosition;

    public static Player Instance;
    [SerializeField]
    protected PlayerData data;

    protected float currentGroundHeight = 1;

    public float CurrentGroundHeight
    {
        get {
            return currentGroundHeight;
        }
        set {
            currentGroundHeight = value;
        }
    }

    public string SerializedFileName
    {
        get
        {
            return "PlayerData";
        }
    }


    public override ActorData Data
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
        set
        {
            data.Gold = value;
        }
    }
    
    public override void Notify(AI ai)
    {//Unimplemented .. test code
        AudioSource ad = GetComponent<AudioSource>();
        ad.Play();
    }

   
    public override bool CheckConversingWith(Actor target)
    {
        Vector3 rotation1 = transform.forward;
        Vector3 rotation2 = target.transform.forward;
        rotation1.y = rotation2.y = 0;
       
        return (Mathf.Abs(Vector3.Angle(rotation1, rotation2) - 180) < 30) && Vector3.Distance(transform.position,target.transform.position) < 5;
    }

    protected override void Awake()
    {
        base.Awake();
        if (!Instance)
        {
            Instance = this;
            PlayerData _data = (PlayerData)SerializeManager.Load(SerializedFileName);
            if (_data != null)
            {
                data = _data;
            }
            else
            {
				data = new PlayerData(data, "Player", null);
				Job j = Data.AddJob(JobType.BLACKSMITH);
                Stats s = new Stats(Stats.StatsType.ATTACK, 2);
                j.AddStats(s);
            }
        }
        else
        {
            Debug.Log("There should only be one instanc of Player.cs in the scene!");
            Destroy(this);
        }
        float h = PlayerPrefs.GetFloat("PlayerHeight", -1);
        if (h != -1)
        {
            height = h;
        }
    }

    public virtual void CastSpell(float value, IDamagable target)
    {
        if (target != null && target.CanBeAttacked)
        {
            float randomVal = UnityEngine.Random.Range(0.8f, 1.2f);
            DealDamage(value * randomVal, target, JobType.MAGIC);
        }
    }
    public override void Die()
    {
        //Lose here
    }

    public void Save()
    {
        //SerializeManager.Save(SerializedFileName,data);
    }



    public void AddGold(int value)
    {
        Gold += value;
    }

    public int EXPBottles
    {
        get
        {
            return data.EXPBottles;
        }
        set
        {
            data.EXPBottles = value;
        }
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