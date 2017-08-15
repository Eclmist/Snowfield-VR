using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Valve.VR.InteractionSystem;

public class LoadGame : VR_Interactable_Object
{
	public virtual void OnControllerEnter(VR_Controller_Custom controller)
	{
		base.OnControllerEnter();

		ControllerButtonHints.ShowTextHint(controller.Hand, Valve.VR.EVRButtonId.k_EButton_SteamVR_Trigger, "Load Game")
	}


	public virtual void OnTriggerPress(VR_Controller_Custom controller)
	{
		base.OnTriggerPress();
		SceneLoader.LoadScene.Instance.Load(1);
	}

}
