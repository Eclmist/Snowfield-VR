using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode]
public class DebugFlyCam : MonoBehaviour {

	float mainSpeed = 5;
	float shiftAdd = 5;
	float maxShift = 100;
	float camSensitivity = 0.25F;

	private Vector3 lastMouse;
	private float totalRun = 1;
 
	void Update()
	{
		if (Input.GetMouseButtonDown(1))
		{
			lastMouse = Input.mousePosition;
		}

		if (Input.GetMouseButton(1))
		{
			lastMouse = Input.mousePosition - lastMouse;
			lastMouse = new Vector3(-lastMouse.y * camSensitivity, lastMouse.x * camSensitivity, 0);
			lastMouse = new Vector3(transform.eulerAngles.x + lastMouse.x, transform.eulerAngles.y + lastMouse.y, 0);
			transform.eulerAngles = lastMouse;
			lastMouse = Input.mousePosition;
			//Mouse  camera angle done.  
		}


		//Keyboard commands
		var p = GetBaseInput();
		if (Input.GetKey(KeyCode.LeftShift))
		{
			totalRun += Time.deltaTime;
			p = p * totalRun * shiftAdd;
			p.x = Mathf.Clamp(p.x, -maxShift, maxShift);
			p.y = Mathf.Clamp(p.y, -maxShift, maxShift);
			p.z = Mathf.Clamp(p.z, -maxShift, maxShift);
		}
		else
		{
			totalRun = Mathf.Clamp(totalRun * 0.5F, 1, 1000);
			p = p * mainSpeed;
		}

		p = p * Time.deltaTime;

		transform.Translate(p);
	}

	private Vector3 GetBaseInput() {  //returns the basic values, if it's 0 than it's not active.

		Vector3 p_Velocity = Vector3.zero;

		if (Input.GetKey(KeyCode.W))
		{
			p_Velocity += new Vector3(0, 0, 1);
		}
		if (Input.GetKey (KeyCode.S)){
			p_Velocity += new Vector3(0, 0 , -1);
		}
		if (Input.GetKey (KeyCode.A)){
			p_Velocity += new Vector3(-1, 0 , 0);
		}
		if (Input.GetKey (KeyCode.D)){
			p_Velocity += new Vector3(1, 0 , 0);
		}

		return p_Velocity;
	}
}
