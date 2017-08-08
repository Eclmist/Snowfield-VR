using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Spell
{
    [SerializeField]
    protected GameObject spellPrefab;
    [SerializeField]
    protected GameObject indicator;

    protected GameObject spellGO;

    public GameObject SpellPrefab
    {
        get { return this.spellPrefab; }
    }

    public GameObject Indicator
    {
        get { return this.indicator; }
    }


	public override void OnUpdateInteraction(VR_Controller_Custom controller)
    {
		base.OnUpdateInteraction(controller);
        transform.position = LinkedController.transform.position;
        transform.rotation = LinkedController.transform.rotation;
    }

	protected override void OnTriggerPress()
    {
        base.OnTriggerPress();
    }

   
	protected override void OnTriggerHold()
    {
		base.OnTriggerHold();
	}

	protected override void OnTriggerRelease()
    {
        base.OnTriggerRelease();
    }
}
