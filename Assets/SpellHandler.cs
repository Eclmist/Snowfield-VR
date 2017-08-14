using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpellHandler : VR_Interactable_Object
{

    protected Actor castor;
    protected Spell currentSpell;

    public Actor Castor
    {
        get
        {
            return castor;
        }
    }

    public void CastSpell(Spell _currentSpell, VR_Controller_Custom _useController, Actor _castor)
    {
        castor = _castor;
        currentInteractingController = _useController;
        currentInteractingController.SetInteraction(this);
        Spell spell = Instantiate(_currentSpell).GetComponent<Spell>();
        spell.InitializeSpell(this);

    }

    public void DecastSpell()
    {
        if (currentInteractingController.InteractedObject == this)
            currentInteractingController.SetInteraction(null);

		Destroy(this.gameObject, 3);
    }

    public override void OnTriggerPress(VR_Controller_Custom controller)
    {

    }

	public override void OnUpdateInteraction(VR_Controller_Custom controller)
	{
		transform.position = controller.transform.position;
		transform.rotation = controller.transform.rotation;
	}

	public override void OnTriggerRelease(VR_Controller_Custom controller)
    {

    }
}
