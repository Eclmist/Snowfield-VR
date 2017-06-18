using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shop : MonoBehaviour
{

    [SerializeField]
    [Tooltip("Spot where AIs go to")]
    private Node locationNode;

    [SerializeField]
    private List<Node> WaypointsBeforeOwner;
    private bool isOccupied = false;

    public Node Location
    {
        get
        {
            return locationNode;
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

    public Node GetPoint(int currentAIWaypoint)
    {
        if (currentAIWaypoint < WaypointsBeforeOwner.Count && currentAIWaypoint >= 0)
            return WaypointsBeforeOwner[currentAIWaypoint];
        return null;
    }
    private void Awake()
    {

        if (!locationNode)
        {
            Debug.Log("Location for " + gameObject.name + ": store has not been set! Hence it wont be considered a shop");
            Destroy(this);
        }

    }


}
