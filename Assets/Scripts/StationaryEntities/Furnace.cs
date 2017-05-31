using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(AudioSource))]
[RequireComponent(typeof(SphereCollider))]
public class Furnace : MonoBehaviour {

    public AudioClip clip;
    public AudioSource audioSource;
    private SphereCollider sphereCollider;

    void Start()
    {
        audioSource = GetComponent<AudioSource>();
        sphereCollider = GetComponent<SphereCollider>();
        sphereCollider.isTrigger = true;
    }


    private void OnTriggerStay(Collider other)
    {
        Ingot bsItem = other.GetComponentInParent<Ingot>();
        if (bsItem != null)
        {
            bsItem.SetHeatingEnvironment((sphereCollider.radius - (Vector3.Distance(other.transform.position,transform.TransformPoint(sphereCollider.center)))) * 1);
        }
    }


    private void OnTriggerExit(Collider other)
    {
        Ingot bsItem = other.GetComponentInParent<Ingot>();
        if(bsItem != null)
        bsItem.HeatSourceDetected = false;
           
    }






}
