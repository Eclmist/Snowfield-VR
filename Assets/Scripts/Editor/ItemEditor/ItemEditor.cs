using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System.IO;

public class ItemEditor : EditorWindow {

    Texture2D gameObjectTexture;

    protected string[] types;
    protected int typesIndex;

    protected string[] childTypes;
    protected int childTypesIndex;

    protected GameObject item;

    public static ItemEditor itemEditor;

    [MenuItem("Editors/Item/Item Editor")]
    protected static void Init()
    {
        itemEditor = (ItemEditor)EditorWindow.GetWindow(typeof(ItemEditor));
        itemEditor.minSize = new Vector2(800, 400);
        itemEditor.Show();
    }

    protected void OnEnable()
    {
        DirectoryInfo directoryInfo = new DirectoryInfo(Application.dataPath + "/Resources/Items");
        FileInfo[] fileinfo = directoryInfo.GetFiles();

        types = new string[fileinfo.Length];

        for (int i = 0; i < fileinfo.Length; i++)
        {
            types[i] = Path.GetFileNameWithoutExtension(fileinfo[i].FullName);
        }
    }

    protected void OnGUI()
    {

        EditorGUILayout.BeginHorizontal();
        item = (GameObject)EditorGUILayout.ObjectField("Item", item, typeof(GameObject), true);

        if(item != null)
        {
            gameObjectTexture = AssetPreview.GetAssetPreview(item);
            GUILayout.Label(gameObjectTexture);

            Repaint();
        }
        EditorGUILayout.EndHorizontal();

        typesIndex = EditorGUILayout.Popup("Item types: ", typesIndex, types);

        DirectoryInfo directoryInfo = new DirectoryInfo(Application.dataPath + "/Resources/Items/" + types[typesIndex]);
        FileInfo[] fileinfo = directoryInfo.GetFiles();

        childTypes = new string[fileinfo.Length];

        for(int i = 0; i < fileinfo.Length; i++)
        {
            childTypes[i] = Path.GetFileNameWithoutExtension(fileinfo[i].FullName);
        }

        childTypesIndex = EditorGUILayout.Popup("Item Category: ", childTypesIndex, childTypes);

        if (GUILayout.Button("Add GameObject"))
        {
            AddGameObject();
        }
    }

    protected void AddGameObject()
    {
        Object prefab = PrefabUtility.CreateEmptyPrefab("Assets/Resources/Items/" + types[typesIndex] + "/" + childTypes[childTypesIndex] + "/" + item.name + ".prefab");
        PrefabUtility.ReplacePrefab(item, prefab, ReplacePrefabOptions.ConnectToPrefab);
        
    }
}
