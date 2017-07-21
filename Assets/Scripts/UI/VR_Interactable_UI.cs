/*
 * Base class for world space interactable VR UI.
 */

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class VR_Interactable_UI : MonoBehaviour
{
    [SerializeField] public bool interactable = true;



    [Header("Vibrations")]
    [SerializeField]
    [Range(0, 10)]
    protected float triggerEnterVibration = 0.8F;
    [SerializeField] [Range(0, 10)] protected float triggerExitVibration = 0.3F;
    [SerializeField] [Range(0, 10)] protected float triggerPressVibration = 0;

    protected virtual void OnControllerEnter()
    {
        currentInteractingController.Vibrate(triggerEnterVibration);
        if (currentInteractingController)
        {
            currentInteractingController.UI = this;
            Debug.Log("interacring with an order");
        }

    }
    protected virtual void OnControllerStay() { }
    protected virtual void OnControllerExit()
    {
        if (currentInteractingController.UI == this)
            currentInteractingController.UI = null;

        currentInteractingController.Vibrate(triggerExitVibration);
        currentInteractingController = null;

    }

    protected virtual void OnTriggerPress()
    {
        if (currentInteractingController)
            currentInteractingController.Vibrate(triggerPressVibration);
    }
    protected virtual void OnTriggerHold() { }
    protected virtual void OnTriggerRelease() { }

    protected virtual void OnApplicationMenuPress()
    {
        currentInteractingController.Vibrate(triggerPressVibration);
        Debug.Log("App");
    }

    protected virtual void OnInteractableChange() { }

    protected VR_Controller_Custom currentInteractingController;

    protected void OnTriggerEnter(Collider other)
    {
        if (interactable)
        {
            VR_Controller_Custom vrController = other.GetComponentInParent<VR_Controller_Custom>();

            if (vrController && currentInteractingController == null)
            {
                currentInteractingController = vrController;
                OnControllerEnter();
            }
        }
    }

    protected void OnTriggerStay(Collider other)
    {
        if (interactable)
        {

            VR_Controller_Custom vrController = other.GetComponentInParent<VR_Controller_Custom>();

            if (vrController && currentInteractingController == vrController)
            {
                OnControllerStay();



            }
        }
    }

    protected void OnTriggerExit(Collider other)
    {
        if (interactable)
        {

            VR_Controller_Custom vrController = other.GetComponentInParent<VR_Controller_Custom>();

            if (vrController && currentInteractingController == vrController)
            {
                OnControllerExit();
            }
        }
    }

    private bool lastInteractable;

    protected virtual void Update()
    {
        if (lastInteractable != interactable)
        {
            lastInteractable = interactable;
            OnInteractableChange();
        }
        if (currentInteractingController)
        {
            if (currentInteractingController.Device.GetPressDown(SteamVR_Controller.ButtonMask.Trigger))
                OnTriggerPress();
            else if (currentInteractingController.Device.GetPress(SteamVR_Controller.ButtonMask.Trigger))
                OnTriggerHold();
            else if (currentInteractingController.Device.GetPressUp(SteamVR_Controller.ButtonMask.Trigger))
                OnTriggerRelease();
            else if (currentInteractingController.Device.GetPressDown(SteamVR_Controller.ButtonMask.ApplicationMenu))
                OnApplicationMenuPress();
        }
       

    }
}


