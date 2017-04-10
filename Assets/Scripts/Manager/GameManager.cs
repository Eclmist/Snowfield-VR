using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour {

    public static GameManager Instance;

    #region ClockRegion
    private Clock gameClock;
    [SerializeField]
    private int secondsPerDay;
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

        if (secondsPerDay > 0)
        {
            gameClock = new Clock(secondsPerDay);
        }
        else
        {
            Debug.Log("Please set a value greater than 0 for secondsPerDay");
        }

    }

    protected void Update()
    {
        RequestBoardUpdate();
    }

    private void RequestBoardUpdate()
    {
        if(gameClock.TimeOfDay > nextRequest)
        {
            nextRequest = (nextRequest + requestConstant / TownManager.Instance.CurrentTown.Population) % 1;
            RequestManager.Instance.NewRequest();
        }
    }



}
