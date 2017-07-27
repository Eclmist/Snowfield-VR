using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LookAtEvent : NodeEvent
{
    [SerializeField]
    private float secondsToWait;

    [Tooltip("The angle the ai looks around the object")]
    [Range(0, 180)]
    [SerializeField]
    private float angle = 10;

    [SerializeField]
    private Transform lookAtTransform;

    public override void HandleEvent(AI ai)
    {
        ai.LookAtObject(lookAtTransform, secondsToWait, angle);
    }
}