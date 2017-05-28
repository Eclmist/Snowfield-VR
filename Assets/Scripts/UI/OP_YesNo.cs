using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;


public class OP_YesNo : OptionPane
{

	public VR_Button btnYes;
	public VR_Button btnNo;

	public override void SetEvent(ButtonType button, UnityAction func)
	{
		switch (button)
		{
			case ButtonType.Yes:
				btnYes.AddOnTriggerReleaseFunction(func);
				break;
			case ButtonType.No:
				btnNo.AddOnTriggerReleaseFunction(func);
				break;
		}
	}

	public override void SetActiveButtons(int active)
	{
		btnYes.interactable = active == 1 ? true : false;
		btnNo.interactable = active == 1 ? true : false;
	}

}
