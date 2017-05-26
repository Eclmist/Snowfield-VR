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
    protected bool nightIsPlaying = false;

    protected void Awake()
    {
        ambient.Play();
        dayTime.Play();
    }

    protected void Update()
    {
        //if (timeline == day && !dayIsPlaying)    //**Future Implementation
        //{
        //    if (nightIsPlaying)
        //    {
        //        StartCoroutine(FadeOut(nightTime));
        //        dayTime.Play();
        //        StartCoroutine(FadeIn(dayTime));
        //    }
        //}
        //else if (timeline == night && !nightIsPlaying)
        //{
        //    if (dayIsPlaying)
        //    {
        //        StartCoroutine(FadeOut(ambient));
        //        StartCoroutine(FadeOut(dayTime));
        //        nightTime.Play();
        //        StartCoroutine(FadeIn(nightTime));
        //    }
        //}

        Debug();
    }

    protected IEnumerator FadeIn(AudioSource audio)
    {
        audio.volume = 0.0f;

        while (audio.volume < 1)
        {
            audio.volume += 0.01f;

            yield return new WaitForSeconds(0.01f);
        }
    }

    protected IEnumerator FadeOut(AudioSource audio)
    {
        while (audio.volume > 0)
        {
            audio.volume -= 0.02f;

            yield return new WaitForSeconds(0.01f);
        }

        if(audio == dayTime)
        {
            dayIsPlaying = false;
        }
        else if(audio == nightTime)
        {
            nightIsPlaying = false;
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
