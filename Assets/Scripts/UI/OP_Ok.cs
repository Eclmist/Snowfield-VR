using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class OP_Ok : OptionPane 
{

	public VR_Button btnOk;

	public override void SetEvent(ButtonType button, UnityAction func)
	{
		switch (button)
		{
			case ButtonType.Ok:
				btnOk.AddOnTriggerReleaseFunction(func);
				break;
		}
	}

	public override void SetActiveButtons(int active)
	{
		btnOk.interactable = active == 1 ? true : false;
	}
}
