using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Spell : VR_Interactable_Object
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

    protected virtual void Cast()
    {

    }

    protected virtual void Release()
    {

    }

	public override void OnUpdateInteraction(VR_Controller_Custom controller)
    {
		base.OnUpdateInteraction(controller);
        transform.position = LinkedController.transform.position;
        transform.rotation = LinkedController.transform.rotation;
    }

	protected override void OnTriggerPress()
    {
        Cast();
    }

   
	protected override void OnTriggerHold()
    {
		base.OnTriggerHold();

		Debug.Log("Holding");
	}

	protected override void OnTriggerRelease()
    {
        Release();
    }
}
