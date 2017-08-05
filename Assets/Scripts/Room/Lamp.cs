using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Opening_Room
{

	public class Lamp : VR_Interactable_Object
	{
		[SerializeField] private Renderer topCover;
		[ColorUsageAttribute(true, true, 0f, 8f, 0, 10f)]
		[SerializeField] private Color topCoverColor;

		[SerializeField] private Renderer bulb;
		[ColorUsageAttribute(true, true, 0f, 8f, 0, 10f)]
		[SerializeField] private Color bulbColor;

		[SerializeField] private Light light;

		private bool isTurnedOn = false;

		public bool GetTurnedOn(/* ͜ʖ ͡*/)
		{
			return isTurnedOn;
		}

		protected override void OnTriggerRelease()
		{
			base.OnTriggerRelease();

			isTurnedOn = !isTurnedOn;
			interactSound.volume /= 8;
			interactSound.pitch *= 4;

			interactSound.Play();
			interactSound.volume *= 8;
			interactSound.pitch /= 4;

			UpdateLamp();
		}

		public void TurnOn(bool on)
		{
			isTurnedOn = on;
			UpdateLamp();
		}

		protected void UpdateLamp()
		{
			if (isTurnedOn)
			{
				light.intensity = 1;
				topCover.material.SetColor("_EmissionColor", topCoverColor);
				bulb.material.SetColor("_EmissionColor", bulbColor);

			}
			else
			{
				light.intensity = 0;
				topCover.material.SetColor("_EmissionColor", Color.black);
				bulb.material.SetColor("_EmissionColor", Color.black);

			}

			bulb.UpdateGIMaterials();
			topCover.UpdateGIMaterials();
		}

		protected override void PlayerLearnedInteraction()
		{
		}
	}
}