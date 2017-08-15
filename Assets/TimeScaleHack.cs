using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TimeScaleHack : MonoBehaviour {

    [SerializeField]
    protected bool startHacks;

    [SerializeField]
    protected float scaleValue;
	
	public static TimeScaleHack Instance;

	public void Awake()
	{
		Instance = this;
	}
	// Update is called once per frame
	void Update () {
        if (startHacks)
            Time.timeScale = scaleValue;
        else
            Time.timeScale = 1;
	}

	public void StartHack(float val)
	{
		startHacks = !startHacks;
		Time.timeScale = val;
	}
}
