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
    private bool isObstacle;
    private bool isSelected = false;
    private int bundleIndex;
    private Vector3 worldPosition;
    
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



    public Node Parent
    {
        get { return parent; }
        set { parent = value; }
    }



    public bool IsObstacle
    {
        get { return isObstacle; }
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

    private void Awake()
    {
        worldPosition = transform.position;
        GetComponent<SphereCollider>().enabled = false;
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
