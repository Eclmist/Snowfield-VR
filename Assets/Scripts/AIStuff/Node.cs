using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Node : IBundle<Node> {

    private Node[] neighbours = new Node[8];
    private Vector3 worldPosition;
    private float startCost, endCost;
    private bool isObstacle;

    private int bundleIndex;

    #region ASTARONLY
    private Node parent;
    #endregion

    public Node Parent
    {
        get { return parent; }
        set { parent = value; }
    }

    public Node(Vector3 _worldPosition, bool _isObstacle)
    {
        worldPosition = _worldPosition;
        isObstacle = _isObstacle;
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

    public Node[] Neighbours
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

    public int CompareTo(object _node)
    {
        Node node = (Node) _node;
        if (FCost < node.FCost || (FCost == node.FCost && this.SCost < node.SCost))
            return 1;
        else if(FCost == node.FCost && this.SCost == node.SCost)
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
}
