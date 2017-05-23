using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Message : MonoBehaviour {

    public static Message Instance;

  
    private bool isCleared = false;
    private bool incomingRequest;


    //public bool IncomingRequest
    //{
    //    get { return this.incomingRequest; }
    //    set { this.incomingRequest = value; }
    //}




    void Awake()
    {
        Instance = this;
    }

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
        if(!isCleared && incomingRequest && other.gameObject.tag == "Player")
        {
            incomingRequest = false;  
            DialogManager.Instance.DisplayDialogBox();
            isCleared = true;
        }
    }
        
        
}
