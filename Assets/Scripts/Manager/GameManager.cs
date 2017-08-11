using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{

    public static GameManager Instance;
	public static bool nightApproaching= false;
	public static bool firstGame = false;

    #region ClockRegion
    [SerializeField]
    [Range(1, 10000)]
    public float secondsPerDay;

    private GameClock gameClock;

    protected int currentTax = 0;

    public GameClock GameClock
    {
        get
        {
            return gameClock;
        }
    }



    [SerializeField]
    [Range(0.5f, 1)]
    private float nightTime = .25f, dayTime = .75f;
    [SerializeField]
    [Range(0, 1)]
    private float startTime;

    #endregion

    public int Tax
    {
        get
        {
            return currentTax;
        }
    }

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

    protected void Start()
    {
        GameClock checkClock = (GameClock)SerializeManager.Load("GameClock");
        if (checkClock != null)
            gameClock = checkClock;
        else
        {
            gameClock = new GameClock(secondsPerDay, startTime);
			firstGame = true;
        }

    }


    protected void Awake()
    {
        if (!Instance)
        {
            Instance = this;

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
        if (Input.GetKeyDown(KeyCode.R))
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        }

    }

    private void GameHandle()
    {
        if (currentState == GameState.DAYMODE)
        {
            if (gameClock.TimeOfDay > nightTime || gameClock.TimeOfDay < dayTime)
            {
                AddPlayerGold(-currentTax);
                currentTax = 0;
                PrepareForNight();
            }
        }
        if (currentState == GameState.NIGHTMODE)
        {
            if (gameClock.TimeOfDay < nightTime && gameClock.TimeOfDay > dayTime)
            {
                if (!WaveManager.Instance.HasMonster)
                    AIManager.Instance.InstantiateMerchant();
                currentState = GameState.DAYMODE;

				if(currentTax != 0)
					MessageManager.Instance.SendMail("INVOICE:" + gameClock.TimeOfDay + ":D", "The town has suffered a total of " + currentTax + " in damages. Please acquire the amount by the start of the following night\n\nFrom:\nSecretary of State Van Allen", null);
            }
        }

    }

    private void PrepareForNight()
    {
        currentState = GameState.NIGHTMODE;
        WaveManager.Instance.SpawnWave(gameClock.Day);
		nightApproaching = true;

    }

    public void AddTax(int value)
    {
        currentTax += value;
    }
    //   private void RequestBoardUpdate()
    //   {
    //  if (gameClock.SecondSinceStart > nextRequest && TownManager.Instance.CurrentTown != null)//update
    //  {
    //      nextRequest = (nextRequest + (requestConstant / TownManager.Instance.CurrentTown.Population));
    //      if (!OrderBoard.Instance.IsMaxedOut)
    //          OrderManager.Instance.NewRequest();
    //  }
    //}
    public void AddPlayerGold(int value)
    {
        Player.Instance.AddGold(value);
        if (Player.Instance.Gold < 0)
        {
            Player.Instance.Die();
        }
    }

    protected void OnDisable()
    {
        //SerializeManager.Save("GameClock", gameClock);
    }


}