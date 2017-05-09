using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Session
{

    [Space(20f)]
    [Tooltip("Title of the conversation")]
    [SerializeField]private string title;
    [Tooltip("List of lines to say")]
    [SerializeField]private List<Line> lines;

    public string Title
    {
        get { return this.title; }
        set { this.title = value; }
    }

    public List<Line> Lines
    {
        get { return this.lines; }
        set { this.lines = value; }
    }


}
