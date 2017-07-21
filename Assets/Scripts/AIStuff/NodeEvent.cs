using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[RequireComponent(typeof(Node))]
public abstract class NodeEvent : MonoBehaviour {

    public enum NodeCondition
    {
        NULL,
        DAY,
        NIGHT
    }

    protected Node node;

    public abstract void HandleEvent(AI ai);

    [SerializeField]
    protected bool finalNodeEvent;

    public bool Final
    {
        get
        {
            return finalNodeEvent;
        }
    }
    protected bool activated;

    public bool Activated
    {
        get
        {
            return activated;
        }
    }
    [SerializeField]
    protected NodeCondition condition;

    protected void Update()
    {
        if (condition == NodeCondition.NULL)
            activated = true;
        else if (condition == NodeCondition.DAY && GameManager.Instance.State == GameManager.GameState.DAYMODE)
            activated = true;
        else if (condition == NodeCondition.NIGHT && GameManager.Instance.State == GameManager.GameState.NIGHTMODE)
            activated = true;
        else
        {
            activated = false;
        }

    }

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



