using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpellHandler : GenericItem
{

	protected Actor castor;
	protected Spell currentSpell;
	protected bool hasSpell;

	public bool HasSpell
	{
		get
		{
			return hasSpell;
		}
	}
	public Actor Castor
	{
		get
		{
			return castor;
		}
	}

	protected override void Awake()
	{
		base.Awake();
		jobType = JobType.MAGIC;
	}

	public override string _Name
	{
		get
		{
			if (currentSpell)
				return currentSpell.name;
			else
				return "Blank";
		}
	}
	public override float Damage
	{
		get
		{
			if (currentSpell)
				return currentSpell.DPS;
			else
				return 0;
		}
	}

	public override void OnFixedUpdateInteraction(VR_Controller_Custom referenceCheck)
	{
		
	}

	public void CastSpell(Spell _currentSpell, VR_Controller_Custom _useController, Actor _castor)
	{
		castor = _castor;
		currentInteractingController = _useController;
		currentInteractingController.SetInteraction(this);
		currentSpell = _currentSpell;
		Spell spell = Instantiate(_currentSpell).GetComponent<Spell>();
		
		hasSpell = true;
		spell.InitializeSpell(this);

	}

	public void DecastSpell()
	{
		hasSpell = false;
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
