using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class SoundEffect : MonoBehaviour {

    protected float hitStrength;
    protected AudioSource audioSource;

	// Use this for initialization
	protected void Start () {
        audioSource = GetComponent<AudioSource>();
	}


    protected void OnCollisionEnter(Collision collision)
    {
        audioSource.Play();
    }
}
