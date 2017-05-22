using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityEditorInternal;

[CustomEditor(typeof(HuntManager))]
public class HuntManagerEditor : Editor {

    HuntManager instance;

    private string[] storyLines;
    private string[] quests;
    private int selectedGridIndex;
    private int selectedQuestIndex;
    private int pselectedGridIndex;
    private int selectedDialogLine;
    private ReorderableList relist;
    private Vector2 questScrollPos;
    private Vector2 messageScrollPos;


    void OnEnable()
    {
        instance = (HuntManager)target;
        relist = new ReorderableList(serializedObject,serializedObject.FindProperty("storyHuntList"), true, true, true, true);
        //relist.drawHeaderCallback += DrawHeader;
        //relist.drawElementCallback += DrawElement;

        //relist.onAddCallback += AddItem;
        //relist.onRemoveCallback += RemoveItem;
    }

    void OnDisable()
    {
        //relist.drawHeaderCallback -= DrawHeader;
        //relist.drawElementCallback -= DrawElement;

        //relist.onAddCallback -= AddItem;
        //relist.onRemoveCallback -= RemoveItem;

    }

    private string ConvertToPath(AudioClip clip)
    {
        return  "Dialog expressions/" + clip.name;
    }

    private void GetAudioFilesFromPath()
    {
        foreach(StoryLine sl in instance.Storylines)
        {
            foreach(StoryHunt hunt in sl.Storyline)
            {
                foreach(Line l in hunt.Dialog.Lines)
                {
                    if (l.ClipPath != null)
                    {
                        Debug.Log("Loaded : " + l.ClipPath);
                        l.Clip = Resources.Load(l.ClipPath) as AudioClip;
                    }
                        
                }
            }
        }
    }


