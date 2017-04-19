using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DialogManager : MonoBehaviour {
    
    
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
    private List<Session> sessionList;
    private Session currentSession;
    private AudioSource audioSource;

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
        sessionList = TaoBaoDialogEditor.Instance.Sessions;

        isTyping = false;
        isShowing = false;
        isOccupied = false;
        Instance = this;
        dialogBox = GameObject.FindWithTag("DialogBox");
        currentText = dialogBox.GetComponentInChildren<Text>();
        

    }



    void Update()
    {
        // Constantly check the status of the dialog box
        dialogBox.SetActive(isShowing);
        HandleDialogBox();            
    }

    public void DisplayDialogBox(string title)
    {
        if(!isOccupied)
        {
            isOccupied = true;
            LoadSessionByTitle(title);
            ShowDialogBox();
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
        bool isFound = false;

        foreach (Session s in sessionList)
        {
            if (s.Title == title)
            {
                currentSession = s;
                isFound = true;
                break;
            }

        }

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

    
    




   













}
