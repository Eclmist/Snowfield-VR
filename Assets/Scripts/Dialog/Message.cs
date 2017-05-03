using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Message : MonoBehaviour {

    [SerializeField]
    private string sessionTitle;
    private bool isCleared = false;
    
	// Use this for initialization


    void Start()
    {
        isCleared = false;
    }
	
	// Update is called once per frame
	void Update ()
    {
        // For debugging purposes
		if(Input.GetKeyDown(KeyCode.X))
        {
            DialogManager.Instance.DisplayDialogBox(sessionTitle);
            isCleared = true;
        }

	}

    void OnTriggerEnter(Collider other)
    {
        if(!isCleared)
        {
            DialogManager.Instance.DisplayDialogBox(sessionTitle);
            isCleared = true;
        }
    }
        
        
}
