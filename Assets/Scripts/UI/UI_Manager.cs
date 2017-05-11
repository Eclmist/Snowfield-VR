using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UI_Manager : MonoBehaviour {

    public static UI_Manager Instance;

    // Use this for initialization

    void Awake()
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

    
    public void Settings()
    {
        Debug.Log("Fuck");
    }

    public void Credits()
    {

    }


}
