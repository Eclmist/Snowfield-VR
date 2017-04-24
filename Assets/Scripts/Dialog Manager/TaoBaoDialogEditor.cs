using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[ExecuteInEditMode]
public class TaoBaoDialogEditor : MonoBehaviour, ISerializable
{

    public static TaoBaoDialogEditor Instance;

    [SerializeField]
    private bool save;
    [SerializeField]
    private List<Session> sessions;



    public List<Session> Sessions
    {
        get { return this.sessions; }
    }


    private void Awake()
    {
        Instance = this;
        sessions.Clear();
        //sessions = SerializeManager.Load("Dialogs") as List<Session>;
        Load();
        Debug.Log(sessions.Count);

    }

    private void Update()
    {

        if (save)
        {
            Save();
            //SerializeManager.Save("Dialogs", sessions);
            Debug.Log("Latest \"" + sessions[sessions.Count - 1].Title + "\" saved");
        }

    }


    private void LoadAudioFiles()
    {
        foreach (Session s in sessions)
        {
            foreach (Line l in s.Lines)
            {
                if (l.ClipPath != "")
                {
                    l.Clip = Resources.Load(l.ClipPath) as AudioClip;
                }


            }
        }
    }


    private string ConvertToPath(string s)
    {
        return "Dialog expressions/" + s;
    }





    private void ConvertToPath()
    {
        foreach (Session s in sessions)
        {
            foreach (Line l in s.Lines)
            {
                if (l.Clip != null)
                {
                    l.ClipPath = GetPath(l.Clip);
                    Debug.Log(l.ClipPath);
                }


            }
        }
    }

    public string GetPath(UnityEngine.Object o)
    {
        return "Dialog expressions/" + o.name;
    }

    public void Save()
    {
        //save the number of sessions
        SerializeManager.Save("sessionCount", sessions.Count);
        

        for (int i =0; i < sessions.Count;i++) // for each session
        {
            SerializeManager.Save("t" + i,sessions[i].Title);
            SerializeManager.Save("s" + i + "LineCount", sessions[i].Lines.Count);

            for (int j =0;j<Sessions[i].Lines.Count;j++) // for each line
            {
                

                if (Sessions[i].Lines[j].Clip != null)
                {
                    Sessions[i].Lines[j].ClipPath = GetPath(Sessions[i].Lines[j].Clip);
                }

                SerializeManager.Save("s"+i+"l"+j+"cp", Sessions[i].Lines[j].ClipPath);
                SerializeManager.Save("s"+i+"l"+j+"m", Sessions[i].Lines[j].Message);

            }
        }
    }

    public void Load()
    {
        sessions = new List<Session>();
        int sessionCount = (int)SerializeManager.Load("sessionCount");

        // Populate with empty sessions
        for (int i = 0; i < sessionCount; i++)
        {
            sessions.Add(new Session());
        }

        for (int i = 0; i < sessions.Count; i++)    // for each session
        {
            sessions[i].Title = SerializeManager.Load("t" + i) as string;
            int numOfLinesInSession = (int)SerializeManager.Load("s"+i+"LineCount");

            // Populate with empty lines
            for (int num = 0; num < numOfLinesInSession;num++)
            {
                sessions[i].Lines = new List<Line>();
                sessions[i].Lines.Add(new Line());
            }
 

            for (int j = 0; j < sessions[i].Lines.Count; j++) // for each line
            {
                sessions[i].Lines[j].ClipPath = SerializeManager.Load("s" + i + "l" + j + "cp") as string;
                sessions[i].Lines[j].Message = SerializeManager.Load("s" + i + "l" + j + "m") as string;
            }

        }

        LoadAudioFiles();
    }
}