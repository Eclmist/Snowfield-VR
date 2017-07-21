using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{

    public static GameManager Instance;
    public AI ai;

    #region ClockRegion
    [SerializeField] [Range(1, 10000)] public float secondsPerDay;

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
    private float nightTime = .75f, dayTime = .25f;
    [SerializeField]
    [Range(0, 1)]
    private float startTime;
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
            gameClock = new GameClock(secondsPerDay, startTime);
        }
        else
        {
            Debug.Log("There should only be one instance of gamemanager running");
            Destroy(this);
        }
    }

	protected void Update()
    {
        GameHandle();
    }

    private void GameHandle()
    {
        if (gameClock.TimeOfDay > nightTime && currentState != GameState.NIGHTMODE)
        {
            PrepareForNight();

            Debug.Log("Night");
        }
        else if (dayTime > gameClock.TimeOfDay && gameClock.TimeOfDay > dayTime && currentState != GameState.DAYMODE)
        {
            currentState = GameState.DAYMODE;
            Debug.Log("Day");
        }
    }

    private void PrepareForNight()
    {
        currentState = GameState.NIGHTMODE;
        AIManager.Instance.SetAllAIState(ActorFSM.FSMState.IDLE);
        WaveManager.Instance.SpawnWave(gameClock.Day);

    }
    //   private void RequestBoardUpdate()
    //   {
    //	if (gameClock.SecondSinceStart > nextRequest && TownManager.Instance.CurrentTown != null)//update 
    //	{
    //		nextRequest = (nextRequest + (requestConstant / TownManager.Instance.CurrentTown.Population));
    //		if (!OrderBoard.Instance.IsMaxedOut)
    //			OrderManager.Instance.NewRequest();
    //	}
    //}
    public void AddPlayerGold(int value)
    {
        Player.Instance.AddGold(value);
        //set lose
    }

    public void AddToPlayerInventory(ItemData data)
    {
        //YP ADD PLEASE
    }



}
