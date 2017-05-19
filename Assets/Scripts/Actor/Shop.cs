using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shop : MonoBehaviour
{

    private Vector3 location;

    public Vector3 Location
    {
        get
        {
            return location;
        }
        set
        {
            location = value;
        }
    }

    [SerializeField]
    private Actor shopOwner;

    public Actor Owner
    {
        get
        {
            return Owner;
        }
    }

    private void Awake()
    {
        Transform locationObject = transform.Find("Location");
        if (locationObject)
        {
            location = locationObject.position;
        }
        else
        {
            Debug.Log("Location for " + gameObject.name + ": store has not been set!");
        }
    }

    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }
}
