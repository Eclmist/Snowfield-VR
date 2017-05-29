using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainMenu_Button : MonoBehaviour, IInteractable {

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
            Collider coll = GetComponent<Collider>();
            StartInteraction(referenceCheck);
            if (coll.gameObject.name == "MenuTitle")
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
            else if(coll.gameObject.name == "Left")
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
            else if(coll.gameObject.name == "Right")
            {
                Debug.Log("Right");
                if (MainMenu.Instance.GetNextState() != MainMenuState.IDLE)
                {
                    MainMenu.Instance.SetState(MainMenu.Instance.GetNextState());
                    Debug.Log("GoingRight");
                }
                else
                {

                }
            }
        }
        else if(referenceCheck.Device.GetTouchUp(SteamVR_Controller.ButtonMask.Trigger))
        {
            StopInteraction(referenceCheck);
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
        //Debug.Log("I am fucking exiting.");
        VR_Controller_Custom controller = col.GetComponent<VR_Controller_Custom>();
        if (controller != null)
            StopInteraction(controller);
    }

    // Use this for initialization
    void Start () {
		
	}
	
	// Update is called once per frame
	void  Update () {
		
	}
}
