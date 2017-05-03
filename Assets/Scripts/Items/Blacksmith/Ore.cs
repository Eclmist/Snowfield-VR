using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ore : GenericItem {

    
    [SerializeField]
    private TYPE type;

    public override void UpdatePosition()
    {
        throw new NotImplementedException();
    }

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}

    public TYPE Type
    {
        get { return this.type; }
    }


	

