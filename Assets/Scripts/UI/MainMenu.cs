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


public class MainMenu : UI_Manager,IInteractable {

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

    public void Interact(VR_Controller_Custom referenceCheck)
    {
        if (linkedController != null && linkedController != referenceCheck)
            linkedController.SetInteraction(null);

        
        curState = MainMenuState.CREDITS;
    }

    public void UpdatePosition()
    {
        //do w/e
    }

    public void StopInteraction(VR_Controller_Custom referenceCheck)
    {
        if (linkedController == referenceCheck)
        {
            linkedController = null;
            referenceCheck.SetInteraction(null);
        }
    }

    private MainMenuState curState = MainMenuState.IDLE, lastState = MainMenuState.NEW_GAME;
    private bool inside = false;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
        if (linkedController != null)
            UpdatePosition();
        
            if (Input.GetMouseButtonDown(0))
            {
                curState = MainMenuState.CREDITS;
            }
            else if (Input.GetMouseButtonDown(1))
            {
                curState = MainMenuState.NEW_GAME;
            }

            if (curState == MainMenuState.IDLE)
            {

            }
            else if (curState == MainMenuState.NEW_GAME)
            {
                Text txt = GetComponentInChildren<Text>();
                TextChange(txt, "New Game");
                curState = MainMenuState.IDLE;
                lastState = MainMenuState.NEW_GAME;
            }
            else if (curState == MainMenuState.CREDITS)
            {
                Text txt = GetComponentInChildren<Text>();
                TextChange(txt, "Credits");
                curState = MainMenuState.IDLE;
                lastState = MainMenuState.CREDITS;
            }
        
    }

    private void TextChange(Text txt, string newTxt)
    {
        txt.text = newTxt;
    }

    //private void OnTriggerEnter(Collider other)
    //{
    //    if(other.tag == "GameController")
    //    {
    //        inside = true;
    //    }

    //    Debug.Log(inside);
    //}

    //private void OnTriggerExit(Collider other)
    //{
    //    if (other.tag == "GameController")
    //    {
    //        inside = false;
    //    }

    //    Debug.Log(inside);
    //}
}
