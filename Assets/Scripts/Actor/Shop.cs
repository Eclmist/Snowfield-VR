using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shop : MonoBehaviour
{

    [SerializeField]
    [Tooltip("Spot where AIs go to")]
    private Transform location;
    private List<Vector3> points = new List<Vector3>();
    [SerializeField]
    private LayerMask pointsIgnoreLayer;
    [SerializeField]
    private List<Transform> WaypointsBeforeOwner;
    private bool isOccupied = false;

    public Vector3 Location
    {
        get
        {
            return location.position;
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

    public Transform GetPoint(int currentAIWaypoint)
    {
        if (currentAIWaypoint < WaypointsBeforeOwner.Count && currentAIWaypoint >= 0)
            return WaypointsBeforeOwner[currentAIWaypoint];
        return null; 
    }
    private void Awake()
    {

        if (!location)
        {
            Debug.Log("Location for " + gameObject.name + ": store has not been set! Hence it wont be considered a shop");
            Destroy(this);
        }
       
    }

    void OnDrawGizmos()
    {
        Gizmos.DrawSphere(location.position, .1f);
        foreach (Vector3 x in points)
        {
            Gizmos.DrawSphere(x, .1f);
        }

    }
}
