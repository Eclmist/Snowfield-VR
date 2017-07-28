using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TextSpawnerManager : MonoBehaviour {

    [SerializeField]
    private SpawnedText textPrefab;
    [SerializeField]
    private Vector3 offset = new Vector3(0f,0.2f,0f);


    public static TextSpawnerManager Instance;

    void Awake()
    {
        Instance = this;
    }
	
    public void SpawnText(string text, Color color, Transform t, Collider c)
    {
        Debug.Log("spawned");
        SpawnedText st = Instantiate(textPrefab, (t.position + t.up * (c.bounds.size.y) + offset), t.rotation);
        Debug.Log(c.bounds.extents.y);
        st.SetText(text);
        st.SetColor(color);
    }

    public void SpawnText(string text, Color color, Transform t, Collider c, int scale)
    {
        Debug.Log("spawned");
        SpawnedText st = Instantiate(textPrefab, (t.position + t.up * (c.bounds.size.y) + offset), t.rotation);
        st.transform.localScale *= scale;
        Debug.Log(c.bounds.extents.y);
        st.SetText(text);
        st.SetColor(color);
    }





}
