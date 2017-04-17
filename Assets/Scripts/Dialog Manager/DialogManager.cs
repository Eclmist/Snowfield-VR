using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DialogManager : MonoBehaviour {
    
    
    public static DialogManager Instance; // Static instance of dialog manager

    [Range(0, 0.1F)]
    [SerializeField]
    private float textSpeed;    // Interval between text characters


    private GameObject dialogBox; // The dialog box containing the text
    private Text currentText; // Text that is currently displayed
    private string currentLine; // The line that is currently being processed
    private IEnumerator typeWriter; // Coroutine that achieves the typewriter effect
    private int lineItor = 0; // To iterate through lines in a session
    private List<TaoBaoDialogEditor.Session> sessionList;
    private TaoBaoDialogEditor.Session currentSession;
    private AudioSource audioSource;

    private bool isShowing;     // Status of dialog box
    private bool isTyping;      // Check if  a co routine is currently running
    

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

    public void ShowDialogBox()
    {
        isShowing = true;
    }

    public void HideDialogBox()
    {
        isShowing = false;
    }
 

    public void LoadSessionByTitle(string title)
    {
        bool isFound = false;

        foreach (TaoBaoDialogEditor.Session s in sessionList)
        {
            if (s.title == title)
            {
                currentSession = s;
                isFound = true;
                break;
            }

        }

        lineItor = 0;
        Debug.Log(isFound);

    }




    public void ToggleDialogBox()
    {
        isShowing = !isShowing;
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
                if(lineItor >= currentSession.lines.Count)
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
        currentLine = currentSession.lines[lineItor].message;
        audioSource.PlayOneShot(currentSession.lines[lineItor].clip);

        // Store the coroutine to stop later
        typeWriter = WriteTextLikeATypeWriter();
        StartCoroutine(typeWriter);
        lineItor++;
    }




   













}
