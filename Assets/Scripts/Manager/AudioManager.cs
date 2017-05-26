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
    }

    protected void Update()
    {
        //if(timeline == day){  **Future Implementation
        if (!dayIsPlaying)
        {
            dayTime.Play();
            dayIsPlaying = true;
        }
        //}
        //else // if timeline == night
        //{
        //  if(nightIsPlaying == false && dayIsPlaying == true)
        //  {
        //      FadeOut(dayTime);
        //      dayIsPlaying = false;

        //      nightTime.Play;
        //      FadeIn(nightTime);
        //      nightIsPlaying = true;
        //  }
        //}
    }

    protected void FadeIn(AudioSource audio)
    {
        audio.volume = 0.0f;

        while (audio.volume < 1)
        {
            audio.volume += 0.00001f * Time.deltaTime / 5;
        }
    }

    protected void FadeOut(AudioSource audio)
    {
        while (audio.volume > 0)
        {
            audio.volume -= 0.00001f * Time.deltaTime / 5;
        }

        audio.Stop();
    }
}
