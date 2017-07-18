using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[System.Serializable]
public class MailManager : MonoBehaviour {


    [System.Serializable]
    public class Mail
    {
        [SerializeField]private string message;
        [SerializeField] private AudioClip clip;
        [SerializeField]private bool isRead;

        public Mail(string message, AudioClip clip)
        {
            this.message = message;
            this.clip = clip;
            this.isRead = false;
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

    public static MailManager Instance;

    [SerializeField] private GameObject notifier;
    [SerializeField] private Text count;
    
    [SerializeField] private Animator animator;
    [SerializeField] private List<Mail> inbox = new List<Mail>();
    [SerializeField] private int totalUnreadMails;
    

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
    private void Start()
    {
        notifier.SetActive(true);
    }

    private void Update()
    {
        if(Input.GetKeyDown(KeyCode.Alpha1))
        {
            SendMail("This is a test mail",null);
        }
        else if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            if(inbox.Count > 0)
                ReadMail(inbox[0]);
        }

        HandleMailEvents();
        UpdateUnreadCounter();
        HandleUI();


    }

    private void HandleUI()
    {
        count.text = totalUnreadMails.ToString();
        animator.SetBool("haveMail", totalUnreadMails > 0);
    }

    


    // Send a message to the player's inbox
    public void SendMail(string message, AudioClip clip)
    {
        inbox.Add(new Mail(message,clip));
    }


    
    public void ReadMail(Mail mail)
    {
        DisplayMail(mail);
        if(!mail.IsRead)
        {
            //play clip

            
        }

        mail.IsRead = true;
        
        
        

    }

    public void DisplayMail(Mail mail)
    {
        
    }

    public void DisplayInbox()
    {

    }


    private void UpdateUnreadCounter()
    {
        int count = 0;

        foreach(Mail m in inbox)
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
