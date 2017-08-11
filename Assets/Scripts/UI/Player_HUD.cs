using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player_HUD : MonoBehaviour
{
	[SerializeField] private bool rightHand;

	[SerializeField] private GameObject HUD;
	[SerializeField] private GameObject VRCamera;

	[SerializeField] private Vector3 offset;
	[SerializeField] private float dotProductThreshold = -0.3F;
	[SerializeField] private float rightThreshold = 0.95F;

	[SerializeField] private float opacityLerpSpeed = 30;
	[SerializeField] private Animator animator;

	private CanvasGroup canvasGroup;
	
	void Start () {
		canvasGroup = HUD.GetComponent<CanvasGroup>();
		Debug.Assert(canvasGroup);
	}
	
	void Update () {

		HUD.transform.position = transform.position + transform.forward * offset.z + transform.right * offset.x + 
				transform.up * offset.y;

		float targetOpacity = 0;
		Quaternion targetRotation = Quaternion.LookRotation(transform.forward, transform.up);


		if ((Vector3.Dot(transform.forward, VRCamera.transform.forward) < dotProductThreshold &&
			Mathf.Abs(Vector3.Dot(transform.right, VRCamera.transform.right)) > rightThreshold && !rightHand))
		{
			targetOpacity = 1;
			targetRotation = Quaternion.LookRotation(VRCamera.transform.position - HUD.transform.position, -VRCamera.transform.up);
			
			if (animator && animator.enabled == false)
				animator.enabled = true;
		}
		else
		{
			if (animator && animator.enabled == true)
				animator.enabled = false;
		}

		HUD.transform.rotation = Quaternion.Lerp(HUD.transform.rotation, targetRotation, Time.deltaTime * opacityLerpSpeed);

		canvasGroup.alpha = Mathf.Lerp(canvasGroup.alpha, targetOpacity, Time.deltaTime * opacityLerpSpeed);


	}
}
