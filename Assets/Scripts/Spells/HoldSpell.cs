using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HoldSpell : Spell {

    protected bool hasFlag = false;
    protected bool casted = false;

    protected override void Hold()
    {
        if (!casted)
        {
            if (!hasFlag)
            {
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

            casted = true;
        }
    }

    protected override void Release()
    {
        if (casted)
        {
            var em = spellGO.GetComponent<ParticleSystem>().emission;

            var emsmoke = spellGO.transform.GetChild(0).gameObject.GetComponent<ParticleSystem>().emission;

            var emindicator = indicator.GetComponent<ParticleSystem>().emission;

            emindicator.enabled = false;
            em.enabled = false;
            emsmoke.enabled = false;

            Destroy(indicator, 1);
            Destroy(spellGO, 1);

            casted = false;
        }
        
    }
}
