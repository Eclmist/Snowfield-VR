using UnityEngine;
using System.Collections;

public class Ragdoll : MonoBehaviour {

    bool isLimp = false;
    public Rigidbody[] myParts;

    // Use this for initialization
	void Start () {
        //Turn off all isKinematics
        myParts = GetComponentsInChildren<Rigidbody>();

        foreach(UnityFixer uf in GetComponentsInChildren<UnityFixer>())
        {
            uf.papaRagdoll = this;
        }

        foreach (Rigidbody rb in GetComponentsInChildren<Rigidbody>())
        {
            rb.isKinematic = true;
        }
        foreach(Collider bc in GetComponentsInChildren<Collider>())
        {
            bc.isTrigger = true;
        }

        isLimp = false;

	}

    public void TriggerWarning()
    {

        if (!isLimp)
        {
            foreach (Rigidbody rb in GetComponentsInChildren<Rigidbody>())
            {
                rb.isKinematic = false;
            }
            foreach (Collider bc in GetComponentsInChildren<Collider>())
            {
                bc.isTrigger = false;
            }
            isLimp = true;
        }

    }

    void Gravity()
    {

    }
	
	// Update is called once per frame
	void Update () {
	
	}

}
