using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpellHandler : VR_Interactable_Object {


	protected Spell currentSpell;

	public void CastSpell(Spell _currentSpell, VR_Controller_Custom _useController, Actor castor)
	{
		currentInteractingController = _useController;
		currentInteractingController.SetInteraction(this);
		Spell spell = Instantiate(_currentSpell).GetComponent<Spell>();
		spell.InitializeSpell(castor, _useController);
	}

	public void DecastSpell()
	{
		currentInteractingController.SetInteraction(null);
	}

	public override void OnTriggerPress(VR_Controller_Custom controller)
	{
		
	}

	public override void OnTriggerRelease(VR_Controller_Custom controller)
	{
		
	}
}
