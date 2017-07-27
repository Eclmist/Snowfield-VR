using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteractableMessage : MonoBehaviour {

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    private MessageManager.Mail storedMail;

    public MessageManager.Mail StoredMail
    {
        get { return this.storedMail; }
        set { this.storedMail = value; }
    }

    public void SetMessageReference()
    {
        if(storedMail != null)
            MessageManager.Instance.DisplayMail(storedMail);
    }
}
