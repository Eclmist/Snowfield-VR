using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class rotate : MonoBehaviour {

    public float speed;

    
	// Update is called once per frame
	void Update () {
        transform.Rotate(new Vector3(0, speed * Time.deltaTime, 0));
	}
    void OnGUI()
    {
        GUI.Label(new Rect(20, Screen.height - 70, 100, 100), "Camera speed");
        speed = -GUI.HorizontalSlider(new Rect(20, Screen.height - 50, 100, 20),-speed,0,50);
    }
}
