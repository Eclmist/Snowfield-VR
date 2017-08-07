using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.PostProcessing;

namespace Opening_Room
{
	public class Bed : VR_Interactable_Object
	{
		[SerializeField] private PostProcessingBehaviour fxBehaviour;

		[SerializeField] private Valve.VR.InteractionSystem.Hand leftHand;
		[SerializeField] private Valve.VR.InteractionSystem.Hand rightHand;

		[SerializeField] private float sleepTime;
		[SerializeField] private float sleepSpeed;

		public static bool canSleep = false;
		public static bool sleeping = false;
		public static float sleepVal;

		public static bool eyeClosed;

		public bool debugSleepTrigger;

		protected override void Start()
		{
			canSleep = false;
		}

		protected void Update()
		{
			if (debugSleepTrigger)
			{
				if (canSleep && !sleeping)
				{
					sleeping = true;
					StartCoroutine(Sleep());
				}

				debugSleepTrigger = false;

				StopHint();
			}
		}

		public override void OnTriggerPress(VR_Controller_Custom controller)
		{
			if (canSleep && !sleeping)
				base.OnTriggerPress(controller);

		}

		public override void OnTriggerRelease(VR_Controller_Custom controller)
		{

			if (canSleep && !sleeping)
			{
				base.OnTriggerRelease(controller);

				sleeping = true;
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
			PostProcessingProfile fx = fxBehaviour.profile;
			fx.vignette.enabled = true;
			fx.depthOfField.enabled = true;

			var vignetteSettings = fx.vignette.settings;
			leftHand.gameObject.SetActive(false);
			rightHand.gameObject.SetActive(false);


			var dofSettings = fx.depthOfField.settings;


			while (sleepVal < 1)
			{
				sleepVal += Time.deltaTime * sleepSpeed;
				vignetteSettings.opacity = sleepVal;
				dofSettings.focalLength = 1 + sleepVal * 10;

				fx.vignette.settings = vignetteSettings;
				fx.depthOfField.settings = dofSettings;
				yield return null;
			}

			eyeClosed = true;
			yield return new WaitForSeconds(sleepTime);
			eyeClosed = false;

			while (sleepVal > 0)
			{
				sleepVal -= Time.deltaTime * sleepSpeed;
				vignetteSettings.opacity = sleepVal;
				dofSettings.focalLength = 1 + sleepVal * 10;
				fx.vignette.settings = vignetteSettings;
				fx.depthOfField.settings = dofSettings;

				yield return null;
			}

			fx.depthOfField.enabled = false;
			fx.vignette.enabled = false;
			leftHand.gameObject.SetActive(true);
			rightHand.gameObject.SetActive(true);

			sleeping = false;
		}

		protected override void PlayerLearnedInteraction()
		{
		}

		protected void OnDisable()
		{
			PostProcessingProfile fx = fxBehaviour.profile;
			fx.vignette.enabled = false;
			fx.depthOfField.enabled = false;

		}
	}
}