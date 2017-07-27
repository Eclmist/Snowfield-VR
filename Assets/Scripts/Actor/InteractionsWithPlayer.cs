using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class InteractionsWithPlayer : MonoBehaviour {

    protected bool hasInteracted = false;

    protected AdventurerAI currentAI;

    protected OptionPane currentPane;

    public bool Interacted
    {
        get
        {
            return hasInteracted;
        }
        set
        {
            hasInteracted = value;
        }
    }
    protected void Awake()
    {
        currentAI = GetComponent<AdventurerAI>();
        if(!currentAI)
        {
            Destroy(this);
        }
    }
    public bool IsInteracting {
        get
        {
            return currentPane != null;
        }
    }

    public void StopInteraction()
    {
        if (currentPane)
            currentPane.ClosePane();
    }

    public abstract bool StartInteraction();

}
