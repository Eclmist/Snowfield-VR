using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : Actor
{
    [SerializeField]
    private int gold;

    public static Player Instance;

    public int Gold
    {
        get
        {
            return gold;
        }
    }

    protected void Start()
    {
        AddJob(JobType.BLACKSMITH);
    }

    protected override void Awake()
    {
        base.Awake();
        if (!Instance)
            Instance = this;
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

    public override void Notify()
    {
        throw new NotImplementedException();
    }
}