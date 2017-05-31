using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public enum InGameState
{
    IDLE,
    INGAME,
    PAUSE
}

public enum InGamePause
{
    IDLE,
    CHARACTER,
    SETTINGS,
    EXIT
}




public class InGameUI : MonoBehaviour
{
    
    public static InGameUI Instance;
    [SerializeField] private InGameState curState = InGameState.INGAME, lastState = InGameState.IDLE;
    [SerializeField] private InGamePause curSelection = InGamePause.IDLE, lastSelection = InGamePause.IDLE;
    
    void Awake()
    {
        if (!Instance)
        {
            Instance = this;
        }
        else
        {
            Debug.Log("Overlord InGame.");
            Destroy(this);
        }
        
    }

    // Use this for initialization
    void Start () {
        
        
	}
	
	// Update is called once per frame
	void Update ()
    {
        switch (curState)
        {
            case InGameState.INGAME:
                lastState = curState;
                curState = InGameState.IDLE;
                curSelection = InGamePause.IDLE;
                lastSelection = InGamePause.IDLE;
                break;
            case InGameState.PAUSE:
                lastState = curState;
                curState = InGameState.IDLE;
                break;
            default:
                break;
        }

        switch (curSelection)
        {
            case InGamePause.CHARACTER:
                lastSelection = curSelection;
                curSelection = InGamePause.IDLE;
                break;
            case InGamePause.SETTINGS:
                lastSelection = curSelection;
                curSelection = InGamePause.IDLE;
                break;
            case InGamePause.EXIT:
                lastSelection = curSelection;
                curSelection = InGamePause.IDLE;
                break;
            default:
                break;
        }


    }

    public InGameState GetLastState()
    {
        return lastState;
    }
    
    public void SetGameState(InGameState state)
    {
        curState = state;
    }

    public void SetGameMenuState(InGamePause state)
    {
        curSelection = state;
    }

    
}
