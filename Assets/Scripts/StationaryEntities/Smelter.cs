using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Smelter : MonoBehaviour {

    [SerializeField][Range(2f,10f)]private float smeltDuration;
    [SerializeField]private Transform funnel;   // The position where all the ores are thrown into
    [SerializeField]private float funnelRadius; // Defines how the boundaries of the funnel
    [SerializeField]private AudioClip smeltSound;   // Played when smelting 
    [SerializeField]private AudioClip doneCasting; // Played when ingot is ready for collection
    private AudioSource source;

    private List<Ore> ores; // Stores the ores that are throw into the funnel
 

    // Use this for initialization
    void Start()
    {
        ores = new List<Ore>();
        source = GetComponent<AudioSource>();
    }
	
    // Populates the ore list
    private void GetOresInFunnel()
    {
        foreach (Collider c in Physics.OverlapSphere(funnel.position, funnelRadius))
        {
            Ore ore = c.gameObject.GetComponent<Ore>();

            if (ore != null)
                ores.Add(ore);
        }
    }

    protected void Update()
    {
        if (Input.GetKeyDown(KeyCode.S))
            Smelt();
    }


    // Call this when the "smelt" button is pressed 
    private void Smelt()
    {
        bool canSmelt = false;
   
        GetOresInFunnel();      

        if(ores.Count > 0)
        {
            foreach (Ingot ingot in BlacksmithManager.Instance.Ingots)
            {
                if (isCompositionMet(ingot))
                {
                    canSmelt = true;
                    source.PlayOneShot(smeltSound);
                    StartCoroutine(SmeltProcess(ingot.PhysicalMaterial.Type));
                    break;
                }
            }
        }
       

        if (!canSmelt)
            Debug.Log("cant smelt into any ingot");


    }

    // Spawns the resulting ingot after the smelting duration
    private IEnumerator SmeltProcess(TYPE type)
    {
        yield return new WaitForSeconds(smeltDuration);
        source.PlayOneShot(doneCasting);
        BlacksmithManager.Instance.SpawnIngot(type,transform);
    }


    // Check if there are enough ores for a particular type of ingot to form
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

    // Counts the number of ores given a certain type
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
