using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Target 


public class TargetSpell : Spell {

    private int castCount;

    protected override void Start()
    {
        castCount = 3;
    }

    protected override void Cast()
    {
        if (castCount > 0)
        {
            Instantiate(spellPrefab);
        }
        else
        {
            var em = Indicator.GetComponent<ParticleSystem>().emission;
            em.enabled = false;
            Destroy(Indicator, 3);
        }
    }




}
