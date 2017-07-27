using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using Edwon.VR.Input;
using System;

namespace Edwon.VR.Gesture
{
    [RequireComponent(typeof(CanvasRenderer))]
    [RequireComponent(typeof(LaserPointerInputModule))]
    public class VRGestureUI : MonoBehaviour
    {
        [Tooltip("the animation delay as the lists of gestures and neural nets pop in")]
        public float buttonListAnimationDelay;

        VRGestureRig rig;
        VRGestureSettings gestureSettings;

        bool uiVisible;

        [HideInInspector]
        public Handedness menuHandedness;
        VRGestureUIPanelManager panelManager;
        Transform vrMenuHand; // the hand to attach the hand ui to
        Transform vrHandUIPanel; // the actual ui
        Transform vrCam;
        public VRGestureGallery vrGestureGallery;
        public float offsetZ;

        //public VRGestureManager VRGestureManagerInstance; // the VRGestureManager script we want to interact with
        public RectTransform mainMenu; // the top level transform of the main menu
        public RectTransform gesturesMenu; // the top level transform of the gesturesMenu where we will generate gesture buttons'
        public RectTransform recordingMenu;
        public RectTransform selectNeuralNetMenu; // the top level transform of the select neural net menu where we will generate buttons
        public RectTransform detectMenu;
        public GameObject neuralNetButtonPrefab;
        public GameObject gestureButtonPrefab;

        // PARENT
        Canvas rootCanvas; // the canvas on the main VRGestureUI object

        // GESTURES MENU
        private List<Button> gestureButtons;

        // NEURAL NETS MENU
        public CanvasRenderer newNeuralNetButton;

        // NEW GESTURE SETTINGS MENU
        public Button singleHandedButton;
        public Button doubleHandedButton;

        // RECORDING MENU
        [Tooltip("the now recording indicator in the recording menu")]
        public Text nowRecordingLabel;
        public Image nowRecordingBackground;
        [Tooltip("the label that tells you what gesture your recording currently")]
        public Text nowRecordingGestureLabel;
        [Tooltip("the label that tells you the handedness of the gesture")]
        public Text nowRecordingHandednessLabel;
        [Tooltip("the label that tells you how many examples you've recorded")]
        public Text nowRecordingTotalExamplesLabel;

        // EDITING MENU
        [Tooltip("the label that tells you what gesture your editing currently")]
        public Text nowEditingGestureLabel;
        [Tooltip("the button that begins delete gesture in the Recording Menu")]
        public Button deleteGestureButton;

        // DELETE CONFIRM MENU
        public Text deleteGestureConfirmLabel;
        [Tooltip("the button that actually deletes a gesture in the Delete Confirm Menu")]
        public Button deleteGestureConfirmButton;

        // SELECT NEURAL NET MENU
        [Tooltip("the panel of the Select Neural Net Menu")]
        public RectTransform neuralNetTitle;
        List<Button> neuralNetButtons;

        // DETECT MENU
        private Slider thresholdSlider;
        private Text thresholdLog;

        // TRAINING MENU
        [Tooltip("the text feedback for the currently training neural net")]
        public Text neuralNetTraining;

        // default settings
        private Vector3 buttonRectScale; // new Vector3(0.6666f, 1, 0.2f);

        void Start()
        {
            panelManager = GetComponentInChildren<VRGestureUIPanelManager>();

            rootCanvas = GetComponent<Canvas>();
            vrHandUIPanel = transform.Find("Panels");
            
            // start with hand UI visible
            uiVisible = true;
            Utils.ToggleCanvasGroup(panelManager.parentCanvasGroup, uiVisible);

            buttonRectScale = new Vector3(0.6666f, 1, 0.2f);

            // get vr player hand and camera
            rig = VRGestureRig.GetPlayerRig(gestureSettings.playerID);
            menuHandedness = (rig.mainHand == Handedness.Left) ? Handedness.Right : Handedness.Left;
            Handedness oppositeHand = rig.mainHand == Handedness.Left ? Handedness.Right : Handedness.Left;
            vrMenuHand = rig.GetHand(menuHandedness);
            vrCam = rig.head;
      
            GenerateGesturesMenu();
            StartCoroutine (GenerateNeuralNetMenuButtons());

            if (!gestureSettings.beginInDetectMode)
            {
                panelManager.FocusPanel("Select Neural Net Menu");
            }
            else
            {
                panelManager.FocusPanel("Detect Menu");
            }
        }

