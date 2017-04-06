/* 
 Author : Ivan Leong
 Description : Quest Searcher
*/

using System;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEditor;

public class QuestSearcher : EditorWindow {

    static QuestSearcher questSearcher;

    //QuestMaker questMaker;

    //-----Editable Variable-----//
    //TextAsset questTypes;
    //string[] questType;
    //int questTypeIndex = 0;

    //TextAsset jamesRewards;
    //string[] jamesReward;
    //int jamesRewardIndex = 0;

    //TextAsset questItems;
    //string[] questItem;
    //int questItemIndex = 0;

    //-----Image-----//
    Texture2D deleteButtonImage;
    Texture2D editButtonImage;

    //-----Input-----//
    int questIndex;
    int nameIndex;
    string questName;

    //-----Authentication-----//
    bool indexPassed = false;
    bool namePassed = false;

    bool searchIndexMethod;
    bool searchNameMethod;

    //-----Function Boolean-----//
    bool editable;

    [MenuItem("Window/Quest/Quest Searcher")]
    protected static void Init()
    {
        questSearcher = (QuestSearcher)EditorWindow.GetWindow(typeof(QuestSearcher));
        questSearcher.minSize = new Vector2(800, 400);
        questSearcher.Show();
    }

    protected void OnEnable()
    {
        Quest.QuestList = QuestMaker.questMaker.Deserialize();
        Debug.Log(Quest.QuestList.Count);

        //----------Load Image----------//
        deleteButtonImage = Resources.Load("Editor/Image/trash") as Texture2D;
        editButtonImage = Resources.Load("Editor/Image/edit.jpg") as Texture2D;

        ////---------------------------Init QuestTypes-----------------------//
        //questTypes = Resources.Load("Quests/QuestTypes") as TextAsset;

        //if (questTypes == null)
        //{
        //    Debug.LogError("QuestTypes.txt is missing!");
        //}
        //else
        //{
        //    questType = questTypes.text.Split('\n');
        //}

        ////---------------------------Init jamesRewards-----------------------//
        //jamesRewards = Resources.Load("Quests/questRewards") as TextAsset;

        //if (jamesRewards == null)
        //{
        //    Debug.LogError("questReward.txt is missing!");
        //}
        //else
        //{
        //    jamesReward = jamesRewards.text.Split('\n');
        //}

        ////---------------------------Init QuestRequiredItems-----------------------//
        //questItems = Resources.Load("Quests/QuestRequiredItems") as TextAsset;

        //if (questItems == null)
        //{
        //    Debug.LogError("QuestRequiredItems.txt is missing!");
        //}
        //else
        //{
        //    questItem = questItems.text.Split('\n');
        //}
    }

