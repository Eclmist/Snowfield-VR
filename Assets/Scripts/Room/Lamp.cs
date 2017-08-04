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

		protected override void OnTriggerPress()
		{
			base.OnTriggerPress();


			if (isTurnedOn)
			{
				light.intensity = 0;
				topCover.material.SetColor("_EmissionColor", Color.black);
				bulb.material.SetColor("_EmissionColor", Color.black);

			}
			else
			{
				light.intensity = 1;
				topCover.material.SetColor("_EmissionColor", topCoverColor);
				bulb.material.SetColor("_EmissionColor", bulbColor);

			}

			bulb.UpdateGIMaterials();
			topCover.UpdateGIMaterials();

			isTurnedOn = !isTurnedOn;
		}

	}
}