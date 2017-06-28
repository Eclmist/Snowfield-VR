using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(SphereCollider))]
public class MorphingTable : MonoBehaviour {

    public Transform lockTransform;

    [SerializeField]
    private Ingot ingotReference;

    void Start()
    {
        GetComponent<SphereCollider>().isTrigger = true;    
    }


    // Update is called once per frame
    void Update () {

        if(ingotReference!=null)
        {
            if (ingotReference.transform.position != lockTransform.position
                        || ingotReference.transform.rotation != lockTransform.rotation)
            {
                ingotReference = null;
            }

        }

	}

    private void OnTriggerEnter(Collider other)
    {

        Ingot ingot = other.gameObject.GetComponent<Ingot>();

        if (ingotReference == null && ingot != null)
        {
            ingotReference = ingot;

            if (ingot.LinkedController != null)
                ingot.OnControllerExit(ingot.LinkedController);

            ingot.GetComponent<Rigidbody>().isKinematic = true;
            LockIntoPosition(ingot.transform);
        }
    }


    public void LockIntoPosition(Transform t)
    {
        t.position = lockTransform.position;
        t.rotation = lockTransform.rotation;
    }

    private void OnDrawGizmos()
    {
        Gizmos.DrawWireSphere(GetComponent<SphereCollider>().center, GetComponent<SphereCollider>().radius);
    }












}
