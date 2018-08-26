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
            ai.Despawn();
            if (ai is AdventurerAI)
            {
                float duration = (ai as AdventurerAI).GetOutOfTimeDuration();
                AIManager.Instance.Spawn(ai, duration, CurrentNode, (ai as AdventurerAI).OutOfTownProgress, 1);//The 1 means every second, can be replaced with ai's capabilities if have time
                (ai as AdventurerAI).NewTownVisit();
            }
            else
            {
                Destroy(ai.gameObject);
            }
            CurrentNode.Occupied = false;
        }
    }
}
