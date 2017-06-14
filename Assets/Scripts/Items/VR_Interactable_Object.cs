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
        rigidBody = GetComponent<Rigidbody>();
    }

    public virtual void OnControllerEnter(VR_Controller_Custom controller) { }

    public virtual void OnControllerStay(VR_Controller_Custom controller) { }

    public virtual void OnTriggerPress(VR_Controller_Custom controller)
    {
        if (currentInteractingController != null && currentInteractingController != controller)
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
    }

    public virtual void OnGripPress(VR_Controller_Custom controller) { }

    public virtual void OnGripHold(VR_Controller_Custom controller) { }

    public virtual void OnGripRelease(VR_Controller_Custom controller) { }

    public virtual void OnInteracting(VR_Controller_Custom controller) {

        currentReleaseVelocityMagnitude = Vector3.Lerp(currentReleaseVelocityMagnitude,controller.Velocity,Time.deltaTime);
        currentReleaseAngularVelocityMagnitude = Vector3.Lerp(currentReleaseAngularVelocityMagnitude, controller.AngularVelocity, Time.deltaTime);
    }
}
