using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Diagnostics;
using System.Net;
using System.Threading;
using Debug = UnityEngine.Debug;

public class AStarManager : MonoBehaviour
{
    public static volatile AStarManager Instance;
    private Thread pathFindingThread;
    private bool processingPath;
    private volatile List<PathRequest> requestQueue = new List<PathRequest>();
    [SerializeField]
    private bool exit;

    //public enum TypeofPathFinding
    //{
    //    DEFAULTASTAR,
    //    GREEDYASTAR,
    //    JUMPPOINTPRECOMPILE,
    //    JUMPPOINTPOSTCOMPILE
    //}

    //public TypeofPathFinding pType;

    void Awake()
    {
        Instance = this;

        pathFindingThread = new Thread(PathProcess);
        pathFindingThread.Start();
    }

    private void PathProcess()
    {
        while (!exit)
        {
            try
            {
                
                if (!processingPath && requestQueue.Count > 0)
                {
                    ProcessNextPath();
                }else
                {
                    Thread.Sleep(10);
                }

            }
            catch (Exception e)
            {
                Debug.Log(e.Message);
                break;
            }
        }
    }

    public void RequestPath(Vector3 _startPoint, Vector3 _endPoint, Action<List<Node>> _callback)
    {
     
        PathRequest newRequest = new PathRequest(_startPoint, _endPoint, _callback/*, _steppableHeight*/);
        lock (requestQueue)
        {
            requestQueue.Add(newRequest);
        }
        
    }
    private void ProcessNextPath()
    {
        processingPath = true;
        PathRequest currentPathRequest;
        lock (requestQueue)
        {
            currentPathRequest = requestQueue[0];
            requestQueue.RemoveAt(0);
        }
        CalculatePath(currentPathRequest);
    }

    private void FinishedProcessingPath(PathRequest _request, List<Node> wayPoint)
    {
       _request.callBack(wayPoint);
       processingPath = false;
    }

    #region StandardAStar

    private void CalculatePath(PathRequest _request)
    {
        List<Node> wayPoint = new List<Node>();

        Node startNode = GridManager.instance.NodeFromWorldPoint(_request.startPoint);

        Node endNode = GridManager.instance.NodeFromWorldPoint(_request.endPoint);

        
            Heap<Node> openNodes = new Heap<Node>();
            HashSet<Node> closeNodes = new HashSet<Node>();
            openNodes.Add(startNode);

            while (openNodes.Count > 0)
            {
                Node currentNode = openNodes.GetFirst();

                closeNodes.Add(currentNode);

                if (currentNode == endNode)
                {
                    wayPoint = RetraceNodes(startNode, currentNode);
                    break;
                }

                for (int i = 0; i < currentNode.Neighbours.Count; i++)
                {

                    Node currentNeighbour = currentNode.Neighbours[i];
                    if (currentNeighbour == null
                        || closeNodes.Contains(currentNeighbour))
                    /*|| Mathf.Abs(currentNeighbour.Position.y - currentNode.Position.y) > _request.stepHeight*/
                    {
                        continue;
                    }


                    float newMovementCost = currentNode.SCost + Vector3.Distance(currentNode.Position, currentNeighbour.Position);
                    if (newMovementCost < currentNeighbour.SCost || !openNodes.Contains(currentNeighbour))
                    {
                        currentNeighbour.SCost = newMovementCost;
                        currentNeighbour.ECost = Vector3.Distance(currentNeighbour.Position, endNode.Position);
                        currentNeighbour.Parent = currentNode;

                        if (!openNodes.Contains(currentNeighbour))
                        {
                            openNodes.Add(currentNeighbour);

                            openNodes.UpdateItem(currentNeighbour);
                        }
                        else
                            openNodes.UpdateItem(currentNeighbour);
                    }

                
            }
        }

        FinishedProcessingPath(_request, wayPoint);
    }

    private List<Node> RetraceNodes(Node _startNode, Node _endNode)
    {
        List<Node> path = new List<Node>();

        Node currentNode = _endNode;
        while (currentNode != _startNode)
        {
            path.Add(currentNode);
            currentNode = currentNode.Parent;
        }
        path.Add(currentNode);
        //Just for visualizing to be removed
        //Vector3 direction = Vector3.zero;

        //for (int i = 1; i < path.Count; i++)
        //{

        //    Vector3 newDirection = path[i - 1].Position - path[i].Position;
        //    if (direction != newDirection)
        //    {
        //        wayPoint.Add(path[i].Position);

        //    }
        //    direction = newDirection;
        //}
        path.Reverse();
        return path;

    }

    #endregion
    void OnDisable()
    {
        exit = true;
    }

}

public struct PathRequest
{
    public readonly Vector3 startPoint;
    public readonly Vector3 endPoint;
    public readonly Action<List<Node>> callBack;
    //public readonly float stepHeight;

    public PathRequest(Vector3 _startPoint, Vector3 _endPoint, Action<List<Node>> _callBack/* float _steppableHeight*/)
    {
        startPoint = _startPoint;
        endPoint = _endPoint;
        callBack = _callBack;
        //stepHeight = _steppableHeight;
    }
}
