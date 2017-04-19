using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class Cooler : MonoBehaviour {

	public AudioClip coolSound;
	private AudioSource audioSource;

	// Use this for initialization
	void Start () {

		audioSource = GetComponent<AudioSource>();

	}
	
	// Update is called once per frame
	void Update () {
		
	}

	private void OnTriggerEnter(Collider other)
	{

		if (other.gameObject.GetComponent<BlacksmithItem>() != null)
		{
			BlacksmithItem bsItem = other.GetComponent<BlacksmithItem>();
            bsItem.QuenchRate = 10f;
            audioSource.volume = bsItem.CurrentTemperature / 1f;
			audioSource.PlayOneShot(coolSound);
		}
	}

    private void OnTriggerExit(Collider other)
    {

        if (other.gameObject.GetComponent<BlacksmithItem>() != null)
        {
            BlacksmithItem bsItem = other.GetComponent<BlacksmithItem>();
            bsItem.QuenchRate = 1f;
            
        }
    }


}
