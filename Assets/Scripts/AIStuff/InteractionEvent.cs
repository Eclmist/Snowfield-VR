using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteractionEvent : NodeEvent {

    [SerializeField]
    private Actor interactTarget;

	public override void HandleEvent(AI ai)
    {
        if (interactTarget)
        {
            interactTarget.Notify(ai);
            ai.Interact(interactTarget);
        }
    }
}
