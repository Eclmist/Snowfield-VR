using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Valve.VR.InteractionSystem;

namespace Opening_Room
{

	public class Keyboard : VR_Interactable_Object
	{

		private bool keypress = false;

		public bool queueKeypress = false;	//todo: make private

		public bool GetKey()
		{
			return keypress;
		}

		private string hoverhint = "";

		public override void OnTriggerPress(VR_Controller_Custom ctrl)
		{
			base.OnTriggerPress();

			queueKeypress = true;

			ControllerButtonHints.HideAllButtonHints(ctrl.Hand);
			hoverhint = "";

		}

		public override void OnControllerEnter(VR_Controller_Custom ctrl)
		{
			base.OnControllerEnter();

			if (hoverhint != "")
				ControllerButtonHints.ShowTextHint(ctrl.Hand, Valve.VR.EVRButtonId.k_EButton_SteamVR_Trigger, hoverhint);
		}

		public override void OnControllerExit(VR_Controller_Custom ctrl)
		{
			base.OnControllerExit();

			ControllerButtonHints.HideAllButtonHints(ctrl.Hand);
		}


		void Update()
		{
			if (queueKeypress)
			{
				keypress = true;
				queueKeypress = false;
			}
			else
			{
				keypress = false;
			}
		}

		public void SetHoverHint(string text)
		{
			hoverhint = text;
		}

	}
}