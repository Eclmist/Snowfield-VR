using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityEditorInternal;
using System.IO;

[CustomEditor(typeof(ItemManager))]
public class ItemManagerEditor : Editor {

    ItemManager instance;

    private ReorderableList relist;

    void OnEnable()
    {
        instance = (ItemManager)target;

        relist = new ReorderableList(serializedObject, serializedObject.FindProperty("itemDataList"), true, true, true, true);
        
        relist.drawHeaderCallback = (Rect rect) =>
        {

            Rect IDRect = rect;
            IDRect.width = 40;
            IDRect.x = 32;
            EditorGUI.LabelField(IDRect, "ID");

            Rect PrefabRect = rect;
            PrefabRect.width = 150;
            PrefabRect.x = 93;
            EditorGUI.LabelField(PrefabRect, "Prefab");

            Rect SpriteRect = rect;
            SpriteRect.width = 150;
            SpriteRect.x = 260;
            EditorGUI.LabelField(SpriteRect,"Sprite");

            Rect StackRect = rect;
            StackRect.width = 150;
            StackRect.x = 430;
            EditorGUI.LabelField(StackRect, "StackSize");

            Rect JobRect = rect;
            JobRect.width = 150;
            JobRect.x = 500;
            EditorGUI.LabelField(JobRect, "JobType");

            Rect CostRect = rect;
            CostRect.width = 150;
            CostRect.x = 680;
            EditorGUI.LabelField(CostRect, "Cost");

            Rect UnlockedRect = rect;
            UnlockedRect.width = 80;
            UnlockedRect.x = rect.x + rect.width - 70 ;
            EditorGUI.LabelField(UnlockedRect, "Unlocked");

        };

        relist.drawElementCallback =
    (Rect rect, int index, bool isActive, bool isFocused) =>
    {
       

        var element = relist.serializedProperty.GetArrayElementAtIndex(index);
        rect.y += 2;

        EditorGUI.BeginDisabledGroup(true);
        EditorGUI.PropertyField(
            new Rect(rect.x, rect.y, 40, EditorGUIUtility.singleLineHeight),
            element.FindPropertyRelative("ID"), GUIContent.none);
        EditorGUI.EndDisabledGroup();

        EditorGUI.PropertyField(
            new Rect(rect.x + 60, rect.y, 150 , EditorGUIUtility.singleLineHeight),
            element.FindPropertyRelative("prefab"), GUIContent.none);

        EditorGUI.PropertyField(
           new Rect(rect.x + 230, rect.y,  150 , EditorGUIUtility.singleLineHeight),
           element.FindPropertyRelative("icon"), GUIContent.none);

        EditorGUI.PropertyField(
           new Rect(rect.x + 420, rect.y, rect.xMin, EditorGUIUtility.singleLineHeight),
           element.FindPropertyRelative("maxStackSize"), GUIContent.none);

        EditorGUI.PropertyField(
           new Rect(rect.x + 470, rect.y, 150, EditorGUIUtility.singleLineHeight),
           element.FindPropertyRelative("jobType"), GUIContent.none);

        EditorGUI.PropertyField(
           new Rect(rect.x + 650, rect.y, 60, EditorGUIUtility.singleLineHeight),
           element.FindPropertyRelative("cost"), GUIContent.none);


        EditorGUI.PropertyField(
            new Rect(rect.x + rect.width - 50, rect.y, 20, EditorGUIUtility.singleLineHeight),
            element.FindPropertyRelative("levelUnlocked"),GUIContent.none);



    };


        //relist.drawHeaderCallback += DrawHeader;
        //relist.drawElementCallback += DrawElement;

        //relist.onAddCallback += AddItem;
        //relist.onRemoveCallback += RemoveItem;

         
        relist.onCanRemoveCallback = (ReorderableList l) => {
            return l.count > 1;
        };

        relist.onRemoveCallback = (ReorderableList l) => {
            if (EditorUtility.DisplayDialog("Deleting item!",
                "Are you sure you want to delete the item?", "HELL YEAH", "Nah"))
            {
                ReorderableList.defaultBehaviours.DoRemoveButton(l);
            }
        };

        
    }

    void OnDisable()
    {

    }

    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();

        EditorGUILayout.BeginVertical("Box");
        EditorGUILayout.LabelField("Register your items below : \n\n1)Drag a prefab into the field. \n2)Define item properties\n3)Click 'Generate unique ID' ");
        EditorGUILayout.EndVertical();


        serializedObject.Update();
        relist.DoLayoutList();
        serializedObject.ApplyModifiedProperties();


        if (GUILayout.Button("\nGenerate unique ID\n"))
        {

            StreamWriter file = new StreamWriter(instance.path);

            for (int x = 0; x < relist.serializedProperty.arraySize; x++)
            {
                relist.serializedProperty.GetArrayElementAtIndex(x).FindPropertyRelative("ID").intValue = x;

                string prefabName;

                GameObject g  = relist.serializedProperty.GetArrayElementAtIndex(x).FindPropertyRelative("prefab").objectReferenceValue as GameObject;
                if (g == null)
                    prefabName = "NO ITEM ASSIGNED";
                else
                {
                    prefabName = g.name;
                }
                    

                    string line = x + "\t" + prefabName;
                   

                file.WriteLine(line);
                g.GetComponent<GenericItem>().ItemID = x;
                
            }

            file.Close();

            relist.DoLayoutList();
            serializedObject.ApplyModifiedProperties();
            AssetDatabase.Refresh();
            
            
            
            

        }




    }





}
