using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shop : MonoBehaviour
{

    [SerializeField]
    [Tooltip("Spot where AIs go to")]
    private Node locationNode,interactionNode;

    [SerializeField]
    private List<Node> wayPoints;
    private bool isOccupied = false;

    public Node Location
    {
        get
        {
            return locationNode;
        }

    }

    public Node InteractionNode
    {
        get
        {
            return interactionNode;
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

    public Node GetRandomPoint(Vector3 currentPoint)
    {
        List<Node> tempNode = new List<Node>();
        tempNode.AddRange(wayPoints);
        
        tempNode.RemoveAll(Node => Node.Occupied || Vector3.Distance(Node.Position,currentPoint) < 0.5);
        if (tempNode.Count == 0)
        {
            return null;
        }
        else
        {

            int randomNumber = Random.Range(0, tempNode.Count);

            return tempNode[randomNumber];
        }
    }
    private void Awake()
    {

        if (!locationNode)
        {
            Debug.Log("Location for " + gameObject.name + " has not been set! Hence it wont be considered a shop");
            Destroy(this);
        }

    }


}
