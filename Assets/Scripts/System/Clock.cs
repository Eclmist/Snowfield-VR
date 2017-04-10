using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Clock {

    private float startTimeInSecond;

    private float totalSecondsPerDay;

    
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


	public Clock(float secondsPerDay)
    {
        totalSecondsPerDay = secondsPerDay;
        startTimeInSecond = Time.time;
    }
}
