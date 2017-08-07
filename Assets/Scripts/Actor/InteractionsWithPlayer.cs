using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class InteractionsWithPlayer : MonoBehaviour {

    protected bool hasInteracted = false;

    protected FriendlyAI currentAI;

    protected IUI currentUI;

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
    protected virtual void Awake()
    {
        currentAI = GetComponent<FriendlyAI>();
        if(!currentAI)
        {
            Destroy(this);
        }
    }
    public bool IsInteracting {
        get
        {
            if (currentUI != null && currentUI.Equals(null))
                currentUI = null;
            return currentUI != null;
        }
    }

    public void StopInteraction()
    {
        if (currentUI != null)
            currentUI.ClosePane();
    }

    public abstract bool StartInteraction();

}
