using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OutOfTownEvent : NodeEvent
{

    public override void HandleEvent(AI ai)
    {
        if (ai is FriendlyAI)
        {
            float duration = (ai as FriendlyAI).GetOutOfTimeDuration();
            ai.Despawn();
            AIManager.Instance.Spawn(ai, duration, CurrentNode, ai.OutOfTownProgress, 1);//The 1 means every second, can be replaced with ai's capabilities if have time
            CurrentNode.Occupied = false;
        }
    }
}
