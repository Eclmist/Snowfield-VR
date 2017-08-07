using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoltSpell : Spell {

    [SerializeField]
    private int castCount;

    protected override void Update()
    {
        base.Update();

        if (castCount <= 0)
        {
            var em = Indicator.GetComponent<ParticleSystem>().emission;
            em.enabled = false;
            Destroy(Indicator, 1);
        }
    }

    protected override void Cast()
    {
        Instantiate(spellPrefab);
    }
}