        void Update()
        {
            // if press Button1 on menu hand toggle menu on off
            Handedness oppositeHand = rig.mainHand == Handedness.Left ? Handedness.Right : Handedness.Left;
            if (rig.GetInput(oppositeHand) != null && rig.GetInput(oppositeHand).GetButtonDown(InputOptions.Button.Button1))
                ToggleVRGestureUI();

            Vector3 handToCamVector = vrCam.position - vrMenuHand.position;
            vrHandUIPanel.position = vrMenuHand.position + (offsetZ * handToCamVector);
            if (-handToCamVector != Vector3.zero)
                vrHandUIPanel.rotation = Quaternion.LookRotation(-handToCamVector, Vector3.up);

            if (rig.uiState == VRGestureUIState.Detecting)
                UpdateConfidenceThresholdUI();

            UpdateCurrentNeuralNetworkText();
            UpdateNowRecordingStatus();

        }

        // toggles this UI's visibility on/off
        public void ToggleVRGestureUI ()
        {
            uiVisible = !uiVisible;

            if (vrGestureGallery != null)
                Utils.ToggleCanvasGroup(vrGestureGallery.canvasGroup, uiVisible);

            if (vrHandUIPanel != null)
                Utils.ToggleCanvasGroup(vrHandUIPanel.GetComponent<CanvasGroup>(), uiVisible);
        }

        #region CALLED BY BUTTON METHODS

        // called every time main menu is entered
        public void BeginMainMenu()
        {
            // GET ALL THE BUTTONS IN MAIN MENU
            CanvasGroup gesturesButton = new CanvasGroup();
            CanvasGroup trainButton = new CanvasGroup();
            CanvasGroup detectButton = new CanvasGroup();
            CanvasGroup[] cgs = mainMenu.GetComponentsInChildren<CanvasGroup>();
            List<CanvasGroup> buttons = new List<CanvasGroup>();
            for (int i = 0; i < cgs.Length; i++)
            {
                if (cgs[i].name == "Gestures Button")
                {
                    gesturesButton = cgs[i];
                    buttons.Add(gesturesButton);
                }
                if (cgs[i].name == "Train Button")
                {
                    trainButton = cgs[i];
                    buttons.Add(trainButton);
                }
                if (cgs[i].name == "Detect Button")
                {
                    detectButton = cgs[i];
                    buttons.Add(detectButton);
                }
            }

            // DISABLE ALL BUTTONS FIRST
            foreach (CanvasGroup c in buttons)
            {
                c.interactable = false;
                c.blocksRaycasts = true;
                c.alpha = .5f;
            }

            // ENABLE BUTTONS DEPENDING ON TRAINING STATE
            // enable record button because always need it
            Utils.ToggleCanvasGroup(gesturesButton, true, 1f);

            if (gestureSettings.gestureBank.Count > 0)
            {
                // some gestures recorded, show edit and train buttons
                Utils.ToggleCanvasGroup(trainButton, true, 1f);
            }
            if (gestureSettings.Gestures.Count > 0)
            {
                // some gestures trained, show detect button
                Utils.ToggleCanvasGroup(detectButton, true, 1f);
            }
        }

        public void BeginDetectMenu()
        {
            StartCoroutine(RefreshDetectLogs("begin", true, 0, ""));
            UpdateConfidenceThresholdUI();
        }

        // called when detect mode begins
        public void BeginDetectMode()
        {
            rig.BeginDetect();
        }

