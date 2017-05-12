using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Settings : MonoBehaviour {

    Image img;

    public static Settings Instance;

    void Awake()
    {
        if (!Instance)
        {
            Instance = this;
        }
        else
        {
            Debug.Log("Overlord Settings.");
            Destroy(this);
        }
    }

    // Use this for initialization
    void Start ()
    {
        img = GetComponent<Image>();
	}
	
	// Update is called once per frame
	void Update ()
    {
		
	}

    public void SetFill(int fill)
    {
        img.fillAmount = fill / 100.0f;
    }
}
