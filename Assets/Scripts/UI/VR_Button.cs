// using this script requirement:
// it is for button
// rigidbody
// collider

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class VR_Button : MonoBehaviour, IInteractable
{
    public UnityEvent onTriggerPress, onTriggerRelease, onApplicationMenuPress, onApplicationMenuRelease;

    protected VR_Controller_Custom linkedController = null;   

    public VR_Controller_Custom LinkedController
    {
        get
        {
            return linkedController;
        }
        set
        {
            linkedController = value;
        }
    }

    public virtual void Interact(VR_Controller_Custom referenceCheck)
    {

        if (referenceCheck.Device.GetTouchDown(SteamVR_Controller.ButtonMask.Trigger))
        {
            Debug.Log("Triggered");
            StartInteraction(referenceCheck);
            onTriggerPress.Invoke();
        }
        else if (referenceCheck.Device.GetTouchUp(SteamVR_Controller.ButtonMask.Trigger))
        {
            Debug.Log("Untriggered");
            StopInteraction(referenceCheck);
            onTriggerRelease.Invoke();
        }
        else if(referenceCheck.Device.GetTouchDown(SteamVR_Controller.ButtonMask.ApplicationMenu))
        {
            Debug.Log("App");
            StartInteraction(referenceCheck);
            onApplicationMenuPress.Invoke();
        }
        else if (referenceCheck.Device.GetTouchUp(SteamVR_Controller.ButtonMask.ApplicationMenu))
        {
            Debug.Log("Unapp");
            StopInteraction(referenceCheck);
            onApplicationMenuRelease.Invoke();
        }
    }

    public virtual void StartInteraction(VR_Controller_Custom referenceCheck)
    {
        if (linkedController != null && linkedController != referenceCheck)
            linkedController.SetInteraction(null);

        linkedController = referenceCheck;
    }

    public void StopInteraction(VR_Controller_Custom referenceCheck)
    {
        if (linkedController == referenceCheck)
        {
            linkedController = null;
            referenceCheck.SetInteraction(null);
        }
    }

    protected virtual void OnTriggerExit(Collider col)
    {
        Debug.Log("Exitting Trigger");
        VR_Controller_Custom controller = col.GetComponent<VR_Controller_Custom>();
        if (controller != null)
            StopInteraction(controller);
    }

    public void MM_CenterButton()
    {
        Debug.Log("MainMenu");
        if (MainMenu.Instance.GetLastState() != MainMenuState.IDLE)
        {
            Debug.Log("SceneChange");
            switch (MainMenu.Instance.GetLastState())
            {
                case MainMenuState.CONTINUE:
                    Debug.Log("Continue");
                    break;
                case MainMenuState.NEW_GAME:
                    Debug.Log("NewGame");
                    break;
                case MainMenuState.SETTINGS:
                    Debug.Log("Settings");
                    break;
                case MainMenuState.CREDITS:
                    Debug.Log("Credits");
                    break;
                case MainMenuState.QUIT:
                    Debug.Log("Quit");
                    break;
                default:
                    Debug.Log("Nothing");
                    break;
            }
        }
        else
        {

        }
    }
    
    public void MM_LeftButton()
    {
        Debug.Log("Left");
        if (MainMenu.Instance.GetLLState() != MainMenuState.IDLE)
        {
            MainMenu.Instance.SetState(MainMenu.Instance.GetLLState());
        }
        else
        {

        }
    }

    public void MM_RightButton()
    {
        Debug.Log("Right");
        if (MainMenu.Instance.GetNextState() != MainMenuState.IDLE)
        {
            MainMenu.Instance.SetState(MainMenu.Instance.GetNextState()); Debug.Log("Damn");
        }
        else
        {

        }
    }
    
}
