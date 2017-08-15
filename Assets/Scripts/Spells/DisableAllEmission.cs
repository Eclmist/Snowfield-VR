using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DisableAllEmission : MonoBehaviour {

    [SerializeField]
    protected float disableEmissionTime;

    protected float time;

    protected void Update()
    {
        time += Time.deltaTime;
        
        if (time >= disableEmissionTime)
        {
            foreach (Transform child in transform)
            {
                ParticleSystem particle = child.GetComponent<ParticleSystem>();

                if (particle)
                {
                    var em = particle.emission;
                    em.enabled = false;
                } 
            }
            Destroy(this.gameObject, 2);
        }
    }
}
