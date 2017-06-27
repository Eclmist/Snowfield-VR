using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class VR_Interactable_Object : MonoBehaviour
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

    protected VR_Controller_Custom currentInteractingController;


	// Outline Rendering
	[SerializeField]
	private Color outlineColor = Color.yellow;

	private Renderer[] childRenderers;
	private List<Material> childMaterials = new List<Material>();

	public VR_Controller_Custom LinkedController
    {
        get
        {
            return currentInteractingController;
        }
        set
        {
            currentInteractingController = value;
        }
    }

    protected virtual void Awake()
    {
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

	protected virtual void Start()
	{
		rigidBody = GetComponent<Rigidbody>();
	}

	protected virtual void Update()
	{
	}

	public virtual void OnControllerEnter(VR_Controller_Custom controller) {
        controller.Vibrate(triggerEnterVibration);
		SetOutline(true);
    }

    public virtual void OnControllerStay(VR_Controller_Custom controller) { }

	public virtual void OnControllerExit(VR_Controller_Custom controller)
	{
		SetOutline(false);
	}

	public virtual void OnTriggerPress(VR_Controller_Custom controller)
    {
		SetOutline(false);

		if (currentInteractingController != null)
            currentInteractingController.SetInteraction(null);

        currentInteractingController = controller;

        currentInteractingController.SetInteraction(this);
    }

    public virtual void OnTriggerHold(VR_Controller_Custom controller) { }

    public virtual void OnTriggerRelease(VR_Controller_Custom controller)
    {
        currentInteractingController = null;
        controller.SetInteraction(null);
        
        rigidBody.velocity = currentReleaseVelocityMagnitude;
        rigidBody.angularVelocity = currentReleaseAngularVelocityMagnitude;
        Debug.Log("CurrentReleaseVelocity:" + rigidBody.velocity);
        Debug.Log(rigidBody.angularVelocity);
    }

    public virtual void OnGripPress(VR_Controller_Custom controller) { }

    public virtual void OnGripHold(VR_Controller_Custom controller) { }

    public virtual void OnGripRelease(VR_Controller_Custom controller) { }

    public virtual void OnInteracting(VR_Controller_Custom controller) {

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