    protected void OnGUI()
    {
        if (searchIndexMethod)
        {
            searchNameMethod = false;
            namePassed = false;
        }

        if(searchNameMethod)
        {
            searchIndexMethod = false;
            indexPassed = false;
        }

        EditorGUILayout.LabelField("Quest Searcher", EditorStyles.boldLabel);

        //EditorGUILayout.Toggle(searchIndexMethod, "Index");
        searchIndexMethod = EditorGUILayout.ToggleLeft("Search by Index", searchIndexMethod);
        searchNameMethod = EditorGUILayout.ToggleLeft("Search by Name", searchNameMethod);

        if (searchIndexMethod)
        {
            EditorGUILayout.BeginHorizontal();
            EditorGUILayout.PrefixLabel("Quest Index: ");
            questIndex = EditorGUILayout.IntSlider(questIndex, 0, Quest.QuestList.Count - 1);
            EditorGUILayout.EndHorizontal();
        }
        else if(searchNameMethod)
        {
            EditorGUILayout.BeginHorizontal();
            EditorGUILayout.PrefixLabel("Quest Name: ");
            questName = EditorGUILayout.TextArea(questName);
            EditorGUILayout.EndHorizontal();
        }

        //if (!editable)
        //{
        if (indexPassed)
        {
            EditorGUILayout.LabelField("Quest Index", EditorStyles.boldLabel);
            EditorGUILayout.SelectableLabel(Quest.QuestList[questIndex].index.ToString());

            EditorGUILayout.LabelField("Quest for: " + Quest.QuestList[questIndex].questJob, EditorStyles.boldLabel);
            EditorGUILayout.LabelField("Required Experience: " + Quest.QuestList[questIndex].requiredExperience, EditorStyles.boldLabel);

            EditorGUILayout.LabelField("Quest Title: ", EditorStyles.boldLabel);
            EditorGUILayout.SelectableLabel(Quest.QuestList[questIndex].title, EditorStyles.wordWrappedLabel);

            EditorGUILayout.LabelField("Quest Type: ", EditorStyles.boldLabel);
            EditorGUILayout.SelectableLabel(Quest.QuestList[questIndex].questType.ToString());

            EditorGUILayout.LabelField("James' Rewards: ", EditorStyles.boldLabel);
            EditorGUILayout.SelectableLabel(Quest.QuestList[questIndex].jamesReward);

            EditorGUILayout.LabelField("Quest Required Items: ", EditorStyles.boldLabel);
            EditorGUILayout.SelectableLabel(Quest.QuestList[questIndex].requiredItem);

            for (int i = 0; i < Quest.QuestList[questIndex].playerDialog.Length; i++)
            {
                if (Quest.QuestList[questIndex].playerDialog[i] != null)
                {
                    EditorGUILayout.LabelField("Stanley Dialog: ");
                    EditorGUILayout.SelectableLabel(Quest.QuestList[questIndex].playerDialog[i]);
                }
            }
        }

        if (namePassed)
        {
            EditorGUILayout.LabelField("Quest Index: ", EditorStyles.boldLabel);
            EditorGUILayout.SelectableLabel(Quest.QuestList[nameIndex].index.ToString());

            EditorGUILayout.LabelField("Quest for: " + Quest.QuestList[nameIndex].questJob, EditorStyles.boldLabel);
            EditorGUILayout.LabelField("Required Experience: " + Quest.QuestList[nameIndex].requiredExperience, EditorStyles.boldLabel);

            EditorGUILayout.LabelField("Quest Title: ", EditorStyles.boldLabel);
            EditorGUILayout.SelectableLabel(Quest.QuestList[nameIndex].title, EditorStyles.wordWrappedLabel);

            EditorGUILayout.LabelField("Quest Type: ", EditorStyles.boldLabel);
            EditorGUILayout.SelectableLabel(Quest.QuestList[nameIndex].questType.ToString());

            EditorGUILayout.LabelField("Quest Rewards: ", EditorStyles.boldLabel);
            EditorGUILayout.SelectableLabel(Quest.QuestList[nameIndex].jamesReward);

            EditorGUILayout.LabelField("Quest Required Items: ", EditorStyles.boldLabel);
            EditorGUILayout.SelectableLabel(Quest.QuestList[nameIndex].requiredItem);

            for (int i = 0; i < Quest.QuestList[nameIndex].playerDialog.Length; i++)
            {
                if (Quest.QuestList[nameIndex].playerDialog[i] != null)
                {
                    EditorGUILayout.LabelField("Stanley Dialog: ");
                    EditorGUILayout.SelectableLabel(Quest.QuestList[nameIndex].playerDialog[i]);
                }
            }
        }
        //}
        //else //if editable
        //{
        //    if (indexPassed)
        //    {
        //        EditorGUILayout.LabelField("Quest Index", EditorStyles.boldLabel);
        //        Quest.QuestList[questIndex].index = EditorGUILayout.IntField(Quest.QuestList[questIndex].index);

        //        EditorGUILayout.LabelField("Quest Title: ", EditorStyles.boldLabel);
        //        Quest.QuestList[questIndex].title = EditorGUILayout.TextArea(Quest.QuestList[questIndex].title);

        //        EditorGUILayout.LabelField("Quest Type: ", EditorStyles.boldLabel);
        //        questTypeIndex = EditorGUILayout.Popup("Quest Types", questTypeIndex, questType);

        //        EditorGUILayout.LabelField("Quest Rewards: ", EditorStyles.boldLabel);
        //        EditorGUILayout.SelectableLabel(Quest.QuestList[questIndex].jamesReward);

        //        EditorGUILayout.LabelField("Quest Required Items: ", EditorStyles.boldLabel);
        //        EditorGUILayout.SelectableLabel(Quest.QuestList[questIndex].requiredItem);

        //        for (int i = 0; i < Quest.QuestList[questIndex].playerDialog.Length; i++)
        //        {
        //            if (Quest.QuestList[questIndex].playerDialog[i] != null)
        //            {
        //                EditorGUILayout.LabelField("Stanley Dialog: ");
        //                EditorGUILayout.SelectableLabel(Quest.QuestList[questIndex].playerDialog[i]);
        //            }
        //        }
        //    }

        //    if (namePassed)
        //    {
        //        EditorGUILayout.LabelField("Quest Index: ", EditorStyles.boldLabel);
        //        EditorGUILayout.SelectableLabel(Quest.QuestList[nameIndex].index.ToString());

        //        EditorGUILayout.LabelField("Quest Title: ", EditorStyles.boldLabel);
        //        EditorGUILayout.SelectableLabel(Quest.QuestList[nameIndex].title, EditorStyles.wordWrappedLabel);

        //        EditorGUILayout.LabelField("Quest Type: ", EditorStyles.boldLabel);
        //        EditorGUILayout.SelectableLabel(Quest.QuestList[nameIndex].questType.ToString());

        //        EditorGUILayout.LabelField("Quest Rewards: ", EditorStyles.boldLabel);
        //        EditorGUILayout.SelectableLabel(Quest.QuestList[nameIndex].jamesReward);

        //        EditorGUILayout.LabelField("Quest Required Items: ", EditorStyles.boldLabel);
        //        EditorGUILayout.SelectableLabel(Quest.QuestList[nameIndex].requiredItem);

        //        for (int i = 0; i < Quest.QuestList[nameIndex].playerDialog.Length; i++)
        //        {
        //            if (Quest.QuestList[nameIndex].playerDialog[i] != null)
        //            {
        //                EditorGUILayout.LabelField("Stanley Dialog: ");
        //                EditorGUILayout.SelectableLabel(Quest.QuestList[nameIndex].playerDialog[i]);
        //            }
        //        }
        //    }
        //}

        if (GUILayout.Button("Search"))
        {
            Search();
        }

        GUILayout.BeginHorizontal();
        editable = (GUILayout.Toggle(editable, editButtonImage, "Button"));
        GUILayout.Space(650);
        if(GUILayout.Button(deleteButtonImage))
        {
            DeleteQuest();
        }
        GUILayout.EndHorizontal();
    }

    protected void Search()
    {
        if (searchIndexMethod)
        {
            for (int i = 0; i < Quest.QuestList.Count; i++)
            {
                if (questIndex < (Quest.QuestList.Count) && questIndex >= 0)
                {
                    indexPassed = true;
                }
                else
                {
                    indexPassed = false;
                }
            }
        }
        else if (searchNameMethod)
        {
            for(int i = 0; i < Quest.QuestList.Count; i++)
            {
                if(questName.ToLower() == (Quest.QuestList[i].title.ToLower()))
                {
                    nameIndex = i;
                    namePassed = true;
                }
            }
        }
        
    }

    protected void DeleteQuest()
    {
        if (searchIndexMethod)
        {
            Quest.QuestList.Remove(Quest.QuestList[questIndex]);
        }
        else // searchNameMethod
        {
            Quest.QuestList.Remove(Quest.QuestList[nameIndex]);
        }

    }

    protected void OnDisable()
    {
        QuestMaker.questMaker.Serialize();
    }
}
