using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public class AudioSettings : MonoBehaviour {
    [SerializeField] private AudioMixer mixer;

	public void SetMaster(float masterVol)
    {
        mixer.SetFloat("Master", masterVol);
    }

    public void SetBGM(float bgmVol)
    {
        mixer.SetFloat("BGM", bgmVol);
    }

    public void SetFX(float fxVol)
    {
        mixer.SetFloat("FX", fxVol);
    }
}
