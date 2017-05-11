using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public enum MainMenuState
{
    IDLE,
    CONTINUE,
    NEW_GAME,
    SETTINGS,
    CREDITS,
    QUIT
}


public class MainMenu : UI_Manager,IInteractable
{
    private MainMenuState curState = MainMenuState.IDLE, lastState = MainMenuState.NEW_GAME;
    private VR_Controller_Custom linkedController = null;

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
            StartInteraction(referenceCheck);
            curState = MainMenuState.CREDITS;
        }
        else if(referenceCheck.Device.GetTouchDown(SteamVR_Controller.ButtonMask.ApplicationMenu))
        {
            StartInteraction(referenceCheck);
            curState = MainMenuState.SETTINGS;        }
        
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
        VR_Controller_Custom controller = col.GetComponent<VR_Controller_Custom>();
        if (controller != null)
            StopInteraction(controller);
    }

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
        
        //test
        if (Input.GetMouseButtonDown(0))
        {
            curState = MainMenuState.CREDITS;
        }
        else if (Input.GetMouseButtonDown(1))
        {
            curState = MainMenuState.NEW_GAME;
        }
        //Main menu state machine
        if (curState == MainMenuState.IDLE)
        {

        }
        else if (curState == MainMenuState.NEW_GAME)
        {
            Text txt = GetComponentInChildren<Text>();
            TextChange(txt, "New Game");
            lastState = curState;
            curState = MainMenuState.IDLE;
        }
        else if (curState == MainMenuState.CREDITS)
        {
            Text txt = GetComponentInChildren<Text>();
            TextChange(txt, "Credits");
            lastState = curState;
            curState = MainMenuState.IDLE;
        }
        else if (curState == MainMenuState.CONTINUE)
        {
            Text txt = GetComponentInChildren<Text>();
            TextChange(txt, "Continue");
            lastState = curState;
            curState = MainMenuState.IDLE;
        }
        else if (curState == MainMenuState.QUIT)
        {
            Text txt = GetComponentInChildren<Text>();
            TextChange(txt, "Quit");
            lastState = curState;
            curState = MainMenuState.IDLE;
        }
        else if (curState == MainMenuState.SETTINGS)
        {
            Text txt = GetComponentInChildren<Text>();
            TextChange(txt, "Settings");
            lastState = curState;
            curState = MainMenuState.IDLE;
        }

    }

    private void TextChange(Text txt, string newTxt)
    {
        txt.text = newTxt;
    }

}
