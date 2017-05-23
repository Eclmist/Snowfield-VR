using System;using System.Collections;using System.Collections.Generic;using UnityEngine;using UnityEngine.UI;

public class DialogManager : MonoBehaviour
{    public abstract class DialogRequest
    {
        public abstract void DoAction();

        public virtual ICanTalk Item
        {
            get;
            set;
        }
    }    public class DialogRequest<T> : DialogRequest where T : ICanTalk
    {
        private T item;
        private Action<T> callBack;

        public DialogRequest(Action<T> _callBack, T _sth)
        {
            item = _sth;
            callBack = _callBack;
        }

        public override void DoAction()
        {
            callBack(item);
        }

        public override ICanTalk Item
        {
            get
            {
                return item;
            }
        }    }    public static DialogManager Instance; // Static instance of dialog manager  [Range(0, 3)]

    [SerializeField]
    private float minDelay;

    [Range(0, 3)]
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
    private List<Session> sessionList;
    private Queue<DialogRequest> dialogRequestQueue;

    private bool isShowing;     // Status of dialog box
    private bool isTyping;      // Check if  a co routine is currently running
    private bool isOccupied;    private void Awake()
    {
        Instance = this;
    }

    private void Start()
    {
        audioSource = GetComponent<AudioSource>();

        isTyping = false;
        isShowing = false;
        isOccupied = false;
        Instance = this;
        dialogBox = GameObject.FindWithTag("DialogBox");
        currentText = dialogBox.GetComponentInChildren<Text>();
        sessionList = TaoBaoDialogEditor.Instance.Sessions;
    }    private void Update()
    {
        // Constantly check the status of the dialog box
        dialogBox.SetActive(isShowing);
        HandleDialogBox();

        if (dialogRequestQueue.Count > 0)
        {
            DisplayDialogBox(dialogRequestQueue.Peek().Item);
        }    }

    public void AddDialog<T>(Action<T> action, T item) where T : ICanTalk
    {
        DialogRequest c = new DialogRequest<T>(action, item);
        dialogRequestQueue.Enqueue(c);    }

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
    }    public void DisplayDialogBox(string title)
    {
        if (!isOccupied)
        {
            isOccupied = true;
            LoadSessionByTitle(title);
            ShowDialogBox();
            Message.Instance.IncomingRequest = false;
        }    }

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
        }    }    private void ShowDialogBox()
    {
        isShowing = true;
    }

    public void HideDialogBox()
    {
        isShowing = false;
        isOccupied = false;
    }    private void LoadSessionByTitle(string title)
    {
        foreach (Session s in sessionList)
        {
            if (s.Title == title)
            {
                currentSession = s;
                break;
            }        }

        lineItor = 0;    }    public void SetCurrentSession(Session s)
    {
        currentSession = s;
        lineItor = 0;
    }    public void ToggleDialogBox()
    {
        isShowing = !isShowing;
        isOccupied = !isOccupied;
    }    // type writing effect
    private IEnumerator WriteTextLikeATypeWriter()
    {
        isTyping = true;

        for (int i = 0; i <= currentLine.Length; i++)
        {
            currentText.text = currentLine.Substring(0, i);
            yield return new WaitForSeconds(0.1f - textSpeed);
        }

        float delay = UnityEngine.Random.Range(-minDelay, maxDelay);
        yield return new WaitForSeconds(Mathf.Clamp01(delay));
        isTyping = false;
    }

    // Handles how messages are being displayed in the dialog box (eg. order of messages etc)
    private void HandleDialogBox()
    {        // If the dialog box is currently active
        if (isShowing)
        {
            if (!isTyping)
            {
                if (lineItor >= currentSession.Lines.Count)
                {
                    ToggleDialogBox();
                    dialogRequestQueue.Peek().DoAction();
                    dialogRequestQueue.Dequeue();
                    lineItor = 0;                }
                else
                {
                    ProceedMessage();
                }            }
        }
    }    private void ProceedMessage()
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
        if (isTyping)
        {            if (UnityEngine.Random.value > skipPercentChance)
            {
                StopCoroutine(typeWriter);
                audioSource.Stop();
                isTyping = false;
            }
        }    }

    private void SyncAudioWithLine()
    {
        //TO DO
    }  //----------------------------------------------------------------------------------------------------------//
}