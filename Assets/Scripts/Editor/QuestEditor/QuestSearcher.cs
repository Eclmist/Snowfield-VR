/* 
 Author : Ivan Leong
 Description : Quest Searcher
*/

using System;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEditor;
using System.Xml;
using System.Xml.Serialization;
using System.Linq;

public class QuestSearcher : EditorWindow {

    static QuestSearcher questSearcher;

    Quest quest;

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


    //-----Editable Variable-----//

    JobType jobItemIndex;

    //-----stanleyReward-----//
    GameObject[] stanleyRewardGO;
    string[] stanleyReward;
    int stanleyRewardIndex = 0;

    //-----ItemType-----//
    string[] itemTypes;
    int itemTypeIndex;

    string[] childTypes;
    int childTypesIndex;

    [MenuItem("Editors/Quest/Quest Searcher")]
    protected static void Init()
    {
        questSearcher = (QuestSearcher)EditorWindow.GetWindow(typeof(QuestSearcher));
        questSearcher.minSize = new Vector2(800, 450);
        questSearcher.Show();
    }

    protected void OnEnable()
    {
        DirectoryInfo directoryInfo = new DirectoryInfo(Application.dataPath + "/Resources/Items");
        FileInfo[] fileinfo = directoryInfo.GetFiles();

        itemTypes = new string[fileinfo.Length];

        for (int i = 0; i < fileinfo.Length; i++)
        {
            itemTypes[i] = Path.GetFileNameWithoutExtension(fileinfo[i].FullName);
        }

        QuestManager.QuestList = QuestFactory.Load(Path.Combine(Application.dataPath, "XML/quests.xml"));

        Debug.Log(QuestManager.QuestList.Count + " quest(s) loaded");

        //----------Load Image----------//
        deleteButtonImage = Resources.Load("Editor/Image/trash") as Texture2D;
        editButtonImage = Resources.Load("Editor/Image/edit.jpg") as Texture2D;
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

        searchIndexMethod = EditorGUILayout.ToggleLeft("Search by Index", searchIndexMethod);
        searchNameMethod = EditorGUILayout.ToggleLeft("Search by Name", searchNameMethod);

        if (searchIndexMethod)
        {
            EditorGUILayout.BeginHorizontal();
            EditorGUILayout.PrefixLabel("Quest Index: ");
            questIndex = EditorGUILayout.IntSlider(questIndex, 0, QuestManager.QuestList.Count - 1);
            EditorGUILayout.EndHorizontal();
        }
        else if(searchNameMethod)
        {
            EditorGUILayout.BeginHorizontal();
            EditorGUILayout.PrefixLabel("Quest Name: ");
            questName = EditorGUILayout.TextArea(questName);
            EditorGUILayout.EndHorizontal();
        }

        if (indexPassed)
        {
            if (!editable)
            {
                EditorGUILayout.LabelField("Quest Index", EditorStyles.boldLabel);
                EditorGUILayout.SelectableLabel(QuestManager.QuestList[questIndex].index.ToString());

                EditorGUILayout.LabelField("Quest for: " + QuestManager.QuestList[questIndex].questJob, EditorStyles.boldLabel);
                EditorGUILayout.LabelField("Required Experience: " + QuestManager.QuestList[questIndex].requiredExperience, EditorStyles.boldLabel);

                EditorGUILayout.LabelField("Quest Title: ", EditorStyles.boldLabel);
                EditorGUILayout.SelectableLabel(QuestManager.QuestList[questIndex].title, EditorStyles.wordWrappedLabel);

                EditorGUILayout.LabelField("Quest Type: ", EditorStyles.boldLabel);
                EditorGUILayout.SelectableLabel(QuestManager.QuestList[questIndex].questType.ToString());

                EditorGUILayout.LabelField("Quest Reward: ", QuestManager.QuestList[questIndex].stanleyReward.ToString());


                for (int i = 0; i < QuestManager.QuestList[questIndex].playerDialog.Length; i++)
                {
                    if (QuestManager.QuestList[questIndex].playerDialog[i] != null)
                    {
                        EditorGUILayout.LabelField("Stanley Dialog: ");
                        EditorGUILayout.SelectableLabel(QuestManager.QuestList[questIndex].playerDialog[i]);
                    }
                }
            }
            else // Editable
            {
                EditorGUILayout.LabelField("Quest Index", EditorStyles.boldLabel);
                EditorGUILayout.SelectableLabel(QuestManager.QuestList[questIndex].index.ToString());

                EditorGUILayout.LabelField("Quest Type: ", EditorStyles.boldLabel);
                EditorGUILayout.SelectableLabel(QuestManager.QuestList[questIndex].questType.ToString());

                QuestManager.QuestList[questIndex].title = EditorGUILayout.TextField("Quest Title", QuestManager.QuestList[questIndex].title);

                QuestManager.QuestList[questIndex].questJob = (JobType)EditorGUILayout.EnumPopup("Jobs", QuestManager.QuestList[questIndex].questJob);
                QuestManager.QuestList[questIndex].requiredExperience = EditorGUILayout.IntField("Required Experience: ", QuestManager.QuestList[questIndex].requiredExperience);

                itemTypeIndex = EditorGUILayout.Popup("Item Types: ", itemTypeIndex, itemTypes);

                DirectoryInfo childDirectoryInfo = new DirectoryInfo(Application.dataPath + "/Resources/Items/" + itemTypes[itemTypeIndex]);
                FileInfo[] childFileInfo = childDirectoryInfo.GetFiles();

                childTypes = new string[childFileInfo.Length];

                for (int i = 0; i < childFileInfo.Length; i++)
                {
                    childTypes[i] = Path.GetFileNameWithoutExtension(childFileInfo[i].FullName);
                }

                childTypesIndex = EditorGUILayout.Popup("Item Category: ", childTypesIndex, childTypes);

                stanleyRewardGO = Resources.LoadAll("Items/" + itemTypes[itemTypeIndex] + "/" + childTypes[childTypesIndex], typeof(GameObject)).Cast<GameObject>().ToArray();

                stanleyReward = new string[stanleyRewardGO.Length];

                for (int i = 0; i < stanleyRewardGO.Length; i++)
                {
                    stanleyReward[i] = stanleyRewardGO[i].name;
                }

                stanleyRewardIndex = EditorGUILayout.Popup("Quest Rewards", stanleyRewardIndex, stanleyReward);

                for (int i = 0; i < QuestManager.QuestList[questIndex].playerDialog.Length; i++)
                {
                    if (QuestManager.QuestList[questIndex].playerDialog[i] != null)
                    {
                        QuestManager.QuestList[questIndex].playerDialog[i] = EditorGUILayout.TextField("Stanley's Dialog ", QuestManager.QuestList[questIndex].playerDialog[i]);
                    }
                }
            }
            
        }

        if (namePassed)
        {
            if (!editable)
            {
                EditorGUILayout.LabelField("Quest Index: ", EditorStyles.boldLabel);
                EditorGUILayout.SelectableLabel(QuestManager.QuestList[nameIndex].index.ToString());

                EditorGUILayout.LabelField("Quest for: " + QuestManager.QuestList[nameIndex].questJob, EditorStyles.boldLabel);
                EditorGUILayout.LabelField("Required Experience: " + QuestManager.QuestList[nameIndex].requiredExperience, EditorStyles.boldLabel);

                EditorGUILayout.LabelField("Quest Title: ", EditorStyles.boldLabel);
                EditorGUILayout.SelectableLabel(QuestManager.QuestList[nameIndex].title, EditorStyles.wordWrappedLabel);

                EditorGUILayout.LabelField("Quest Type: ", EditorStyles.boldLabel);
                EditorGUILayout.SelectableLabel(QuestManager.QuestList[nameIndex].questType.ToString());

                EditorGUILayout.LabelField("Quest Rewards: ", EditorStyles.boldLabel);

                EditorGUILayout.LabelField("Quest Reward: ", QuestManager.QuestList[questIndex].stanleyReward.ToString());

                for (int i = 0; i < QuestManager.QuestList[nameIndex].playerDialog.Length; i++)
                {
                    if (QuestManager.QuestList[nameIndex].playerDialog[i] != null)
                    {
                        EditorGUILayout.LabelField("Stanley Dialog: ");
                        EditorGUILayout.SelectableLabel(QuestManager.QuestList[nameIndex].playerDialog[i]);
                    }
                }
            }
            else // Editable
            {
                EditorGUILayout.LabelField("Quest Index", EditorStyles.boldLabel);
                EditorGUILayout.SelectableLabel(QuestManager.QuestList[nameIndex].index.ToString());

                EditorGUILayout.LabelField("Quest Type: ", EditorStyles.boldLabel);
                EditorGUILayout.SelectableLabel(QuestManager.QuestList[nameIndex].questType.ToString());

                QuestManager.QuestList[nameIndex].title = EditorGUILayout.TextField("Quest Title", QuestManager.QuestList[nameIndex].title);

                QuestManager.QuestList[nameIndex].questJob = (JobType)EditorGUILayout.EnumPopup("Jobs", QuestManager.QuestList[nameIndex].questJob);
                QuestManager.QuestList[nameIndex].requiredExperience = EditorGUILayout.IntField("Required Experience: ", QuestManager.QuestList[nameIndex].requiredExperience);

                itemTypeIndex = EditorGUILayout.Popup("Item Types: ", itemTypeIndex, itemTypes);

                DirectoryInfo childDirectoryInfo = new DirectoryInfo(Application.dataPath + "/Resources/Items/" + itemTypes[itemTypeIndex]);
                FileInfo[] childFileInfo = childDirectoryInfo.GetFiles();

                childTypes = new string[childFileInfo.Length];

                for (int i = 0; i < childFileInfo.Length; i++)
                {
                    childTypes[i] = Path.GetFileNameWithoutExtension(childFileInfo[i].FullName);
                }

                childTypesIndex = EditorGUILayout.Popup("Item Category: ", childTypesIndex, childTypes);

                stanleyRewardGO = Resources.LoadAll("Items/" + itemTypes[itemTypeIndex] + "/" + childTypes[childTypesIndex], typeof(GameObject)).Cast<GameObject>().ToArray();

                stanleyReward = new string[stanleyRewardGO.Length];

                for (int i = 0; i < stanleyRewardGO.Length; i++)
                {
                    stanleyReward[i] = stanleyRewardGO[i].name;
                }

                stanleyRewardIndex = EditorGUILayout.Popup("Quest Rewards", stanleyRewardIndex, stanleyReward);

                for (int i = 0; i < QuestManager.QuestList[nameIndex].playerDialog.Length; i++)
                {
                    if (QuestManager.QuestList[nameIndex].playerDialog[i] != null)
                    {
                        QuestManager.QuestList[nameIndex].playerDialog[i] = EditorGUILayout.TextField("Stanley's Dialog ", QuestManager.QuestList[nameIndex].playerDialog[i]);
                    }
                }
            }
        }
            

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
            for (int i = 0; i < QuestManager.QuestList.Count; i++)
            {
                if (questIndex < (QuestManager.QuestList.Count) && questIndex >= 0)
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
            for(int i = 0; i < QuestManager.QuestList.Count; i++)
            {
                if(questName.ToLower() == (QuestManager.QuestList[i].title.ToLower()))
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
            QuestManager.QuestList.Remove(QuestManager.QuestList[questIndex]);
        }
        else // searchNameMethod
        {
            QuestManager.QuestList.Remove(QuestManager.QuestList[nameIndex]);
        }

    }

    protected void OnDisable()
    {
        QuestFactory.Save(Path.Combine(Application.dataPath, "XML/quests.xml"));
    }
}
