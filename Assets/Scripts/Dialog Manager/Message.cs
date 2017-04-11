using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Message : MonoBehaviour {

    [SerializeField]
    private int index;
    private bool isCleared = false;
    
	// Use this for initialization


    void Start()
    {
        isCleared = false;
    }
	
	// Update is called once per frame
	void Update ()
    {
		
	}

    void OnTriggerEnter(Collider other)
    {
        if(!isCleared)
        {
            DialogManager.Instance.LoadConversationByIndex(index);
            DialogManager.Instance.ShowDialogBox();
            isCleared = true;
        }
    }
        
        
}
