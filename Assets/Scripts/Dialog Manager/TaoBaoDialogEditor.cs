using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[ExecuteInEditMode]
public class TaoBaoDialogEditor : MonoBehaviour
{

    [System.Serializable]
    public class Session
    {
        [Space(20f)]
        [Tooltip("Title of the conversation")]
        public string title;
        [Tooltip("List of lines to say")]
        public List<Line> lines;
    }

    [System.Serializable]
    public class Line
    {
        [Tooltip("A single line of message")]
        [TextArea]
        public string message;
        [NonSerialized]
        public AudioClip clip;
        [Tooltip("Name of audio that is going to play in this line")]
        public string audioName;


    }


    [SerializeField]
    private bool save;
    [SerializeField]
    private List<Session> sessions;

    private void Awake()
    {
        sessions = SerializeManager.Instance.Load("Dialogs") as List<Session>;
        LoadAudioFiles();

    }

    private void Update()
    {
        if (save = !save)
        {
            SerializeManager.Instance.Save("Dialogs", sessions);
            Debug.Log("Latest \"" + sessions[sessions.Count - 1].title + "\" saved");
        }

    }

    private void LoadAudioFiles()
    {
        foreach (Session s in sessions)
        {
            foreach (Line l in s.lines)
            {
                if (l.audioName != "")
                {
                    l.clip = Resources.Load(ConvertToPath(l.audioName)) as AudioClip;
                }


            }
        }
    }


    private string ConvertToPath(string s)
    {
        return "Dialog expressions/" + s + ".wav";
    }



    //private void ConvertToPath()
    //{
    //    foreach(Session s in sessions)
    //    {
    //        foreach(Line l in s.lines)
    //        {
    //            if(l.clip != null)
    //            l.path = UnityEditor.AssetDatabase.GetAssetPath(l.clip);

    //        }
    //    }
    //}



}