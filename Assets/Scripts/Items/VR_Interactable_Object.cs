using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class VR_Interactable_Object : VR_Interactable_Thing
{

    protected Rigidbody rigidBody;

    [SerializeField]
    public bool interactable = true;

    [Header("Vibrations")]
    [SerializeField]
    [Range(0, 10)]
    protected float triggerEnterVibration = 0.8F;
    [SerializeField]
    [Range(0, 10)]
    protected float triggerExitVibration = 0.3F;
    [SerializeField]
    [Range(0, 10)]
    protected float triggerPressVibration = 0;

	private Vector3 currentReleaseVelocityMagnitude = Vector3.zero, currentReleaseAngularVelocityMagnitude = Vector3.zero;

	// Outline Rendering
	[SerializeField]
	private Color outlineColor = Color.yellow;

	private Renderer[] childRenderers;
	private List<Material> childMaterials = new List<Material>();



	protected override void Awake()
	{
		rigidBody = GetComponent<Rigidbody> ();
		childRenderers = GetComponentsInChildren<Renderer>();

		foreach (Renderer r in childRenderers)
		{
			if (r.GetComponent<ParticleSystem>())
				continue;

			foreach (Material m in r.materials)
			{
				if (m)
				{
					m.SetOverrideTag("RenderType", "Outline");
					m.SetColor("_OutlineColor", Color.black);
					childMaterials.Add(m);
				}
			}
		}
	}
		

	protected virtual void Update()
	{
	}

	public override void OnControllerEnter(VR_Controller_Custom controller) {
        controller.Vibrate(triggerEnterVibration);
		SetOutline(true);
    }
		

	public override void OnControllerExit(VR_Controller_Custom controller)
	{
		SetOutline(false);
	}

	public override void OnTriggerPress(VR_Controller_Custom controller)
    {
		SetOutline(false);

		if (currentInteractingController != null)
            currentInteractingController.SetInteraction(null);

        currentInteractingController = controller;

        currentInteractingController.SetInteraction(this);
    }
		

	public override void OnTriggerRelease(VR_Controller_Custom controller)
    {
        currentInteractingController = null;
        controller.SetInteraction(null);
        
        rigidBody.velocity = currentReleaseVelocityMagnitude;
        rigidBody.angularVelocity = currentReleaseAngularVelocityMagnitude;
        Debug.Log("CurrentReleaseVelocity:" + rigidBody.velocity);
        Debug.Log(rigidBody.angularVelocity);
    }
		

	public override void OnInteracting(VR_Controller_Custom controller) {

        if (currentReleaseVelocityMagnitude.magnitude > controller.Velocity.magnitude)
            currentReleaseVelocityMagnitude = Vector3.Lerp(currentReleaseVelocityMagnitude, controller.Velocity, Time.fixedDeltaTime * 5);
        else
            currentReleaseVelocityMagnitude = controller.Velocity;
        if(currentReleaseAngularVelocityMagnitude.magnitude > controller.AngularVelocity.magnitude)
        currentReleaseAngularVelocityMagnitude = Vector3.Lerp(currentReleaseAngularVelocityMagnitude, controller.AngularVelocity, Time.fixedDeltaTime * 5);
        else
            currentReleaseAngularVelocityMagnitude = controller.AngularVelocity;

    }

	public void SetOutline(bool enabled)
	{
		SetOutlineColor(enabled ? outlineColor : Color.black);
	}

	private void SetOutlineColor(Color color)
	{
		foreach (Material m in childMaterials)
		{
			m.SetColor("_OutlineColor", color);
		}
	}
}
