﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System.IO;

public class ItemEditor : EditorWindow {

    Texture2D gameObjectTexture;
    protected GameObject item;

    public static ItemEditor itemEditor;

    [MenuItem("Editors/Texture Generator")]
    protected static void Init()
    {
        itemEditor = (ItemEditor)EditorWindow.GetWindow(typeof(ItemEditor));
        itemEditor.minSize = new Vector2(500, 200);
        itemEditor.Show();
    }

    protected void OnGUI()
    {
        EditorGUILayout.BeginHorizontal();
        item = (GameObject)EditorGUILayout.ObjectField("Item", item, typeof(GameObject), true);

        if (item != null)
        {
            gameObjectTexture = AssetPreview.GetAssetPreview(item);
            GUILayout.Label(gameObjectTexture);

            Repaint();
        }
        EditorGUILayout.EndHorizontal();

        if (GUILayout.Button("Create Texture"))
        {
            CreateTexture();
        }
    }

    protected void CreateTexture()
    {
        byte[] bytes = gameObjectTexture.EncodeToPNG();
        File.WriteAllBytes(Application.dataPath + "/Icon/" + item.name + ".png", bytes);
        Debug.Log(item.name + "'s Texture Created");
        AssetDatabase.Refresh();
    }
}
