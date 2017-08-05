using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class VR_Interactable_Object : VR_Interactable
{

    protected Rigidbody rigidBody;

    // Outline Rendering
    [SerializeField]
    private Color outlineColor = Color.yellow;

    private Renderer[] childRenderers;
    private List<Material> childMaterials = new List<Material>();

	protected GameObject targetPositionPoint;

	[SerializeField]
	protected AudioSource interactSound;

	private static readonly int interactableLayerIndex = 8;
	private CollisionDetectionMode defaultCollisionMode;

	public static bool playerKnowsHowToInteractWithObjects;

	public void HintObject()
	{
		if (!isHinting)
		{
			StartCoroutine(Hint());
		}
	}

	public void StopHint()
	{
		if (isHinting)
		{
			isHinting = false;
			SetOutline(false);
		}
	}


	private bool isHinting;
	private bool hintOverride;
	private IEnumerator Hint()
	{
		isHinting = true;

		while (isHinting)
		{
			if (!hintOverride)
			{
				Color col = Color.Lerp(outlineColor, Color.black, (Mathf.Sin(Time.time * 2.5F) + 1) / 2);
				SetOutlineColor(col);
			}
			yield return new WaitForFixedUpdate();
		}
	}

	protected override void Awake()
	{

        rigidBody = GetComponent<Rigidbody>();
        childRenderers = GetComponentsInChildren<Renderer>();
        rigidBody = GetComponent<Rigidbody>();

        gameObject.layer = interactableLayerIndex;   //Set layer to 8 (interactable)

		for (int i = 0; i < transform.childCount; i++)
		{
			(transform.GetChild(i)).gameObject.layer = interactableLayerIndex;
		}

        foreach (Renderer r in childRenderers)
        {
            if (r.GetComponent<ParticleSystem>())
                continue;

            foreach (Material m in r.materials)
            {
                if (m)
                {
                    m.SetOverrideTag("RenderType", "Outline");
	                m.SetColor("_OutlineColor", new Color(0,0,0,0));

					childMaterials.Add(m);
                }
            }
        }
    }


	protected override void Start()
	{
		base.Start();

		defaultCollisionMode = rigidBody.collisionDetectionMode;

		if (!interactSound)
		{
			interactSound = gameObject.AddComponent<AudioSource>();

			interactSound.spatialBlend = 1;
			interactSound.clip = Resources.Load<AudioClip>("click5");
		}

	}



	public virtual void OnControllerEnter(VR_Controller_Custom controller)
    {
		controller.Vibrate (triggerEnterVibration);
		OnControllerEnter ();
		SetOutline(true);
		hintOverride = true;
	}

	public virtual void OnControllerExit(VR_Controller_Custom controller)
    {
		controller.Vibrate(triggerExitVibration);

		OnControllerExit();	
        SetOutline(false);

		hintOverride = false;
    }

	public virtual void OnTriggerPress(VR_Controller_Custom controller)
    {
		if (interactSound)
			interactSound.Play();

		PlayerLearnedInteraction();

	    if (isHinting)
		    StopHint();
	    else
	    {
		    SetOutline(false);
		}

		OnTriggerPress();

		controller.Vibrate(triggerPressVibration);


		targetPositionPoint = new GameObject (name + "'s pivot point");
		targetPositionPoint.transform.position = transform.position;
		targetPositionPoint.transform.rotation = transform.rotation;

		targetPositionPoint.transform.parent = controller.transform;


        if (currentInteractingController)
            currentInteractingController.Release();

        currentInteractingController = controller;
        controller.SetInteraction(this);
        lastPosition = transform.position;
        currentReleaseVelocity = Vector3.zero;

		//Set CCD
		rigidBody.collisionDetectionMode = CollisionDetectionMode.ContinuousDynamic;
    }

    protected Vector3 currentReleaseVelocity = Vector3.zero;

	public virtual void OnTriggerRelease(VR_Controller_Custom controller)
    {
		OnTriggerRelease ();
	
        //rigidBody.angularVelocity = controller.AngularVelocity;


		targetPositionPoint.transform.parent = null;
		Destroy (targetPositionPoint);
		currentInteractingController = null;
		controller.SetInteraction (null);

		//Set CCD
		rigidBody.collisionDetectionMode = defaultCollisionMode;
	}

	public virtual void OnGripRelease(VR_Controller_Custom controller) {
		OnGripRelease ();
	}

	public virtual void OnGripPress(VR_Controller_Custom controller) {
		OnGripPress ();
	}

	public virtual void OnGripHold(VR_Controller_Custom controller) {
		OnGripHold ();
	}

	public virtual void OnTriggerHold(VR_Controller_Custom controller) {
		OnTriggerHold ();
	}

    protected Vector3 lastPosition = Vector3.zero;

	public virtual void OnUpdateInteraction(VR_Controller_Custom controller)
    {

        currentReleaseVelocity = (transform.position - lastPosition) / Time.deltaTime;
        lastPosition = transform.position;
        //if (currentReleaseVelocity.magnitude > flatVelocity.magnitude)
        //    currentReleaseVelocity = Vector3.Lerp(currentReleaseVelocity, flatVelocity, Time.deltaTime * 10);
        //else
        //    currentReleaseVelocity = flatVelocity;

    }

	public virtual void OnFixedUpdateInteraction(VR_Controller_Custom referenceCheck)
	{
		
	}

	protected virtual void PlayerLearnedInteraction()
	{
		playerKnowsHowToInteractWithObjects = true;
	}

    public void SetOutline(bool enabled)
    {
        SetOutlineColor(enabled ? outlineColor : new Color(0,0,0,0));
    }

    private void SetOutlineColor(Color color)
    {
        foreach (Material m in childMaterials)
        {
            m.SetColor("_OutlineColor", color);
        }
    }

	protected virtual void OnTriggerEnter(Collider col)
    {

		if (currentInteractingController) {
			currentInteractingController.OnTriggerEnter (col);
		}
    }



	protected virtual void OnTriggerExit(Collider col)
    {

        if (currentInteractingController)
            currentInteractingController.OnTriggerExit(col);
    }
}
