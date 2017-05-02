using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Smelter : MonoBehaviour {

    private List<Ore> ores;

	// Use this for initialization
	void Start () {

        ores = new List<Ore>();
	}
	
	// Update is called once per frame
	void Update () {

        


    }

    void OnTriggerEnter(Collider other)
    {
        Ore ore = other.gameObject.GetComponent<Ore>();

        if (ore != null)
            ores.Add(ore);



    }


    private void Smelt()
    {

        foreach(Ingot ingot in BlacksmithManager.Instance.Ingots)
        {
            if(isCompositionMet(ingot))
            {
                BlacksmithManager.Instance.SpawnIngot(ingot.PhysicalMaterial.Type);
            }
        }
    }

    private bool isCompositionMet(Ingot ingot)
    {
        int currentComp = 0;

        foreach(Ore ore in ores)
        {
            if(ore.Type == ingot.PhysicalMaterial.Type)
                currentComp++;
        }

        return (currentComp == ingot.OreComposition);
           

    }


    private int CountOresOfType(TYPE type)
    {
        int count = 0;

        foreach(Ore ore in ores)
        {
            if (ore.Type == type)
                count++;
        }

        return count;
    }
 


}
