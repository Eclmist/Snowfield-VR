using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HoldSpell : Spell {

    protected override void Start()
    {
        Debug.Log("Started");
    }

    protected bool hasFlag = false;

    protected override void Hold()
    {
        Debug.Log("Holding");

        if (!hasFlag)
        {
            var em = indicator.GetComponent<ParticleSystem>().emission;
            em.enabled = false;

            //Create a fire Charge here

            spellGO = Instantiate(spellPrefab, currentInteractingController.transform);

            hasFlag = true;
        }
        else
        {
            var em = spellGO.GetComponent<ParticleSystem>().emission;

            var emsmoke = spellGO.transform.GetChild(0).gameObject.GetComponent<ParticleSystem>().emission;

            em.enabled = true;
            emsmoke.enabled = true;
        }
    }

    protected override void Release()
    {
        Debug.Log("Released");

        var em = spellGO.GetComponent<ParticleSystem>().emission;

        var emsmoke = spellGO.transform.GetChild(0).gameObject.GetComponent<ParticleSystem>().emission;

        em.enabled = false;
        emsmoke.enabled = false;

        Destroy(indicator, 3);
        Destroy(spellGO, 3);
    }
}
