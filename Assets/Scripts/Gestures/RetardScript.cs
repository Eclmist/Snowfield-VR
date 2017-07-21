using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Edwon.VR.Gesture;

public class RetardScript : MonoBehaviour {

    public static RetardScript Instance;

    public List<Gesture> listofGestures;

    void Awake()
    {
        Instance = this;
    }

    // Use this for initialization
    void Start ()
    {
        
	}
	
	// Update is called once per frame
	void Update ()
    {
		if(Input.GetKeyDown(KeyCode.X))
        {
            foreach (Gesture g in listofGestures)
            {
                Debug.Log(g.name);
            }
        }
	}
}
