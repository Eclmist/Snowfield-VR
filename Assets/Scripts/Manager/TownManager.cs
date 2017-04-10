using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TownManager : MonoBehaviour {

    public static TownManager Instance;

    private Town currentTown;
	// Use this for initialization
	void Awake () {
        if (!Instance)
        {
            Instance = this;
        }
        else
        {
            Debug.Log("There should only be one instance of townmanager running");
            Destroy(this);
        }
    }

    public Town CurrentTown
    {
        get
        {
            return currentTown;
        }
    }
	
    public void ChangeTown(Town _town)
    {
        currentTown = _town;
    }
}