        // called when entering "New Gesture Settings Menu" 
        // where you choose single handed or double handed type of gesture to record
        public void BeginNewGestureSettingsMenu()
        {
            panelManager.FocusPanel("New Gesture Settings Menu");

            singleHandedButton.onClick.RemoveAllListeners();
            doubleHandedButton.onClick.RemoveAllListeners();

            string newGestureName = "Gesture " + (gestureSettings.gestureBank.Count + 1);

            singleHandedButton.onClick.AddListener(() => CreateGesture(newGestureName, false));
            //singleHandedButton.onClick.AddListener(() => gestureSettings.RefreshGestureBank(true));
            singleHandedButton.onClick.AddListener(() => BeginRecordingMenu(newGestureName));
            singleHandedButton.onClick.AddListener(() => panelManager.FocusPanel("Recording Menu"));
            doubleHandedButton.onClick.AddListener(() => CreateGesture(newGestureName, true));
            doubleHandedButton.onClick.AddListener(() => BeginRecordingMenu(newGestureName));
            doubleHandedButton.onClick.AddListener(() => panelManager.FocusPanel("Recording Menu"));
        }

        // called when entering recording menu
        public void BeginRecordingMenu(string gestureName)
        {
            nowRecordingGestureLabel.text = gestureName;
            rig.BeginReadyToRecord(gestureName);
            RefreshTotalExamplesLabel();
            RefreshHandednessLabel();
        }

        public void BeginEditGesture(string gestureName)
        {
            nowEditingGestureLabel.text = gestureName;
            deleteGestureButton.onClick.RemoveAllListeners();
            deleteGestureButton.onClick.AddListener(() => panelManager.FocusPanel("Delete Confirm Menu")); // go to confirm delete menu
            deleteGestureButton.onClick.AddListener(() => BeginDeleteConfirm(gestureName));
            rig.BeginEditing(gestureName);
        }

        public void BeginDeleteConfirm(string gestureName)
        {
            deleteGestureConfirmLabel.text = gestureName;
            deleteGestureConfirmButton.onClick.RemoveAllListeners();
            deleteGestureConfirmButton.onClick.AddListener(() => DeleteGesture(gestureName));
            deleteGestureConfirmButton.onClick.AddListener(() => panelManager.FocusPanel("Gestures Menu")); 
        }

        public void BeginTraining()
        {
            panelManager.FocusPanel("Training Menu");
            neuralNetTraining.text = gestureSettings.currentNeuralNet;
            gestureSettings.BeginTraining(OnFinishedTraining);
        }

        public void QuitTraining()
        {
            gestureSettings.EndTraining(OnQuitTraining);
        }

        void OnFinishedTraining(string neuralNetName)
        {
            StartCoroutine(TrainingMenuDelay(1f));
        }

        void OnQuitTraining(string neuralNetName)
        {
            StartCoroutine(TrainingMenuDelay(1f));
        }

        public void CreateGesture(string gestureName, bool isSynchronized = false)
        {
            gestureSettings.CreateGesture(gestureName, isSynchronized);
            GenerateGesturesMenu();
        }

        public void DeleteGesture(string gestureName)
        {
            gestureSettings.DeleteGesture(gestureName);
        }

        void UpdateConfidenceThresholdUI()
        {
            if (thresholdSlider != null)
            {
                gestureSettings.confidenceThreshold = thresholdSlider.value;
                thresholdSlider.value = (float)gestureSettings.confidenceThreshold;
            }
            if (thresholdLog != null)
            {
                thresholdLog.text = gestureSettings.confidenceThreshold.ToString("F3");
            }
        }

