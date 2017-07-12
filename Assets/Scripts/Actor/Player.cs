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
    private int gold;

    [SerializeField]
    private Transform vivePosition;

    [SerializeField]
    private Transform interactableArea;

    public static Player Instance;

    public int Gold
    {
        get
        {
            return gold;
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
        return Mathf.Abs(Vector3.Angle(rotation1, rotation2) - 180) < 30;
    }

    protected override void Awake()
    {
        base.Awake();
        if (!Instance)
        {
            Instance = this;
            actorData = (ActorData)SerializeManager.Load("PlayerData");
            if (actorData == null)
            {
                actorData = new ActorData(null, "Player");
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


    public bool AddGold(int value)
    {
        gold += value;
        return gold >= 0;
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