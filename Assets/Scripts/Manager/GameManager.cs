using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{

    public static GameManager Instance;

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
            //MessageManager.Instance.SendMail("Welcome", "You have been assigned the role of a merchant in this wonderful world. Here you can craft weapons and sell them to players. Have fun!\n\nFrom:\nXiaotian", null);
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
                AIManager.Instance.SpawnMerchant();
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
        //if (gameClock.Day == 1)
           // MessageManager.Instance.SendMail("Defend The Town","All of our guards got banned from hacking and we are really short-handed right now. If those monsters get to the heart of the town, somebody's gotta pay for damages. I'm just saying...\n\nFrom:\nMayor",null);

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
            //lose
        }
    }

    protected void OnDisable()
    {
        //SerializeManager.Save("GameClock", gameClock);
    }


}