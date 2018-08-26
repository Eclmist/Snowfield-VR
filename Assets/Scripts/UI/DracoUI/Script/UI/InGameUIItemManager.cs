    using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InGameUIItemManager : MonoBehaviour {
    public static InGameUIItemManager Instance;
    private Transform[] options, temp;

    private void Awake()
    {
        if (!Instance)
        {
            Instance = this;
        }
        else
        {
            Debug.Log("Overlord.");
            Destroy(this);
        }
    }

    // Use this for initialization
    void Start ()
    {
        options = new Transform[3];
        int i = 0;
        temp = GetComponentsInChildren<Transform>(true);

        foreach (Transform t in temp)
        {
            if (t.tag.Equals("InGameMenuItem"))
            {
                options[i] = t;
                Debug.Log(options[i]);
                i++; 
            }
        }

    }
	
	// Update is called once per frame
	void Update () {
		
	}

    
}
