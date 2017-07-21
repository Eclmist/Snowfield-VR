using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

[System.Serializable]
public struct VR_Button_Events
{
	public UnityEvent onControllerEnter;
	public UnityEvent onControllerStay;
	public UnityEvent onControllerExit;

	[Space(10F)]
	public UnityEvent onTriggerPress;
	public UnityEvent onTriggerHold;
	public UnityEvent onTriggerRelease;
}

[RequireComponent(typeof(Collider))]
public class VR_Button : VR_Interactable_UI
{
	[SerializeField] protected bool useTransitions = true;
	[SerializeField] protected Graphic targetGraphic;

	[SerializeField] protected Color normalColor = Color.white;
	[SerializeField] protected Color hightlightedColor = Color.white;
	[SerializeField] protected Color pressedColor = Color.yellow;
	[SerializeField] protected Color disabledColor = Color.grey;

	[SerializeField] protected float fadeDuration = 0.1F;

	[SerializeField] public VR_Button_Events events;

	protected override void OnInteractableChange()
	{
		base.OnInteractableChange();

		targetGraphic.CrossFadeColor((interactable ? normalColor : disabledColor), fadeDuration, true, true);
	}

	protected override void OnControllerEnter()
	{
		base.OnControllerEnter();

		events.onControllerEnter.Invoke();

		if (useTransitions)
		{
			targetGraphic.CrossFadeColor(hightlightedColor, fadeDuration, true, true);
		}
	}
	protected override void OnControllerStay()
	{
		base.OnControllerStay();

		events.onControllerStay.Invoke();
	}
	protected override void OnControllerExit()
	{
		events.onControllerExit.Invoke();

		base.OnControllerExit();

		if (useTransitions)
		{
			targetGraphic.CrossFadeColor(normalColor, fadeDuration, true, true);
		}
	}

	protected override void OnTriggerPress()
	{
		base.OnTriggerPress();
        Debug.Log("hit");
		events.onTriggerPress.Invoke();

		if (useTransitions)
		{
			targetGraphic.CrossFadeColor(pressedColor, fadeDuration, true, true);
		}
	}
	protected override void OnTriggerHold()
	{
		base.OnTriggerHold();

		events.onTriggerHold.Invoke();
	}

	protected override void OnTriggerRelease()
	{
		base.OnTriggerRelease();

		events.onTriggerRelease.Invoke();

		if (useTransitions)
		{
            if(targetGraphic)
			    targetGraphic.CrossFadeColor(hightlightedColor, fadeDuration, true, true);
		}
	}

	public virtual void AddOnTriggerReleaseFunction(UnityAction newAction)
	{
		events.onTriggerRelease.AddListener(newAction);
	}
}
