using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Edwon.VR.Gesture;
using Edwon.VR;

public class SpellController : MonoBehaviour
{
	protected VR_Controller_Custom controllerRef;

	protected CaptureHand handRef;

	[SerializeField]
	protected SpellHandler spellHandlerPrefab;
	protected float timer = 0f;
	protected void Start()
	{
		//if (rig == null)
		//{
		//	Debug.Log("there is no VRGestureRig in the scene, please add one");
		//}

		controllerRef = GetComponent<VR_Controller_Custom>();
		if (!controllerRef)
		{
			Debug.Log("No Controller");
			Destroy(this);
		}
	}

	protected void Update()
	{
		if (handRef == null)
		{
			if (controllerRef.Handle == VR_Controller_Custom.Controller_Handle.LEFT)
				handRef = VRGestureRig.leftCapture;
			else if (controllerRef.Handle == VR_Controller_Custom.Controller_Handle.RIGHT)
				handRef = VRGestureRig.rightCapture;
		}
		else if(controllerRef.Device != null)
		{
			if (controllerRef.Device.GetPress(SteamVR_Controller.ButtonMask.Trigger) && controllerRef.CurrentItemInHand == null)
				timer += Time.deltaTime;
			else if (controllerRef.Device.GetPressUp(SteamVR_Controller.ButtonMask.Trigger))
				timer = 0;

			if (timer > .25f && handRef.HandState != CaptureHand.State.ACTIVATED)
			{
				
				handRef.HandState = CaptureHand.State.ACTIVATED;
			}
			else if(timer < .25f && handRef.HandState != CaptureHand.State.DEACTIVATED)
			{
				handRef.HandState = CaptureHand.State.DEACTIVATED;
			}
		}

	}
	void OnEnable()
	{
		GestureRecognizer.GestureDetectedEvent += OnGestureDetected;
	}

	void OnDisable()
	{
		GestureRecognizer.GestureDetectedEvent -= OnGestureDetected;
	}

	void OnGestureDetected(string gestureName, double confidence, Handedness hand, bool isDouble = false)
	{

		if ((hand == Handedness.Left && controllerRef.Handle == VR_Controller_Custom.Controller_Handle.LEFT) || (hand == Handedness.Right && controllerRef.Handle == VR_Controller_Custom.Controller_Handle.RIGHT))
		{
            Spell spell = SpellManager.Instance.GetSpell(gestureName);
            if (spell)
            {
				controllerRef.Vibrate(10);
				SpellHandler handler = Instantiate(spellHandlerPrefab,transform.position,transform.rotation);
                handler.CastSpell(spell, controllerRef, Player.Instance);
            }
        }
	}

}
