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


public class MainMenu : MonoBehaviour
{
    public static MainMenu Instance;
    protected MainMenuState curState = MainMenuState.NEW_GAME, 
        lastState = MainMenuState.IDLE, 
        lastLastState = MainMenuState.IDLE, 
        nextState = MainMenuState.SETTINGS;

    private GameObject settings;

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

   
	// Use this for initialization
	void Start ()
    {
        
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
            MainMenuManager.Instance.ActiveDeactiveCredits(false);
            MainMenuManager.Instance.ActiveDeactiveSettings(false);
            MainMenuManager.Instance.TextChange("Continue");
            lastState = curState;
            curState = MainMenuState.IDLE;
        }
        else if (curState == MainMenuState.NEW_GAME)
        {
            MainMenuManager.Instance.ActiveDeactiveCredits(false);
            MainMenuManager.Instance.ActiveDeactiveSettings(false);
            MainMenuManager.Instance.TextChange("New Game");
            lastLastState = MainMenuState.IDLE;
            lastState = curState;
            nextState = MainMenuState.SETTINGS;
            curState = MainMenuState.IDLE;
        }
        else if (curState == MainMenuState.SETTINGS)
        {
            MainMenuManager.Instance.ActiveDeactiveCredits(false);
            MainMenuManager.Instance.ActiveDeactiveSettings(true);
            MainMenuManager.Instance.TextChange("Settings");
            lastLastState = MainMenuState.NEW_GAME;
            lastState = curState;
            nextState = MainMenuState.CREDITS;
            curState = MainMenuState.IDLE;
        }
        else if (curState == MainMenuState.CREDITS)
        {
            MainMenuManager.Instance.ActiveDeactiveCredits(true);
            MainMenuManager.Instance.ActiveDeactiveSettings(false);
            MainMenuManager.Instance.TextChange("Credits");
            lastLastState = MainMenuState.SETTINGS;
            lastState = curState;
            nextState = MainMenuState.QUIT;
            curState = MainMenuState.IDLE;
            
        }
        else if (curState == MainMenuState.QUIT)
        {
            MainMenuManager.Instance.ActiveDeactiveCredits(false);
            MainMenuManager.Instance.ActiveDeactiveSettings(false);
            Text txt = GetComponentInChildren<Text>();
            MainMenuManager.Instance.TextChange( "Quit");
            lastLastState = MainMenuState.CREDITS;
            lastState = curState;
            nextState = MainMenuState.IDLE;
            curState = MainMenuState.IDLE;
        }

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
