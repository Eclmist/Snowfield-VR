using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.PostProcessing;

namespace Opening_Room
{
	public class Bed : VR_Interactable_Object
	{
		[SerializeField] private PostProcessingProfile fx;
		[SerializeField] private Valve.VR.InteractionSystem.Hand leftHand;
		[SerializeField] private Valve.VR.InteractionSystem.Hand rightHand;

		[SerializeField] private float sleepTime;
		[SerializeField] private float sleepSpeed;

		private float sleepVal = 0;

		public static bool canSleep;
		public static bool eyeClosed;
		public override void OnTriggerPress(VR_Controller_Custom ctrl)
		{
			if (canSleep)
			{
				base.OnTriggerPress(ctrl);
				StartCoroutine(Sleep());
			}
		}

		public override void OnControllerEnter(VR_Controller_Custom controller)
		{
			if (canSleep)
				base.OnControllerEnter(controller);
		}

		IEnumerator Sleep()
		{
			fx.vignette.enabled = true;
			fx.depthOfField.enabled = true;

			var vignetteSettings = fx.vignette.settings;
			leftHand.gameObject.SetActive(false);
			rightHand.gameObject.SetActive(false);


			var dofSettings = fx.depthOfField.settings;


			while (sleepVal < 1)
			{
				sleepVal += Time.deltaTime * sleepSpeed;
				vignetteSettings.intensity = sleepVal;
				dofSettings.focalLength = 1 + sleepVal * 10;
				yield return null;
			}

			eyeClosed = true;
			yield return new WaitForSeconds(sleepTime);
			eyeClosed = false;

			leftHand.gameObject.SetActive(true);
			rightHand.gameObject.SetActive(true);
			

			while (sleepVal > 0)
			{
				sleepVal -= Time.deltaTime * sleepSpeed;
				vignetteSettings.intensity = sleepVal;
				dofSettings.focalLength = 1 + sleepVal * 10;
				yield return null;
			}

			fx.depthOfField.enabled = false;
			fx.vignette.enabled = false;

		}
	}
}