    public override void OnInspectorGUI()
    {

        DrawDefaultInspector();
        GUILayout.BeginHorizontal("Box");
        {
            if (GUILayout.Button("\nSave\n"))
            {
                SerializeManager.Save("storylines", instance.Storylines);
                Debug.Log("saved!");
            }

            if (GUILayout.Button("\nLoad Quest\n"))
            {
                instance.Storylines = (List<StoryLine>)SerializeManager.Load("storylines");
                GetAudioFilesFromPath();
                Debug.Log("loaded!");
            }
        }
        GUILayout.EndHorizontal();

        PopulateStoryLineSelection();
        DisplayQuestsInStoryLine();
        //CheckForEmptyLists();

        // Current selected quest
        GUILayout.Space(10);
        GUILayout.Label("Currently selected quest:", EditorStyles.boldLabel);
        GUILayout.BeginVertical("Box");
        {
            if (instance.Storylines[selectedGridIndex].Storyline.Count > 0)
            {
                GetSelectedQuest().Name = EditorGUILayout.TextField("Name", GetSelectedQuest().Name);
                GetSelectedQuest().Reward = (GameObject)EditorGUILayout.ObjectField("Reward", GetSelectedQuest().Reward, typeof(GameObject), true);
                GetSelectedQuest().Experience = EditorGUILayout.IntField("Experience", GetSelectedQuest().Experience);
                GetSelectedQuest().RequiredLevel = EditorGUILayout.IntField("Required Level", GetSelectedQuest().RequiredLevel);
                GetSelectedQuest().JobType = instance.Storylines[selectedGridIndex].JobType;

                GUILayout.Label("Dialogs", EditorStyles.largeLabel);
                if (GetSelectedQuest().Dialog == null)
                {
                    GetSelectedQuest().Dialog = new Session();
                    GetSelectedQuest().Dialog.Title = "Insert title here";
                }
                else
                    GetSelectedQuest().Dialog.Title = EditorGUILayout.TextField("Session Title", GetSelectedQuest().Dialog.Title);
  

                if (GetSelectedQuest().Dialog.Lines == null)
                {
                    GetSelectedQuest().Dialog.Lines = new List<Line>();
                    GetSelectedQuest().Dialog.Lines.Add(new Line());
                }

                messageScrollPos = GUILayout.BeginScrollView(messageScrollPos, GUILayout.Height(150));
                {
                    for (int i = 0; i < GetSelectedQuest().Dialog.Lines.Count; i++)
                    {
                        Line l = GetSelectedQuest().Dialog.Lines[i];

                        GUILayout.BeginVertical("Box");
                        {
                            GUILayout.Label("Message " + i, EditorStyles.boldLabel);
                            EditorStyles.textField.wordWrap = true;
                            l.Message = EditorGUILayout.TextArea(l.Message);
                            l.Clip = (AudioClip)EditorGUILayout.ObjectField("Audio", l.Clip, typeof(AudioClip), true);
                            if(l.Clip != null)
                            l.ClipPath = ConvertToPath(l.Clip);
                        }
                        GUILayout.EndVertical();


                        if (GUILayout.Button("Remove message") && GetSelectedQuest().Dialog.Lines.Count > 1)
                        {
                            GetSelectedQuest().Dialog.Lines.Remove(l);
                        }

                        GUILayout.Space(10);

                    }
                }
                GUILayout.EndScrollView();
                GUILayout.Space(10);

                if (GUILayout.Button("\nAdd new message\n"))
                {
                    GetSelectedQuest().Dialog.Lines.Add(new Line());
                }
  

            }
        }
        GUILayout.EndVertical();

        GUILayout.Space(10);
        GUILayout.Label("available storylines:", EditorStyles.boldLabel);

        if (selectedGridIndex >= instance.Storylines.Count)
        {
            selectedGridIndex = 0;
        }

        selectedGridIndex = GUILayout.SelectionGrid(selectedGridIndex, storyLines, instance.Storylines.Count);

        if (pselectedGridIndex != selectedGridIndex)
        {
            pselectedGridIndex = selectedGridIndex;
            selectedQuestIndex = 0;
        }
        
        // Quests
        GUILayout.BeginVertical("Box");
        {
            questScrollPos = GUILayout.BeginScrollView(questScrollPos, GUILayout.Height(150));
            selectedQuestIndex = GUILayout.SelectionGrid(selectedQuestIndex, quests, 1);
            GUILayout.EndScrollView();
        }
        GUILayout.EndVertical();

        // Storylines
        GUILayout.Space(10);
        if (GUILayout.Button("Add storyline"))
        {
            StoryLine tempLine = new StoryLine(JobType.ALCHEMY);
            tempLine.Storyline = new List<StoryHunt>();
            tempLine.Add(new StoryHunt("New Quest", instance.Storylines[selectedGridIndex].JobType, null, null, 0, 0, 0));
            instance.Storylines.Add(tempLine);
        }
        if (GUILayout.Button("Add quest"))
        {
            instance.Storylines[selectedGridIndex].Add(new StoryHunt("New Quest", instance.Storylines[selectedGridIndex].JobType, null, null, 0, 0, 0));
        }

    }

    private void CheckForEmptyLists()
    {
        if (instance.Storylines == null)
            instance.Storylines = new List<StoryLine>();
    }

    private StoryHunt GetSelectedQuest()
    {
        return instance.Storylines[selectedGridIndex].Storyline[selectedQuestIndex];
     
    }

    private StoryLine GetSelectedStoryLine()
    {
        return instance.Storylines[selectedGridIndex];
    }



    private void PopulateStoryLineSelection()
    {
        if (instance.Storylines.Count < 1)
        {
            instance.Storylines.Add(new StoryLine(JobType.BLACKSMITH));
        }      

        List<string> temp = new List<string>();
        
        foreach (StoryLine sl in instance.Storylines)
        {
            temp.Add(sl.JobType.ToString());
        }
        
        storyLines = temp.ToArray();

    }

    private void DisplayQuestsInStoryLine()
    {

        if (instance.Storylines[selectedGridIndex].Count < 1)
            instance.Storylines[selectedGridIndex].Add(new StoryHunt("New Quest", instance.Storylines[selectedGridIndex].JobType, null, null, 0, 0, 0));

        List<string> temp = new List<string>();
        quests = new string[instance.Storylines[selectedGridIndex].Count];

        int count = 0;

        foreach (StoryHunt hunt in instance.Storylines[selectedGridIndex].Storyline)
        {
            hunt.ProgressionIndex = count;
            count++;
            temp.Add(hunt.Name);
        }

        quests = temp.ToArray();


    }






}
