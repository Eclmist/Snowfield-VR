using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : Actor
{

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

    protected void Awake()
    {
        if (!Instance)
            Instance = this;
        else
        {
            Debug.Log("There should only be one instanc of Player.cs in the scene!");
            Destroy(this);
        }
    }

    public void AddGold(int value)
    {
        gold += value;
        if (gold < 0)
        {
            gold = 0;//for display purposes
            //set lose condition
        }
    }

    
}