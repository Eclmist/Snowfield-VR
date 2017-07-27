using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour {

    public static GameManager Instance;

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
    private float nightTime = .25f , dayTime = .75f;
    [SerializeField]
    [Range(0, 1)]
    private float startTime;

    #endregion

    #region RequestRegion

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
            gameClock = new GameClock(secondsPerDay,startTime);
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
        if(currentState == GameState.DAYMODE)
        {
            if (gameClock.TimeOfDay > nightTime || gameClock.TimeOfDay < dayTime)
                PrepareForNight();
        }
        if (currentState == GameState.NIGHTMODE)
        {
            if(gameClock.TimeOfDay < nightTime && gameClock.TimeOfDay > dayTime)
            currentState = GameState.DAYMODE;
        }

        //Debug.Log(currentState);
    }

    private void PrepareForNight()
    {
        currentState = GameState.NIGHTMODE;

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
       
    }

    public void AddToPlayerInventory(ItemData data)
    {
        StoragePanel.Instance.StoreInAvailableSlot(data,1);
    }



}
