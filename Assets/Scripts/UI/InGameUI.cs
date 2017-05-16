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

public class InGameUI : MonoBehaviour
{
    public static InGameUI Instance;
    public InGameState curState = InGameState.INGAME, lastState = InGameState.IDLE;
    private Transform[] go;
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
        go = GetComponentsInChildren<Transform>(true);
        
	}
	
	// Update is called once per frame
	void Update ()
    {
        switch (curState)
        {
            case InGameState.INGAME:
                CheckObject("InGame", "MainMenu");
                lastState = curState;
                curState = InGameState.IDLE;
                break;
            case InGameState.PAUSE:
                CheckObject("MainMenu", "InGame");
                lastState = curState;
                curState = InGameState.IDLE;
                break;
            default:
                break;
        }


    }

    public InGameState GetLastState()
    {
        return lastState;
    }

    private void CheckObject(string name1, string name2)
    {
        if (go != null)
        {
            foreach (Transform t in go)
            {
                if (t.name == name1)
                {
                    t.gameObject.SetActive(true);
                }
                if (t.name == name2)
                {
                    t.gameObject.SetActive(false);
                }
            }
        }
    }
}
