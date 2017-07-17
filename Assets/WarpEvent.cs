using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WarpEvent : NodeEvent {

    [SerializeField]
    private Node warpPoint;

    [SerializeField]
    private float timeForWarp;

    
    public override void HandleEvent(AI ai)
    {
        ai.gameObject.SetActive(false);
        //if null despawn
        AIManager.Instance.Spawn(ai, timeForWarp, warpPoint);
    }
}
