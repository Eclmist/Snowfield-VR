using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[RequireComponent(typeof(Node))]
public abstract class NodeEvent : MonoBehaviour {

    protected Node node;

    public abstract void HandleEvent(AI ai);

    public Node CurrentNode
    {
        get
        {
            return node;
        }
    }

    protected virtual void Awake()
    {
        node = GetComponent<Node>();
    }
}



