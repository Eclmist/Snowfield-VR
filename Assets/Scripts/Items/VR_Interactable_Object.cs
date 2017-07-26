using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class VR_Interactable_Object : VR_Interactable
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



    // Outline Rendering
    [SerializeField]
    private Color outlineColor = Color.yellow;

    private Renderer[] childRenderers;
    private List<Material> childMaterials = new List<Material>();



    protected override void Awake()
    {
        rigidBody = GetComponent<Rigidbody>();
        childRenderers = GetComponentsInChildren<Renderer>();
        rigidBody = GetComponent<Rigidbody>();

        gameObject.layer = 8;

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

    public override void OnControllerEnter(VR_Controller_Custom controller)
    {
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


        currentInteractingController = controller;
        controller.SetInteraction(this);
        lastPosition = transform.position;
        currentReleaseVelocity = Vector3.zero;
    }

    private Vector3 currentReleaseVelocity = Vector3.zero;
    public override void OnTriggerRelease(VR_Controller_Custom controller)
    {
        currentInteractingController = null;

        rigidBody.AddForce(currentReleaseVelocity, ForceMode.Impulse);
        rigidBody.angularVelocity = controller.AngularVelocity;
    }

    protected Vector3 lastPosition = Vector3.zero;

    public override void OnInteracting(VR_Controller_Custom controller)
    {
        Vector3 flatVelocity = (transform.position - lastPosition) / Time.deltaTime;
        lastPosition = transform.position;
        if (currentReleaseVelocity.magnitude > flatVelocity.magnitude)
            currentReleaseVelocity = Vector3.Lerp(currentReleaseVelocity, flatVelocity, Time.deltaTime);
        else
            currentReleaseVelocity = flatVelocity;

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

    protected virtual void OnTriggerEnter(Collider col)
    {
        if (currentInteractingController)
            currentInteractingController.OnTriggerEnter(col);
    }

    protected virtual void OnTriggerExit(Collider col)
    {
        if (currentInteractingController)
            currentInteractingController.OnTriggerExit(col);
    }
}
