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
			var em = this.gameObject.GetComponentInChildren<ParticleSystem>().emission;
            em.enabled = false;

			handler.DecastSpell();

			Destroy(this.gameObject, 3);
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
