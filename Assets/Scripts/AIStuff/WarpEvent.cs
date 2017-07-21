using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WarpEvent : NodeEvent
{

    [SerializeField]
    private Node warpPoint;

    [SerializeField]
    private float timeForWarp;


    public override void HandleEvent(AI ai)
    {
        if (ai is AdventurerAI)
        {
            Debug.Log("hitttt");
            if (Vector3.Distance(ai.transform.position, CurrentNode.Position) > .01)
            {
                ai.gameObject.SetActive(false);
                ai.SetNode(CurrentNode);
                AIManager.Instance.Spawn(ai, timeForWarp, warpPoint);
            }
        }
    }
}
