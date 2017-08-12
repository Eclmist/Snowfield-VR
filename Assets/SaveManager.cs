using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SaveManager : MonoBehaviour {

    public static SaveManager Instance;

    [SerializeField]
    protected List<GameObject> serializeObject = new List<GameObject>();

	public void Awake()
    {
        if (!Instance)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
            Debug.Log("SaveManager already exist");
        }
    }

    protected void Update()
    {
        if (Input.GetKeyDown(KeyCode.S))
        {
            Save();
        }
    }

    public void Save()
    {
        foreach(GameObject obj in serializeObject)
        {
            ICanSerialize ser = obj.GetComponent<ICanSerialize>();
            ser.Save();
            Debug.Log("load");
        }
    }
}


