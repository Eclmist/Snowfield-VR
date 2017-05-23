using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(OrderManager))]
public class OrderManagerEditor : Editor {

    OrderManager instance;

	void OnEnable()
    {
        instance = (OrderManager)target;
    }

    void OnDisable()
    {

    }

    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();


    }





}
