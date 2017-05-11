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


public class MainMenu : MonoBehaviour, IInteractable
{
    public static MainMenu Instance;
    protected MainMenuState curState = MainMenuState.IDLE, 
        lastState = MainMenuState.NEW_GAME, 
        lastLastState = MainMenuState.IDLE, 
        nextState = MainMenuState.SETTINGS;
    protected VR_Controller_Custom linkedController = null;
    void Awake()
    {
        if (!Instance)
        {
            Instance = this;
        }
        else
        {
            Debug.Log("Overlord.");
            Destroy(this);
        }
    }

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
            if (coll.gameObject.name == "MainMenu")
            {
                Debug.Log("MainMenu");
            }
            else if(coll.gameObject.name == "Arrow")
            {
                Debug.Log("Left");
                if (lastLastState != MainMenuState.IDLE)
                {
                    curState = lastLastState;
                }
                else
                {

                }
            }
            if(coll.gameObject.name == "Arrow (1)")
            {
                Debug.Log("Right");
                if (nextState != MainMenuState.IDLE)
                {
                    curState = nextState;Debug.Log("Damn");
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
        Debug.Log("I am fucking exiting.");
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
        
        //Main menu state machine
        if (curState == MainMenuState.IDLE)
        {

        }
        else if (curState == MainMenuState.CONTINUE)
        {
            Text txt = GetComponentInChildren<Text>();
            TextChange(txt, "Continue");
            lastState = curState;
            curState = MainMenuState.IDLE;
        }
        else if (curState == MainMenuState.NEW_GAME)
        {
            Text txt = GetComponentInChildren<Text>();
            TextChange(txt, "New Game");
            lastLastState = MainMenuState.IDLE;
            lastState = curState;
            nextState = MainMenuState.SETTINGS;
            curState = MainMenuState.IDLE;
        }
        else if (curState == MainMenuState.SETTINGS)
        {
            Text txt = GetComponentInChildren<Text>();
            TextChange(txt, "Settings");
            lastLastState = MainMenuState.NEW_GAME;
            lastState = curState;
            nextState = MainMenuState.CREDITS;
            curState = MainMenuState.IDLE;
        }
        else if (curState == MainMenuState.CREDITS)
        {
            Text txt = GetComponentInChildren<Text>();
            TextChange(txt, "Credits");
            lastLastState = MainMenuState.SETTINGS;
            lastState = curState;
            nextState = MainMenuState.QUIT;
            curState = MainMenuState.IDLE;
        }
        else if (curState == MainMenuState.QUIT)
        {
            Text txt = GetComponentInChildren<Text>();
            TextChange(txt, "Quit");
            lastLastState = MainMenuState.CREDITS;
            lastState = curState;
            nextState = MainMenuState.IDLE;
            curState = MainMenuState.IDLE;
        }

    }

    private void TextChange(Text txt, string newTxt)
    {
        txt.text = newTxt;
    }

    public void SetState(MainMenuState state)
    {
        curState = state;
    }

    public MainMenuState GetLLState()
    {
        return lastLastState;
    }

    public MainMenuState GetLastState()
    {
        return lastState;
    }

    public MainMenuState GetNextState()
    {
        return nextState;
    }
}
