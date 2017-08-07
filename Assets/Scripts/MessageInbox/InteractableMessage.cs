using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InteractableMessage : MonoBehaviour {

	[SerializeField]
    private Text title;

    private MessageManager.Mail storedMail;

    // Use this for initialization
    void Start () {

        //if (title.text.Length >= 20)
        //    title.text = storedMail.Title.Substring(0, 15) + "...";
        //else
        //    title.text = storedMail.Title;

    }

	private void Update()
	{
		if (Input.GetKeyDown(KeyCode.X))
			SetMessageReference();
	}

	public void CreateMail(MessageManager.Mail mail)
	{
		storedMail = mail;

		if (mail.Title.Length >= 20)
			title.text = mail.Title.Substring(0, 15) + "...";
		else
			title.text = storedMail.Title;
	}



    public MessageManager.Mail StoredMail
    {
        get { return this.storedMail; }
        set { this.storedMail = value; }
    }

    public void SetMessageReference()
    {
        if(storedMail != null)
            MessageManager.Instance.DisplayMail(storedMail);

		Debug.Log("you are viewing me");
    }
}
