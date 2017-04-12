using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour {

    public static GameManager Instance;

    #region ClockRegion
    [SerializeField] [Range(1, 10000)] private int secondsPerDay;

    private Clock gameClock;
    #endregion

    #region RequestRegion
    private float nextRequest = 0;
    private float requestConstant;
    #endregion
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
        RequestBoardUpdate();
    }

    private void RequestBoardUpdate()
    {
        if(gameClock.SecondSinceStart > nextRequest)//update
        {
            nextRequest = (nextRequest + requestConstant / TownManager.Instance.CurrentTown.Population) % 1;
            OrderManager.Instance.NewRequest();
        }
    }



}
