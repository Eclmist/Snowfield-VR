using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[System.Serializable]
public class GameClock {

    private float startTimeInSecond;

    private float totalSecondsPerDay;

    private float clockOffset;

    public float SecondsPerDay
    {
        get
        {
            return totalSecondsPerDay;
        }
    }
    public float SecondSinceStart
    {
        get
        {
            return Time.time - startTimeInSecond + clockOffset;
        }
    }

	public bool IsDay
	{
		get { return TimeOfDay > 0.2F && TimeOfDay < 0.65F; }
	}


	public int Day
    {
        get
        {
            float currentTimeInSecond = SecondSinceStart;
            return (int)(currentTimeInSecond / totalSecondsPerDay) + 1;
        }
    }

	// returns 0-1 float, 0 being midnight and 0.5 being noon
    public float TimeOfDay
    {
        get
        {
            float currentTimeInSecond = SecondSinceStart;
            return (currentTimeInSecond % totalSecondsPerDay) / totalSecondsPerDay;
        }
    }


	public GameClock(float secondsPerDay,float _startTime)
    {
        totalSecondsPerDay = secondsPerDay;
        startTimeInSecond = Time.time;
        clockOffset = _startTime * totalSecondsPerDay;
    }
}
