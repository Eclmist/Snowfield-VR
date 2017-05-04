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


public class MainMenu : UI_Manager {
    private MainMenuState curState = MainMenuState.IDLE, lastState = MainMenuState.NEW_GAME;
    private bool inside = false;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
        if (inside)
        {
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
        else
        {

        }
    }

    private void TextChange(Text txt, string newTxt)
    {
        txt.text = newTxt;
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.tag == "GameController")
        {
            inside = true;
        }

        Debug.Log(inside);
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.tag == "GameController")
        {
            inside = false;
        }

        Debug.Log(inside);
    }
}
