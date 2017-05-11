using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Line {

    [Tooltip("A single line of message")]
    [TextArea][SerializeField]private string message;
    [SerializeField]private AudioClip clip;
    [HideInInspector][SerializeField]private string clipPath;

    public string Message
    {
        get { return this.message; }
        set { this.message = value; }
    }

    public AudioClip Clip
    {
        get { return this.clip; }
        set { this.clip = value; }
    }

    public string ClipPath
    {
        get { return this.clipPath; }
        set { this.clipPath = value; }
    }


}
