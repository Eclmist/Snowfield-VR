using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour {

    public static GameManager Instance;

    #region ClockRegion
    [SerializeField] [Range(1, 10000)] private int secondsPerDay;

    private GameClock gameClock;

    public GameClock GameClock
    {
        get
        {
            return gameClock;
        }
    }

    [SerializeField]
    [Range(0.5f, 1)]
    private float timeOfNight = .6f;

    [SerializeField]
    [Range(0.01f, 0.1f)]
    private float preparationTime = .05f;
    #endregion

    #region RequestRegion

    [SerializeField] [Range(1, 100)] private int requestConstant;
    private float nextRequest = 0;
    public enum GameState
    {
        DAYMODE,
        NIGHTMODE
    }

    private GameState currentState;

    public GameState State
    {
        get
        {
            return currentState;
        }
    }
    #endregion
    protected void Awake()
    {
        if (!Instance)
        {
            Instance = this;
            gameClock = new GameClock(secondsPerDay);
        }
        else
        {
            Debug.Log("There should only be one instance of gamemanager running");
            Destroy(this);
        }
    }

    protected void Update()
    {
        RequestBoardUpdate();
        GameHandle();
    }

    private void GameHandle()
    {
        if (gameClock.TimeOfDay >= timeOfNight - preparationTime && currentState != GameState.NIGHTMODE)
        {
            StartCoroutine(PrepareForNight());
            Debug.Log("Night");
        }
        else if (gameClock.TimeOfDay < timeOfNight - preparationTime && currentState != GameState.DAYMODE)
        {
            currentState = GameState.DAYMODE;
            TownManager.Instance.ChangeWarpPoint(currentState);
            AIManager.Instance.SetAllAIState(ActorFSM.FSMState.IDLE);
            Debug.Log("Day");
        }
    }

    private IEnumerator PrepareForNight()
    {
        currentState = GameState.NIGHTMODE;
        AIManager.Instance.SetAllAIState(ActorFSM.FSMState.IDLE);
        TownManager.Instance.ChangeWarpPoint(currentState);
        yield return new WaitForSecondsRealtime(preparationTime);
        //WaveManager.Start

    }
    private void RequestBoardUpdate()
    {
		if (gameClock.SecondSinceStart > nextRequest && TownManager.Instance.CurrentTown != null)//update 
		{
			nextRequest = (nextRequest + (requestConstant / TownManager.Instance.CurrentTown.Population));
			if (!OrderBoard.Instance.IsMaxedOut)
				OrderManager.Instance.NewRequest();
		}
	}
	public void AddPlayerGold(int value)
    {
        if (!Player.Instance.AddGold(value))
        {
            //Lose
        }
    }

    



}
