using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoltSpell : Spell {

    [SerializeField]
    private GameObject bolt;

    [SerializeField]
    private int castCount;

    public override void InitializeSpell(SpellHandler handler)
    {
        base.InitializeSpell(handler);
        transform.position = handler.transform.position;
        transform.rotation = handler.transform.rotation;
        transform.parent = handler.transform;
    }

    public override void Update()
    {
        base.Update();

        if (castCount <= 0)
        {
            var em = this.gameObject.GetComponent<ParticleSystem>().emission;
            em.enabled = false;
            Destroy(this.gameObject, 1);

            handler.DecastSpell();
        }
    }

    public override void OnTriggerPress()
    {
        if (castCount > 0)
        {
            if (bolt != null)
            {
                Instantiate(bolt, this.gameObject.transform).transform.parent = null;
                castCount--;
            }
        }
    }

}
