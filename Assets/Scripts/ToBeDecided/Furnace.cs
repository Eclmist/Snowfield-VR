using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class Furnace : MonoBehaviour {

    public AudioClip clip;
    public AudioSource audioSource;
    private SphereCollider sphereCollider;

    void Start()
    {
        audioSource = GetComponent<AudioSource>();
        sphereCollider = GetComponent<SphereCollider>();
    }


    private void OnTriggerStay(Collider other)
    {
        if (other.gameObject.GetComponent<BlacksmithItem>() != null)
        {
            BlacksmithItem bsItem = GetBlacksmithComponent(other.gameObject);
            bsItem.SetHeatingEnvironment((sphereCollider.radius - (Vector3.Distance(other.transform.position,transform.TransformPoint(sphereCollider.center)))) * 1);
            
        }
    }


    private void OnTriggerExit(Collider other)
    {
        BlacksmithItem bsItem = GetBlacksmithComponent(other.gameObject);
        bsItem.HeatSourceDetected = false;
           
    }

    private BlacksmithItem GetBlacksmithComponent(GameObject g)
    {
        return g.GetComponent<BlacksmithItem>();
    }






}
