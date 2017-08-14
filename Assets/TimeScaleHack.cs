using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TimeScaleHack : MonoBehaviour {

    [SerializeField]
    protected bool startHacks;

    [SerializeField]
    protected float scaleValue;
	
	// Update is called once per frame
	void Update () {
        if (startHacks)
            Time.timeScale = scaleValue;
        else
            Time.timeScale = 1;
	}
}
