using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WarpEvent : NodeEvent {

    [SerializeField]
    private Node warpPoint;
    private Node currentPoint;

    public Node WarpPoint
    {
        get
        {
            return warpPoint;
        }
        set
        {
            currentPoint = value;
        }
    }
	
    protected override void Awake()
    {
        base.Awake();
        currentPoint = warpPoint;
    }

    public override void HandleEvent(AI ai)
    {
        if(Vector3.Distance(ai.transform.position,transform.position) > .01)
            ai.Warp(currentPoint.Position);
        //if null despawn
    }
}
