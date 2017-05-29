using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayAudio : MonoBehaviour 
{
	[SerializeField] private AudioSource[] audio;

	public void PlayFX(int index)
	{
		if (audio.Length <= index + 1 && index >= 0)
			audio[index].Play();
	}
}
