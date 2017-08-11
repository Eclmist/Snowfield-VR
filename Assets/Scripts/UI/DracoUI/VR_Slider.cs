using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class VR_Slider : VR_Button {

    [SerializeField] [Range(0, 1)] protected float vibrationThreshold = 0.05F;
    [SerializeField] [Range(0, 1)] protected float vibrationIntensity = 0.1F;

    protected Transform controller;

    [SerializeField] protected Slider slider;
    private float halfLength;
    private float halfLengthSquared;

    private Vector3 sliderRight;

    protected void Start()
    {
        Vector3 worldLeft = slider.fillRect.TransformPoint(slider.fillRect.rect.xMin, 0, 0);
        halfLength = Mathf.Abs((slider.transform.position - worldLeft).magnitude);
        halfLengthSquared = halfLength * halfLength;
        sliderRight = slider.transform.right * halfLength;
    }


    protected override void OnTriggerHold()
    {
        base.OnTriggerHold();


        float targetTransform = 
            Vector3.Dot(currentInteractingController.transform.position - slider.transform.position, sliderRight) / (halfLengthSquared);

        float normalizedValue = (targetTransform + 1) / 2;

        if (Mathf.Abs(slider.value - normalizedValue) > vibrationThreshold)
            currentInteractingController.Vibrate(vibrationIntensity);

        slider.value = Mathf.Lerp(slider.value, normalizedValue, Time.deltaTime * 20);

    }



    // Update is called once per frame

}
