using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shop : MonoBehaviour
{

    [SerializeField][Tooltip("Spot where AIs go to")]
    private Transform location;

    public Transform Location
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
            return shopOwner;
        }
    }

    private void Awake()
    {
        
        if (!location)
        {
            Debug.Log("Location for " + gameObject.name + ": store has not been set! Hence default value will be assigned");
            location = transform;
        }
    }

}
