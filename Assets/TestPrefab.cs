using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class TestPrefab : MonoBehaviour {

    [SerializeField]
    protected GameObject obj;
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
        if (Input.GetKeyDown(KeyCode.U))
        {
            Debug.Log(PrefabUtility.GetPrefabType(obj).ToString());
        }
	}
}
