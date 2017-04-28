using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlacksmithManager : MonoBehaviour {

    public static BlacksmithManager Instance;

    private List<Ingot> availableIngots;


	void Awake()
    {
        Instance = this;
        availableIngots = new List<Ingot>();

        StoreReferences();
    }

	
	// Update is called once per frame
	void Update () {
		
	}

    private void StoreReferences()
    {
        Object[] temp = Resources.LoadAll("Ingots");

        foreach (Object o in temp)
        {
            availableIngots.Add(o as Ingot);
        }
    }

   

   
}