        IEnumerator RefreshDetectLogs(string gestureName, bool isNull, double confidence, string info)
        {
            float clearDelay = 1f;

            // get all the elements
            Image bigFeedbackImage = detectMenu.Find("List Panel/Detect Big Feedback").GetComponent<Image>();
            Text bigFeedbackText = bigFeedbackImage.transform.GetChild(0).GetComponent<Text>();
            Text gestureLog = detectMenu.Find("List Panel/Sub Panel/Right Panel/Detect Log Gesture").GetChild(0).GetComponent<Text>();
            Text confidenceLog = detectMenu.Find("List Panel/Sub Panel/Right Panel/Detect Log Confidence").GetChild(0).GetComponent<Text>();
            Text infoLog = detectMenu.Find("List Panel/Sub Panel/Right Panel/Detect Log Info").GetChild(0).GetComponent<Text>();
            thresholdLog = detectMenu.Find("List Panel/Threshold Sub Panel/Detect Log Threshold").GetChild(0).GetComponent<Text>();
            thresholdSlider = detectMenu.Find("List Panel/Threshold Sub Panel/Threshold Slider").GetComponent<Slider>();

            // at first set the slider to the stored confidence threshold
            thresholdSlider.value = (float)gestureSettings.confidenceThreshold;

            // set the log text
            gestureLog.text = gestureName;
            confidenceLog.text = confidence.ToString("F3");
            infoLog.text = info;

            // set the big feedback
            if (isNull)
            {
                bigFeedbackImage.color = Color.red;
                bigFeedbackText.text = "REJECTED";
            }
            else
            {
                bigFeedbackImage.color = Color.green;
                bigFeedbackText.text = "DETECTED";
            }

            if (gestureName == "begin")
            {
                bigFeedbackImage.color = Color.white;
                bigFeedbackText.text = "";
            }

            // wait a second, then clear everything visually
            yield return new WaitForSeconds(clearDelay);
            bigFeedbackImage.color = Color.white;
            bigFeedbackText.text = "";

            yield return null;
        }

        IEnumerator TrainingMenuDelay(float delay)
        {
            // after training complete and a short delay go back to main menu
            yield return new WaitForSeconds(delay);
            panelManager.FocusPanel("Main Menu");
        }

        #endregion

        #region GENERATIVE BUTTONS

        List<string> GesturesAsStringList(List<Gesture> gesturesToConvert)
        {
            List<string> gestureStringList = new List<string>();
            foreach (Gesture g in gesturesToConvert)
            {
                gestureStringList.Add(g.name);
            }
            return gestureStringList;
        }

        void GenerateGesturesMenu()
        {
            Transform listPanelParent = gesturesMenu.Find("List Panel/Buttons Parent");
            gestureSettings.RefreshGestureBank(true);
            StartCoroutine(GenerateGestureButtons(gestureSettings.gestureBank, listPanelParent));
        }

        IEnumerator GenerateGestureButtons(List<Gesture> gesturesToGenerate, Transform buttonsParent)
        {
            Transform newGestureButton = gesturesMenu.Find("List Panel/New Gesture Button");

            // first destroy the old gesture buttons if they are there
            if (gestureButtons != null)
            {
                if (gestureButtons.Count > 0)
                {
                    foreach (Button button in gestureButtons)
                    {
                        Destroy(button.gameObject);
                    }
                    gestureButtons.Clear();
                }
            }

            float gestureButtonHeight = 30;

            yield return StartCoroutine
            (
                GenerateButtonsFromList
                (
                    GesturesAsStringList(gesturesToGenerate),
                    buttonsParent, 
                    gestureButtonPrefab, 
                    gestureButtonHeight, 
                    buttonListAnimationDelay,
                    value => gestureButtons = value
                )
            );

            // set the functions that the button will call when pressed
            for (int i = 0; i < gestureButtons.Count; i++)
            {
                AddGestureButtonListeners(i);

                // set the gesture total examples
                Text totalExamplesText = gestureButtons[i].transform.Find("Gesture Info Parent/Gesture Total").GetComponentInChildren<Text>();
                totalExamplesText.text = gesturesToGenerate[i].exampleCount.ToString();
                // set the gesture handedness 
                Text handednessText = gestureButtons[i].transform.Find("Gesture Info Parent/Gesture Handedness").GetComponentInChildren<Text>();
                CanvasGroup doubleHandedIcon = gestureButtons[i].transform.Find("Gesture Info Parent/Gesture Handedness/Double Handed Icon").GetComponent<CanvasGroup>();
                CanvasGroup singleHandedIcon = gestureButtons[i].transform.Find("Gesture Info Parent/Gesture Handedness/Single Handed Icon").GetComponent<CanvasGroup>();

                switch (gesturesToGenerate[i].isSynchronous)
                {
                    case true:
                        {
                            handednessText.text = "Double";
                            doubleHandedIcon.alpha = 1;
                            doubleHandedIcon.gameObject.SetActive(true);
                            singleHandedIcon.alpha = 0;
                            singleHandedIcon.gameObject.SetActive(false);
                        }
                        break;
                    case false:
                        {
                            handednessText.text = "Single";
                            doubleHandedIcon.alpha = 0;
                            doubleHandedIcon.gameObject.SetActive(false);
                            singleHandedIcon.alpha = 1;
                            singleHandedIcon.gameObject.SetActive(true);
                        }
                        break;
                }
            }

            newGestureButton.transform.SetAsLastSibling();

            yield break;
        }

