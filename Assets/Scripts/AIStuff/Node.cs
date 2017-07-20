using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
[RequireComponent(typeof(SphereCollider))]
public class Node : MonoBehaviour, IBundle<Node>
{

    [SerializeField]
    private List<Node> neighbours = new List<Node>();
    private float startCost, endCost;
    private bool isSelected = false;
    private int bundleIndex;
    private Vector3 worldPosition;
    [SerializeField]
    protected LayerMask actorMask;
    
    private List<NodeEvent> nodeEvents = new List<NodeEvent>();

    protected bool isOccupied;
    public List<NodeEvent> Events
    {
        get
        {
            return nodeEvents;
        }
    }


    public bool Occupied
    {
        get
        {
            return isOccupied;
        }
        set
        {
            isOccupied = value;
        }
    }

    public bool Selected
    {
        get
        {
            return isSelected;
        }
        set
        {
            isSelected = value;
        }
    }

    #region ASTARONLY
    private Node parent;
    #endregion


    #region Properties
    public Node Parent
    {
        get { return parent; }
        set { parent = value; }
    }

    private float FCost
    {
        get
        {
            return startCost + endCost;
        }
    }

    public float SCost
    {
        set
        {
            startCost = value;
        }
        get
        {
            return startCost;
        }
    }

    public float ECost
    {
        set
        {
            endCost = value;
        }
        get
        {
            return endCost;
        }
    }

    public List<Node> Neighbours
    {
        get
        {
            return neighbours;
        }
        set
        {
            neighbours = value;
        }
    }

    public Vector3 Position
    {
        get
        {
            return worldPosition;
        }
    }

    #endregion
    private void Awake()
    {
        GetComponent<SphereCollider>().enabled = false;
        worldPosition = transform.position;
        nodeEvents.AddRange(GetComponents<NodeEvent>());
    }

    public int CompareTo(object _node)
    {
        Node node = (Node)_node;
        if (FCost < node.FCost || (FCost == node.FCost && this.SCost < node.SCost))
            return 1;
        else if (FCost == node.FCost && this.SCost == node.SCost)
            return 0;
        else
        {
            return -1;
        }
    }

    public int BundleIndex
    {
        get
        {
            return bundleIndex;
        }
        set { bundleIndex = value; }
    }

    protected void OnDrawGizmos()
    {

        if (!isSelected)
            Gizmos.color = Color.white;
        else
            Gizmos.color = Color.red;

        Gizmos.DrawSphere(transform.position, GetComponent<SphereCollider>().radius);

        foreach (Node n in neighbours)
        {
            Gizmos.DrawLine(transform.position, n.transform.position);
        }
    }

}
