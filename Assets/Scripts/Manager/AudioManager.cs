using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour {


	[SerializeField]
    protected AudioSource ambient;

    [SerializeField]
    protected AudioSource dayTime;

    [SerializeField]
    protected AudioSource nightTime;

    protected bool dayIsPlaying = false;


    protected void Awake()
    {
		ambient.Play();
		//StartCoroutine(FadeIn(ambient));
	}

    protected void Update()
    {
		if (GameManager.Instance)
		{

			// if daytime
			if (GameManager.Instance.GameClock.IsDay)
			{
				if (!dayIsPlaying)
				{
					dayIsPlaying = true;
					StopAllCoroutines();
					StartCoroutine(FadeOut(nightTime));
					StartCoroutine(StartPlayingAfterRandomTime(dayTime));
				}
			}
			else
			{
				if (dayIsPlaying)
				{
					dayIsPlaying = false;
					StopAllCoroutines();
					StartCoroutine(FadeOut(dayTime));
					StartCoroutine(StartPlayingAfterRandomTime(nightTime));
				}

			}
		}

		//Debug();
	}

	protected IEnumerator StartPlayingAfterRandomTime(AudioSource audio)
	{
		yield return new WaitForSeconds(Random.Range(5, 20));

		yield return StartCoroutine(FadeIn(audio));
	}



	protected IEnumerator FadeIn(AudioSource audio)
    {
		audio.Play();
	    audio.volume = 0;
		while (audio.volume < 1)
        {
            audio.volume += Time.deltaTime;

            yield return null;
        }
    }

    protected IEnumerator FadeOut(AudioSource audio)
    {
		while (audio.volume > 0)
        {
            audio.volume -= Time.deltaTime;

			yield return null;
		}

		audio.Stop();
    }

    protected void Debug()
    {
        if (Input.GetKeyDown(KeyCode.KeypadMinus))
        {
            StartCoroutine(FadeOut(dayTime));
            StartCoroutine(FadeOut(ambient));
            nightTime.Play();
            StartCoroutine(FadeIn(nightTime));
        }

        if (Input.GetKeyDown(KeyCode.KeypadPlus))
        {
            StartCoroutine(FadeOut(nightTime));
            dayTime.Play();
            StartCoroutine(FadeIn(dayTime));
            ambient.Play();
            StartCoroutine(FadeIn(ambient));
        }
    }
}
