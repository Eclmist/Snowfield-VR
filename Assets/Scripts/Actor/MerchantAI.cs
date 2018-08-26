using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[RequireComponent(typeof(Merchant))]

public class MerchantAI : FriendlyAI {

    protected Merchant merChant;

    [SerializeField]
    protected ActorData data;

    public override ActorData Data
    {
        get
        {
            return data;
        }

        set
        {
            data = value;
        }
    }

    protected override void Awake()
    {
        base.Awake();
        merChant = GetComponent<Merchant>();
    }

    public override bool CanSpawn()
    {
        return !WaveManager.Instance.HasMonster;
    }
}
