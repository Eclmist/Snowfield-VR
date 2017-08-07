using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class FriendlyAI : AI {

    private List<InteractionsWithPlayer> interactionsWithPlayer = new List<InteractionsWithPlayer>();

    protected virtual void Start()
    {
        interactionsWithPlayer.AddRange(GetComponents<InteractionsWithPlayer>());
    }
    public void StopAllInteractions()
    {
        foreach (InteractionsWithPlayer interaction in interactionsWithPlayer)
        {
            interaction.StopInteraction();
        }
    }

    public virtual float GetOutOfTimeDuration()
    {
        return 0;
    }

    public bool Interacting
    {
        get
        {
            foreach (InteractionsWithPlayer interaction in interactionsWithPlayer)
            {
                if (interaction.IsInteracting)
                    return true;
            }
            return false;
        }
    }

    public override void Interact(Actor actor)
    {
        (currentFSM as FriendlyAiFSM).StartInteractRoutine(actor);
    }

    public bool IsInteractionAvailable()
    {
        foreach (InteractionsWithPlayer interaction in interactionsWithPlayer)
        {
            if (!interaction.Interacted)
                return true;
        }

        return false;
    }

    public bool StartInteraction()
    {
        foreach (InteractionsWithPlayer interaction in interactionsWithPlayer)
        {
            if (!interaction.Interacted)
            {
                interaction.StartInteraction();
                return true;
            }
        }

        return false;
    }

}
