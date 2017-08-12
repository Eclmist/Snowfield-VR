using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour {


	[Tooltip("0 being midnight and 0.5 being noon")]
	[SerializeField] [Range(0, 1)] private float dayStartAt;

	[Tooltip("0 being midnight and 0.5 being noon")]
	[SerializeField]
	[Range(0, 1)]
	private float nightStartAt;

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
			if (GameManager.Instance.GameClock.TimeOfDay > dayStartAt && GameManager.Instance.GameClock.TimeOfDay < nightStartAt)
			{
				if (!dayIsPlaying)
				{
					dayIsPlaying = true;
					StartCoroutine(FadeOut(nightTime));
					StartCoroutine(StartPlayingAfterRandomTime(dayTime));
				}
			}
			else
			{
				if (dayIsPlaying)
				{
					dayIsPlaying = false;
					StartCoroutine(FadeOut(dayTime));
					StartCoroutine(StartPlayingAfterRandomTime(nightTime));
				}

			}
		}

		//Debug();
	}

	protected IEnumerator StartPlayingAfterRandomTime(AudioSource audio)
	{
		yield return new WaitForSeconds(Random.Range(5, 30));

		yield return StartCoroutine(FadeIn(audio));
	}



	protected IEnumerator FadeIn(AudioSource audio)
    {
		audio.Play();

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
