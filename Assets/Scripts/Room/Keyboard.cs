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

		public override void OnTriggerRelease(VR_Controller_Custom ctrl)
		{
			playerKnowsHowToInteractWithObjects = false;

			base.OnTriggerRelease(ctrl);

			queueKeypress = true;
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

		protected override void PlayerLearnedInteraction()
		{
		}
	}
}