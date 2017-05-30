﻿using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : Actor
{
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



    protected override void Awake()
    {
        base.Awake();
        if (!Instance)
        {
            Instance = this;
            AddJob(JobType.BLACKSMITH);
        }
        else
        {
            Debug.Log("There should only be one instanc of Player.cs in the scene!");
            Destroy(this);
        }
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