        void AddGestureButtonListeners(int index)
        {
            string gestureName = gestureSettings.gestureBank[index].name;

            Button recordButton = gestureButtons[index].transform.Find("Record Button").GetComponent<Button>();
            recordButton.onClick.AddListener(() => BeginRecordingMenu(gestureName));
            recordButton.onClick.AddListener(() => panelManager.FocusPanel("Recording Menu"));

            Button editButton = gestureButtons[index].transform.Find("Edit Button").GetComponent<Button>();
            editButton.onClick.AddListener(() => BeginEditGesture(gestureName));
            editButton.onClick.AddListener(() => panelManager.FocusPanel("Editing Menu"));
        }

        IEnumerator GenerateNeuralNetMenuButtons()
        {

            int neuralNetMenuButtonHeight = 30;

            yield return StartCoroutine
            (
                GenerateButtonsFromList
                (
                    gestureSettings.neuralNets,
                    selectNeuralNetMenu.transform.Find("Buttons Parent"),
                    neuralNetButtonPrefab,
                    neuralNetMenuButtonHeight,
                    buttonListAnimationDelay,
                    value => neuralNetButtons = value
                )
            );

            // set the functions that the button will call when pressed
            for (int i = 0; i < neuralNetButtons.Count; i++)
            {
                neuralNetButtons[i].onClick.AddListener(() => panelManager.FocusPanel("Main Menu"));
                AddSelectNeuralNetListener(i);
            }

            // add on click functions to new neural net button
            Button newNeuralNetButtonButton = newNeuralNetButton.GetComponent<Button>();
            newNeuralNetButtonButton.onClick.RemoveAllListeners();
            newNeuralNetButtonButton.onClick.AddListener(gestureSettings.CreateNewNeuralNet);
            newNeuralNetButtonButton.onClick.AddListener(RefreshNeuralNetMenu);
            newNeuralNetButton.transform.SetAsLastSibling();

            yield break;
        }

        void AddSelectNeuralNetListener(int index)
        {
            string neuralNetName = gestureSettings.neuralNets[index];
            neuralNetButtons[index].onClick.AddListener(() => gestureSettings.SelectNeuralNet(neuralNetName));
        }

        void RefreshNeuralNetMenu()
        {
            // delete all the neural net menu buttons
            for (int i = neuralNetButtons.Count -1; i >= 0; i--)
            {
                Destroy(neuralNetButtons[i].gameObject);
            }
            neuralNetButtons.Clear();

            // refresh the list
            gestureSettings.RefreshNeuralNetList();

            // create the neural net menu buttons again
            StartCoroutine(GenerateNeuralNetMenuButtons());
        }

