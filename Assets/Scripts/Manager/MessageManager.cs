using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


[System.Serializable]
public class MessageManager : MonoBehaviour
{


    [System.Serializable]
    public class Mail
    {
        [SerializeField]
        private string title;
        [SerializeField][TextArea]
        private string message;
        [SerializeField]
        private AudioClip clip;
        private bool isRead;

        public Mail(string title, string message, AudioClip clip)
        {
            this.title = title;
            this.message = message;
            this.clip = clip;
            this.isRead = false;
        }

        public string Title
        {
            get { return this.title; }
            set { this.title = value; }
        }


        public string Message
        {
            get { return this.message; }
            set { this.message = value; }
        }

        public AudioClip Clip
        {
            get { return this.clip; }
            set { this.clip = value; }
        }



        public bool IsRead
        {
            get { return this.isRead; }
            set { this.isRead = value; }
        }

    }

    public static MessageManager Instance;

    
    [SerializeField] private GridLayoutGroup glp;
    [SerializeField] private Text messageTitle;
    [SerializeField] private Text messageBody;
    [SerializeField] private InteractableMessage interactableMesssage;
    [SerializeField] private List<Mail> inbox = new List<Mail>();
    [SerializeField] private int totalUnreadMails;
    [SerializeField] private AudioClip newMailSound;


    private Mail currentlyDisplayedMail;

    public List<Mail> Inbox
    {
        get { return this.inbox; }
    }

    public int TotalUnreadMails
    {
        get { return this.totalUnreadMails; }
    }

    public int TotalMails
    {
        get { return this.inbox.Count; }
    }


    private void Awake()
    {
        Instance = this;
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            SendMail("The adventure begins","Welcome to Snowfield!", null);
        }
        else if (Input.GetKeyDown(KeyCode.Alpha2))
        {
			SendMail("Chen Xiang", "Do spell structure", null);
		}
		else if (Input.GetKeyDown(KeyCode.Alpha3))
		{
			SendMail("Third", "Third", null);
		}

		HandleMailEvents();
        UpdateUnreadCounter();

    }
  
    // Send a message to the player's inbox
    public void SendMail(string title, string message, AudioClip clip)
    {

        Mail temp = new Mail(title,message, clip);
        inbox.Add(temp);

        if (interactableMesssage)
        {
            Instantiate(interactableMesssage, glp.transform, false).CreateMail(temp);
        }

        if (newMailSound)
            AudioSource.PlayClipAtPoint(newMailSound,Player.Instance.transform.position,0.2f);

    }



    public void ReadMail(Mail mail)
    {
        DisplayMail(mail);
        if (!mail.IsRead)
        {
            //play clip

        }

        mail.IsRead = true;




    }

    // Select the mail and display it on the main message view
    public void DisplayMail(Mail mail)
    {
		if(messageBody)
			messageBody.text = mail.Message;

		if (messageTitle)
			messageTitle.text = mail.Title;

		//Debug.Log(messageTitle == null);

	}

    public void DisplayInbox()
    {

    }


    private void UpdateUnreadCounter()
    {
        int count = 0;

        foreach (Mail m in inbox)
        {
            if (!m.IsRead)
                count++;
        }

        totalUnreadMails = count;
    }


    // Determines when to send mails throughout the game
    private void HandleMailEvents()
    {

    }


















}

