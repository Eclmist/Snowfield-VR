using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode]
public class TaoBaoDialogEditor : MonoBehaviour {

    [System.Serializable]
    public struct Session
    {
        [Space(20f)]
        [Tooltip("Title of the conversation")]   public string title;
        [Tooltip("List of lines to say")] public List<Line> lines;
    }

    [System.Serializable]
    public struct Line
    {
        [Tooltip("A single line of message")] [TextArea] public string message;
        [Tooltip("Whatever sound that is going to play in this line")] public AudioClip clip;
    }


    [SerializeField]
    private bool save;
    [SerializeField]
    private List<Session> sessions;

    void Update()
    {
        if (save = !save)
            Debug.Log("Latest \""+ sessions[sessions.Count-1].title+"\" saved");
    }




}
