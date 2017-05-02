using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour {

    public static GameManager Instance;

    #region ClockRegion
    [SerializeField] [Range(1, 10000)] private int secondsPerDay;

    private GameClock gameClock;
    #endregion

    #region RequestRegion

    [SerializeField] [Range(1, 100)] private int requestConstant;
    private float nextRequest = 0;

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
    }

    private void RequestBoardUpdate()
    {
        if (gameClock.SecondSinceStart > nextRequest)//update
        {
            nextRequest = (nextRequest + (requestConstant / TownManager.Instance.CurrentTown.Population));
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
