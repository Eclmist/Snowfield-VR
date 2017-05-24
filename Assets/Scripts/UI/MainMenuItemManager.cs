using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainMenuItemManager : MonoBehaviour {
    private Transform[] child, temp;
    public static MainMenuItemManager Instance;

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
        child = new Transform[3];
        int i = 0;
        temp = GetComponentsInChildren<Transform>(true);
        
        foreach(Transform t in temp)
        {
            if (t.tag.Equals("MainMenuItem"))
            {
                child[i] = t;

                i++;
            }
        }

    }
	
	// Update is called once per frame
	void Update ()
    {

	}

    public Transform[] GetChild()
    {
        return child;
    }

    public GameObject GetSpecificChild(string n)
    {
        Transform temp = null;
        foreach (Transform t in child)
        {
            if(t.name == n)
            {
                temp = t;
            }
        }
        return temp.gameObject;
    }
}
