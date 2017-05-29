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
        if(settings == null)
            settings = MainMenuItemManager.Instance.GetSpecificChild("BGM"); 
        
        //Main menu state machine
        if (curState == MainMenuState.IDLE)
        {

        }
        else if (curState == MainMenuState.CONTINUE)
        {
            SettingsSet();
            Text txt = GetComponentInChildren<Text>();
            TextChange(txt, "Continue");
            lastState = curState;
            curState = MainMenuState.IDLE;
        }
        else if (curState == MainMenuState.NEW_GAME)
        {
            SettingsSet();
            Text txt = GetComponentInChildren<Text>();
            TextChange(txt, "New Game");
            lastLastState = MainMenuState.IDLE;
            lastState = curState;
            nextState = MainMenuState.SETTINGS;
            curState = MainMenuState.IDLE;
        }
        else if (curState == MainMenuState.SETTINGS)
        {
            SettingsSet();
            MainMenuItemManager.Instance.GetChild();
            Text txt = GetComponentInChildren<Text>();
            TextChange(txt, "Settings");
            lastLastState = MainMenuState.NEW_GAME;
            lastState = curState;
            nextState = MainMenuState.CREDITS;
            curState = MainMenuState.IDLE;
        }
        else if (curState == MainMenuState.CREDITS)
        {
            SettingsSet();
            Text txt = GetComponentInChildren<Text>();
            TextChange(txt, "Credits");
            lastLastState = MainMenuState.SETTINGS;
            lastState = curState;
            nextState = MainMenuState.QUIT;
            curState = MainMenuState.IDLE;
            
        }
        else if (curState == MainMenuState.QUIT)
        {
            SettingsSet();
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
   

    private void SettingsSet()
    {
        if (settings != null && curState == MainMenuState.SETTINGS)
        {
            settings.SetActive(true);
        }
        else if (settings != null && curState != MainMenuState.SETTINGS)
        {
            settings.SetActive(false);
        }

    }
}
