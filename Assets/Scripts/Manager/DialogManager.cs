using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DialogManager : MonoBehaviour {
    
    public struct DialogRequest
    {
        public ICanTalk smth;
        public AI ai;

        public DialogRequest(ICanTalk canTalk, AI aiRequester)
        {
            smth = canTalk;
            ai = aiRequester;
        }

    }

    
    public static DialogManager Instance; // Static instance of dialog manager
    
    [Range(0,3)]
    [SerializeField]
    private float minDelay;
    [Range(0,3)]
    [SerializeField]
    private float maxDelay;
    [Range(0, 1)]
    [SerializeField]
    private float skipPercentChance;

    [Range(0, 0.1F)]
    [SerializeField]
    private float textSpeed;    // Interval between text characters

    private GameObject dialogBox; // The dialog box containing the text
    private Text currentText; // Text that is currently displayed
    private string currentLine; // The line that is currently being processed
    private IEnumerator typeWriter; // Coroutine that achieves the typewriter effect
    private int lineItor = 0; // To iterate through lines in a session
    private Session currentSession;
    private AudioSource audioSource;
    List<Session> sessionList;
    private Queue<DialogRequest> dialogRequestQueue;

    private bool isShowing;     // Status of dialog box
    private bool isTyping;      // Check if  a co routine is currently running
    private bool isOccupied;    
    

    void Awake()
    {
        Instance = this;
    }

    void Start()
    {
        audioSource = GetComponent<AudioSource>();

        isTyping = false;
        isShowing = false;
        isOccupied = false;
        Instance = this;
        dialogBox = GameObject.FindWithTag("DialogBox");
        currentText = dialogBox.GetComponentInChildren<Text>();
        sessionList = TaoBaoDialogEditor.Instance.Sessions;

    }



    void Update()
    {
        // Constantly check the status of the dialog box
        dialogBox.SetActive(isShowing);
        HandleDialogBox();

        if(dialogRequestQueue.Count > 0)
        {
            DisplayDialogBox(dialogRequestQueue.Peek().smth);
        }
        
    }

    public Queue<DialogRequest> DialogRequestQueue
    {
        get { return this.dialogRequestQueue; }
    }

    public void AddToQueue(DialogRequest dr)
    {
        dialogRequestQueue.Enqueue(dr);
    }

    public bool IsOccupied
    {
        get { return this.isOccupied; }
    }

    
    public void DisplayDialogBox(string title)
    {
        if(!isOccupied)
        {
            isOccupied = true;
            LoadSessionByTitle(title);
            ShowDialogBox();
            Message.Instance.IncomingRequest = false;
        }
        
    }

    public void DisplayDialogBox(ICanTalk talkableStuff)
    {
        if (!isOccupied)
        {
            isOccupied = true;
            currentSession = talkableStuff.Session;
            ShowDialogBox();
            Message.Instance.IncomingRequest = false;
        }
    }

    public void DisplayDialogBox()
    {
        if (!isOccupied && currentSession != null)
        {
            isOccupied = true;
            ShowDialogBox();
            Message.Instance.IncomingRequest = false;
        }

    }



    private void ShowDialogBox()
    {
        isShowing = true;
    }

    public void HideDialogBox()
    {
        isShowing = false;
        isOccupied = false;
    }
 

    private void LoadSessionByTitle(string title)
    {
        foreach (Session s in sessionList)
        {
            if (s.Title == title)
            {
                currentSession = s;
                break;
            }

        }

        lineItor = 0;

    }


    public void SetCurrentSession(Session s)
    {
        currentSession = s;
        lineItor = 0;
    }
   


    public void ToggleDialogBox()
    {
        isShowing = !isShowing;
        isOccupied = !isOccupied;
    }



    // type writing effect
    private IEnumerator WriteTextLikeATypeWriter()
    {
        isTyping = true;

        for (int i = 0; i <= currentLine.Length; i++)
        {
            currentText.text = currentLine.Substring(0, i);
            yield return new WaitForSeconds(0.1f - textSpeed);
        }

        float delay = Random.Range(-minDelay, maxDelay);
        yield return new WaitForSeconds(Mathf.Clamp01(delay));
        isTyping = false;
    }

    // Handles how messages are being displayed in the dialog box (eg. order of messages etc)
    private void HandleDialogBox()
    {

        // If the dialog box is currently active
        if (isShowing)
        {
            if (!isTyping)
            {
                if(lineItor >= currentSession.Lines.Count)
                {
                    ToggleDialogBox();
                    lineItor = 0;
                    
                }
                else
                {
                    ProceedMessage();
                }           
            }
        }
    }
    
    private void ProceedMessage()
    {
        currentLine = currentSession.Lines[lineItor].Message;
        audioSource.PlayOneShot(currentSession.Lines[lineItor].Clip);

        // Store the coroutine to stop later
        typeWriter = WriteTextLikeATypeWriter();
        StartCoroutine(typeWriter);
        lineItor++;
    }

    private void RandomlyCutMessageAndSkip()
    {
        if(isTyping)
        {
           if(Random.value > skipPercentChance)
            {
                StopCoroutine(typeWriter);
                audioSource.Stop();
                isTyping = false;
            }
        }
 
    }

    private void SyncAudioWithLine()
    {
        //TO DO
    }

    //----------------------------------------------------------------------------------------------------------//
    




   













}
