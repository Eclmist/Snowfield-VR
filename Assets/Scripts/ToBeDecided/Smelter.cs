using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Smelter : MonoBehaviour {

    [SerializeField][Range(2f,10f)]private float smeltDuration;
    [SerializeField]private Transform funnel;
    [SerializeField]private float funnelRadius;
    [SerializeField]private AudioClip smeltSound;
    [SerializeField]private AudioClip doneCasting;
    private AudioSource source;
    private List<Ore> ores;
 

    // Use this for initialization
    void Start()
    {
        ores = new List<Ore>();
        source = GetComponent<AudioSource>();
    }
	
	// Update is called once per frame
	void Update () {
        
        if(Input.GetKeyDown(KeyCode.S))
        {
            Smelt();
        }

    }

    private void GetOresInFunnel()
    {
        foreach (Collider c in Physics.OverlapSphere(funnel.position, funnelRadius))
        {
            Ore ore = c.gameObject.GetComponent<Ore>();

            if (ore != null)
                ores.Add(ore);
        }
    }


    // Button press
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

    private IEnumerator SmeltProcess(TYPE type)
    {
        yield return new WaitForSeconds(smeltDuration);
        source.PlayOneShot(doneCasting);
        BlacksmithManager.Instance.SpawnIngot(type);
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
