using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameClock {

    private float startTimeInSecond;

    private float totalSecondsPerDay;

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
            return Time.time - startTimeInSecond;
        }
    }

    public int Day
    {
        get
        {
            float currentTimeInSecond = Time.time - startTimeInSecond;
            return (int)(currentTimeInSecond / totalSecondsPerDay) + 1;
        }
    }

    public float TimeOfDay
    {
        get
        {
            float currentTimeInSecond = Time.time - startTimeInSecond;
            return (currentTimeInSecond % totalSecondsPerDay) / totalSecondsPerDay;
        }
    }


	public GameClock(float secondsPerDay)
    {
        totalSecondsPerDay = secondsPerDay;
        startTimeInSecond = Time.time;
    }
}