        IEnumerator GenerateButtonsFromList(List<string> strings, Transform parent, GameObject prefab, float buttonHeight, float instantiateDelay, Action<List<Button>> buttonList)
        {
            List<Button> buttons = new List<Button>();
            for (int i = 0; i < strings.Count; i++)
            {
                // instantiate the button
                GameObject button = GameObject.Instantiate(prefab);
                button.transform.SetParent(parent);
                button.transform.localPosition = Vector3.zero;
                button.transform.localRotation = Quaternion.identity;
                RectTransform buttonRect = button.GetComponent<RectTransform>();
                buttonRect.localScale = Vector3.one;
                button.transform.name = strings[i] + " Button";
                // set the button y position
                float totalHeight = strings.Count * buttonHeight;
                float y = 0f;
                if (i == 0)
                {
                    y = totalHeight / 2;
                }
                y = (totalHeight / 2) - (i * buttonHeight);
                buttonRect.localPosition = new Vector3(0, y, 0);
                // set the button text
                Text buttonText = null;
                if (button.transform.Find("Gesture Info Parent/Gesture Name") == true)
                {
                    // this is special for gesture buttons
                    buttonText = button.transform.Find("Gesture Info Parent/Gesture Name").GetComponentInChildren<Text>();
                }
                else
                {
                    // this is special for neural net buttons
                    buttonText = button.transform.GetComponentInChildren<Text>(true);
                }
                buttonText.text = strings[i];
                buttons.Add(button.GetComponent<Button>());

                yield return new WaitForSeconds(instantiateDelay);
            }
            buttonList(buttons);
            yield break;
        }

        #endregion

        void OnEnable()
        {
            gestureSettings = Utils.GetGestureSettings();

            rig = VRGestureRig.GetPlayerRig(gestureSettings.playerID);

            GestureRecognizer.GestureDetectedEvent += OnGestureDetected;
            GestureRecognizer.GestureRejectedEvent += OnGestureRejected;
            VRGestureUIPanelManager.OnPanelFocusChanged += PanelFocusChanged;
            //VRControllerUIInput.OnVRGuiHitChanged += VRGuiHitChanged;

            
        }

        void OnDisable()
        {
            GestureRecognizer.GestureDetectedEvent -= OnGestureDetected;
            GestureRecognizer.GestureRejectedEvent -= OnGestureRejected;
            VRGestureUIPanelManager.OnPanelFocusChanged -= PanelFocusChanged;
            //VRControllerUIInput.OnVRGuiHitChanged -= VRGuiHitChanged;
        }

        void OnGestureDetected (string gestureName, double confidence, Handedness hand, bool isDouble)
        {
            StartCoroutine(RefreshDetectLogs(gestureName, false, confidence, "Gesture Detected" ));
            //detectLog.text = gestureName + "\n" + confidence.ToString("F3");
        }

        void OnGestureRejected(string error, string gestureName = null, double confidence = 0)
        {
            StartCoroutine(RefreshDetectLogs(gestureName, true,confidence, error));
            //detectLog.text = "null" + "\n" + error;
        }

        void VRGuiHitChanged(bool hitBool)
        {
            if (hitBool)
            {
                if (rig.uiState == VRGestureUIState.ReadyToRecord)
                {
                    TogglePanelAlpha("Recording Menu", 1f);
                    TogglePanelInteractivity("Recording Menu", true);
                }
            }
            else if (!hitBool)
            {
                if (rig.uiState == VRGestureUIState.ReadyToRecord || rig.uiState == VRGestureUIState.Recording)
                {
                    TogglePanelAlpha("Recording Menu", .35f);
                    TogglePanelInteractivity("Recording Menu", false);
                }
            }
        }

        void TogglePanelAlpha(string panelName, float toAlpha)
        {
            CanvasRenderer[] canvasRenderers = vrHandUIPanel.GetComponentsInChildren<CanvasRenderer>();
            foreach (CanvasRenderer cr in canvasRenderers)
            {
                cr.SetAlpha(toAlpha);
            }
        }

        void TogglePanelInteractivity(string panelName, bool interactive)
        {
            Button[] buttons = vrHandUIPanel.GetComponentsInChildren<Button>();
            foreach (Button button in buttons)
            {
                button.interactable = interactive;
            }
        }

