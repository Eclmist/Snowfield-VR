using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public abstract class NodeEvent : MonoBehaviour {

    public abstract void HandleEvent(AI ai);
}



