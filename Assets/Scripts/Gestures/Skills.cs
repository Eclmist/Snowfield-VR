using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Skills : MonoBehaviour {

    public static Skills Instance;

    void Awake()
    {
        Instance = this;
    }

    public ParticleSystem charge;
    public ParticleSystem cast;


    //TODO: See if skills is available (Integrate with Player)
}