        void PanelFocusChanged(Panel panel)
        {
            if (panel.name == "Main Menu")
            {
                rig.uiState = VRGestureUIState.Idle;
                BeginMainMenu();
            }
            if (panel.name == "Select Neural Net Menu")
            {
                gestureSettings.RefreshNeuralNetList();
                rig.uiState = VRGestureUIState.Idle;
            }
            if (panel.name == "Gestures Menu")
            {
                rig.uiState = VRGestureUIState.Gestures;
                GenerateGesturesMenu();
            }
            if (panel.name == "New Gesture Settings Menu")
            {
                rig.uiState = VRGestureUIState.Idle;
                GenerateGesturesMenu();
            }
            if (panel.name == "Recording Menu")
            {
                //vrGestureManager.state = VRGestureManagerState.ReadyToRecord;
                //Not sure why this is in here. This re-introduced the sticky button bug.
            }
            if (panel.name == "Editing Menu")
            {
                rig.uiState = VRGestureUIState.Editing;
            }
            if (panel.name == "Delete Confirm Menu")
            {
                rig.uiState = VRGestureUIState.Editing;
            }
            if (panel.name == "Detect Menu")
            {
                BeginDetectMenu();
            }
        }

        void UpdateCurrentNeuralNetworkText()
        {
            if (GetCurrentNeuralNetworkText() == null)
                return;

            Text title = GetCurrentNeuralNetworkText();
            title.text = gestureSettings.currentNeuralNet;
        }

        //Maybe these could be fixed with Event Listeners for Start/Capture Events.
        //This will be problemtatic for showing a left vs right recorded gesture.
        //What happens when you start recording a gesture while another one is being recorded.
        //This will create a lot of prolems.
        void UpdateNowRecordingStatus()
        {

            if (rig.uiState == VRGestureUIState.ReadyToRecord
                || rig.uiState == VRGestureUIState.EnteringRecord)
            {
                nowRecordingBackground.color = Color.grey;
                nowRecordingLabel.text = "ready to record";
            }
            else if (rig.uiState == VRGestureUIState.Recording)
            {
                nowRecordingBackground.color = Color.red;
                nowRecordingLabel.text = "RECORDING";
            }
            // update gesture example count in UI if gesture just finished recording
            if (rig.uiState != rig.uiStateLast)
            {
                if (rig.uiStateLast == VRGestureUIState.Recording)
                {
                    RefreshTotalExamplesLabel();
                }
            }
        }

        void RefreshHandednessLabel()
        {
            Predicate<Gesture> gestureFinder = (Gesture g) => { return g.name == rig.currentTrainer.CurrentGesture.name; };
            Gesture gesture = gestureSettings.gestureBank.Find(gestureFinder);
            Text instructionsText = recordingMenu.Find("List Panel/Instructions Label/Text").GetComponent<Text>();

            switch (gesture.isSynchronous)
            {
                case true:
                    {
                        nowRecordingHandednessLabel.text = "Double Handed";
                        instructionsText.text = "hold left and right triggers to record";
                    }
                    break;
                case false:
                    {
                        nowRecordingHandednessLabel.text = "Single Handed";
                        instructionsText.text = "hold trigger to record";
                    }
                    break;
            }
        }

        // refresh the label that says how many examples recorded
        void RefreshTotalExamplesLabel ()
        {
            Predicate<Gesture> gestureFinder = (Gesture g) => { return g.name == rig.currentTrainer.CurrentGesture.name; };
            Gesture gesture = gestureSettings.gestureBank.Find(gestureFinder);
            nowRecordingTotalExamplesLabel.text = gesture.exampleCount.ToString();
            int totalExamples = Utils.GetGestureExamplesTotal(gesture, gestureSettings.currentNeuralNet);
            nowRecordingTotalExamplesLabel.text = totalExamples.ToString();
        }

        Text GetCurrentNeuralNetworkText()
        {
            // update current neural network name on each currentNeuralNetworkTitle UI thingy
            if (panelManager == null)
                return null;
            if (vrHandUIPanel == null)
                return null;
            if (vrHandUIPanel.Find(panelManager.currentPanel.name) == null)
                return null;
            Transform currentPanelParent = vrHandUIPanel.Find(panelManager.currentPanel.name);
            if (currentPanelParent == null)
                return null;
            Transform currentNeuralNetworkTitle = currentPanelParent.Find("Top Panel/Current Neural Network");
            if (currentNeuralNetworkTitle == null)
                return null;

            Text title = currentNeuralNetworkTitle.Find("neural network name").GetComponent<Text>();
            return title;
        }
    }
}
