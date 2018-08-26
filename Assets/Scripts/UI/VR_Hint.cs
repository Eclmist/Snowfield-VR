using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VR_Hint : MonoBehaviour
{
	private float aspectRatio;
	private float fov;

	private float zDistance;


	protected void Start ()
	{
		zDistance = transform.localPosition.z;
		Debug.Log("zDistance");
	}
	
	protected void Update ()
	{
		
	}
}
