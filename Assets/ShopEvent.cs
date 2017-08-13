using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShopEvent : NodeEvent {

    [SerializeField]
    protected float minShopTime, maxShopTime;
    public override void HandleEvent(AI ai)
    {
        if(ai is FriendlyAI)
        {
            ai.Despawn();
            float randomNumber = UnityEngine.Random.Range(minShopTime, maxShopTime);
            AIManager.Instance.Spawn(ai, randomNumber, CurrentNode);
        }
        CurrentNode.Occupied = false;
    }
}
