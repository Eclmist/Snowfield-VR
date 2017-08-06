using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestCube : MonoBehaviour {

    public Vector3 offset;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
        if(Input.GetKeyDown(KeyCode.Alpha0))
        {
            TextSpawnerManager.Instance.SpawnSoundEffect("\"ニャ!\"", Color.white,transform,offset,7);
        }
	}
}
