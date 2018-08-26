using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroyParticle : MonoBehaviour {

    private ParticleSystem particleRef;

	// Use this for initialization
	void Start () {
        particleRef = GetComponent<ParticleSystem>();
    }
	
	// Update is called once per frame
	void Update () {
        if (!particleRef.IsAlive())
        {
            Destroy(this.gameObject);
        }
    }
}
