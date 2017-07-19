using UnityEngine;
using System.Collections;

public class UnityFixer : MonoBehaviour {

    // Use this for initialization
    public Ragdoll papaRagdoll;

	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    void OnTriggerEnter(Collider c)
    {
        if(c.tag == "Pain")
        {
            papaRagdoll.TriggerWarning();
        }
        
    }
}
