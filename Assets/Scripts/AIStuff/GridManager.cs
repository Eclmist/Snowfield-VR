using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class GridManager : MonoBehaviour
{

    public static GridManager instance;

    //[SerializeField]
    //private Vector2 mapSize;
    //[SerializeField]
    //private float nodeSize;

    //private Vector3 offset;
    //[SerializeField]
    //private LayerMask obstacles;

    public LayerMask terrainType;

    //private int noOfNodesX, noOfNodesY;

    #region EditorRef
    [HideInInspector]
    public Node selectedNode1, selectedNode2, latestSelectedNode;

    [Header("Editor")]
    public Node nodePrefab;
    
    public float nodesDistance;
    #endregion

    public List<Node> worldNodes = new List<Node>();


    void Awake()
    {
        instance = this;
        worldNodes.Clear();
        //offset = transform.position;
        //CreateGrid();
    }

    void Start()
    {
        //CalculateDefaultASTARNeighbours();

    }

    public void AddNode(Node newNode)
    {
        worldNodes.Add(newNode);
    }

    //private void CalculateDefaultASTARNeighbours()
    //{
    //    for (int i = 0; i < noOfNodesX; i++)
    //    {
    //        for (int j = 0; j < noOfNodesY; j++)
    //        {
    //            Node[] neighbours = new Node[8];
    //            int index = 0;
    //            for (int x = -1; x <= 1; x++)
    //            {
    //                for (int y = -1; y <= 1; y++)
    //                {
    //                    if (x == 0 && y == 0)
    //                        continue;
    //                    int gridX = i + x;
    //                    int gridY = j + y;


    //                    if (gridX >= 0 && gridX < noOfNodesX && gridY >= 0 && gridY < noOfNodesY)
    //                        neighbours[index] = worldNodes[gridX, gridY];

    //                    index++;
    //                }
    //            }
    //            worldNodes[i, j].Neighbours = neighbours;
    //        }
    //    }
    //}

    //private void CreateGrid()
    //{
    //    noOfNodesX = Mathf.FloorToInt(mapSize.x / nodeSize);
    //    noOfNodesY = Mathf.FloorToInt(mapSize.y / nodeSize);
    //    worldNodes = new Node[noOfNodesX, noOfNodesY];
    //    for (int i = 0; i < noOfNodesX; i++)
    //    {
    //        for (int j = 0; j < noOfNodesY; j++)
    //        {
    //            bool isObstacle;
    //            Vector3 worldPoint = offset + new Vector3(nodeSize * i + nodeSize / 2, 0, nodeSize * j + nodeSize / 2);// The Y zero value can eventually be changed to something that refers to height too
    //            RaycastHit hit;

    //            bool collided = Physics.Raycast(worldPoint + Vector3.up * 1000, Vector3.down, out hit, Mathf.Infinity, terrainType);
    //            if (collided)
    //            {
    //                worldPoint.y = hit.point.y;
    //                isObstacle = Physics.CheckSphere(worldPoint, nodeSize, obstacles);
    //            }
    //            else
    //            {
    //                worldPoint.y = 0;
    //                isObstacle = true;
    //            }

    //            Node newNode = new Node(worldPoint, isObstacle);
    //            worldNodes[i, j] = newNode;
    //        }
    //    }

    //}


    //void OnDrawGizmos()
    //{
    //    offset = transform.position;
    //    Gizmos.DrawWireCube(offset + new Vector3(mapSize.x / 2, 0, mapSize.y / 2), new Vector3(mapSize.x, .1f, mapSize.y));//Gizmos to draw the size of an object with assumption that


    //    if (worldNodes != null)
    //    {
    //        foreach (Node X in worldNodes)
    //        {
    //            if (X.IsObstacle)
    //                Gizmos.color = Color.red;
    //            else
    //            {
    //                Gizmos.color = new Color(0, 0, 0.1f * X.Neighbours.Length);
    //            }

    //            Gizmos.DrawCube(X.Position, new Vector3(nodeSize, .1f, nodeSize));
    //        }
    //    }
    //    else
    //    {
    //        Gizmos.color = Color.red;
    //        Gizmos.DrawCube(offset + new Vector3(nodeSize / 2, 11, nodeSize / 2), new Vector3(nodeSize, .1f, nodeSize));//Gizmos to draw the size of an object with assumption that
    //    }
    //    if (OpenNodes != null)
    //    {
    //        for (int i = 0; i < OpenNodes.Count; i++)
    //        {
    //            Gizmos.color = Color.yellow;

    //            Gizmos.DrawCube(OpenNodes[i].Position, new Vector3(nodeSize, nodeSize, nodeSize));//Gizmos to draw the size of an object with assumption that
    //        }
    //    }
    //    if (path != null)
    //    {
    //        foreach (Node x in path)
    //        {
    //            Gizmos.color = Color.green;
    //            Gizmos.DrawCube(x.Position, new Vector3(nodeSize, nodeSize, nodeSize));//Gizmos to draw the size of an object with assumption that
    //        }
    //    }
    //}

    public Node NodeFromWorldPoint(Vector3 _position)
    {
        Node closestNode = null;
        float closestDist = 1000000f;

        foreach (Node node in worldNodes)
        {
            float currentDist = Vector3.Distance(node.Position, _position);

            if (currentDist < closestDist)
            {
                closestDist = currentDist;
                closestNode = node;
            }
        }

        return closestNode;
    }




}
