using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ward : MonoBehaviour // , IBLOCK
{
	[SerializeField] private AnimationCurve opacityCurve;
	[SerializeField] [Range(0,10)] private float fadeSpeed = 5;

	private float opacitySlider;
	[SerializeField] private bool wardActive = false;

	private ModifyRenderer renMod;


	protected void Start()
	{
		renMod = GetComponent<ModifyRenderer>();

		if (renMod == null)
		{
			Destroy(gameObject);
		}
	}

	protected void Update()
	{
		if (wardActive)
		{
			if (opacitySlider < 1)
			{
				opacitySlider += fadeSpeed * Time.deltaTime;
				if (opacitySlider > 1)
					opacitySlider = 1;
			}
		}
		else
		{
			if (opacitySlider > 0)
			{
				opacitySlider -= fadeSpeed * Time.deltaTime;
				if (opacitySlider < 0)
					opacitySlider = 0;
			}

		}

		renMod.SetFloat("_Opacity", opacityCurve.Evaluate(opacitySlider));
	}


	public void SetWardActive(bool active)
	{
		wardActive = active;
	}
}
