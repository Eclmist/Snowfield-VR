using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Edwon.VR.Gesture;
using Edwon.VR;

public class Controller : MonoBehaviour {

    protected VRGestureRig rig;

    protected Transform leftHand;
    protected Transform rightHand;

    private void Start()
    {
        rig = FindObjectOfType<VRGestureRig>();
        if (rig == null)
        {
            Debug.Log("there is no VRGestureRig in the scene, please add one");
        }

        rightHand = rig.handRight;
        leftHand = rig.handLeft;
    }

    void OnEnable()
    {
        GestureRecognizer.GestureDetectedEvent += OnGestureDetected;
    }

    void OnDisable()
    {
        GestureRecognizer.GestureDetectedEvent -= OnGestureDetected;
    }


    // called when a gesture is detected  	
    void OnGestureDetected(string gestureName, double confidence, Handedness hand, bool isDouble = false)
    {
        switch (gestureName)
        {
            case "Circle1":
                {
                    // DO SOMETHING REACTIN TO CIRCLE GESTURE
                    SorceryCast.Instance.Charge(rightHand);
                }
                break;
            case "Push1":
                {
                    // DO SOMETHING REACTING TO TRIANGLE GESTURE
                    SorceryCast.Instance.Cast(rightHand);
                }
                break;
        }
    }
}

