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
    private string[] lines;     // Split text resource into lines
    private Text currentText; // Text that is currently displayed
    private string currentLine; // The line that is currently being processed
    private IEnumerator typeWriter; // Coroutine that achieves the typewriter effect
    private int iterator = 0; // To iterate through conversations

    private TextAsset conversations; // Text resource containing all conversation information
    private List<string> retrievedLines; // List to store lines that belong to a particular conversation
    //private List<AudioClip> audioClips; // The audio for each line
    private List<string> messageOrder;  // List to store the messages in order
    private List<List<AudioClip>> audioClips;

    private bool isShowing;     // Status of dialog box
    private bool isTyping;      // Check if  a co routine is currently running
    
    

    void Awake()
    {
        // Load the text resource
        conversations = Resources.Load("testComm") as TextAsset;

        if (conversations == null)
            Debug.LogError("Conversations resource missing!");
        else
            lines = conversations.text.Split('\n');


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
  
    // Search through the text resource to find the specified conversation
    // Accessed by outside gameobjects
    public void LoadConversationByIndex(int conversationIndex)
    {
        retrievedLines = new List<string>();
        ClearPreviousConversations();
        bool storeLine = false;

        foreach(string line in lines)
        {
            // ***the following series of operations are in the RIGHT order***

            if (line.Contains("--End") && retrievedLines.Count > 0)
                break;

            // To stop storing lines
            if (line.Contains("--End"))
                storeLine = false;

            // store the current line
            if (storeLine == true)
                retrievedLines.Add(line);

            // This check is at the end to ensure that we only store the line after this header line
            if (line.Contains("--Session " + conversationIndex))
                storeLine = true;
       
        }

        SortCharacterAndSpeech(retrievedLines);
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
                if(iterator >= messageOrder.Count)
                {
                    ToggleDialogBox();
                    // Reset the iterator
                    iterator = 0;
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
        currentLine = messageOrder[iterator];
        // Store the coroutine to stop later
        typeWriter = WriteTextLikeATypeWriter();
        StartCoroutine(typeWriter);
        iterator++;
    }

    // Core function to set up conversation
    private void SortCharacterAndSpeech(List<string> retrievedConversation)
    {
        messageOrder = new List<string>();

        foreach (string m in retrievedConversation)
        {
            messageOrder.Add(m);
        }
    }


    // Removes previously added items in all lists
    private void ClearPreviousConversations()
    {
        if (retrievedLines != null)
            retrievedLines.Clear();

        if (messageOrder != null)
            messageOrder.Clear();
    }














}
