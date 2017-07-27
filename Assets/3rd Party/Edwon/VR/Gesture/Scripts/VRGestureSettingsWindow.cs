#if UNITY_EDITOR
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using System;

namespace Edwon.VR.Gesture
{
    public class VRGestureSettingsWindow : EditorWindow
    {
        int maxWidth = 300;

        public VRGestureSettings gestureSettings;
        public SerializedObject serializedObject;

        #region GUI VARIABLES

        // neural net gui helpers
        int lastSelectedNeuralNetIndex = 0;
        int selectedNeuralNetIndex = 0;
        string newNeuralNetName;

        public enum VRGestureRenameState { Good, NoChange, Duplicate };
        string selectedFocus = "";

        #region GUI CONTENT VARIABLES

        public enum EditorListOption
        {
            None = 0,
            ListSize = 1,
            ListLabel = 2,
            ElementLabels = 4,
            Buttons = 8,
            Default = ListSize | ListLabel | ElementLabels,
            NoElementLabels = ListSize | ListLabel,
            ListLabelButtons = ListLabel | Buttons,
            All = Default | Buttons
        }

        private static GUILayoutOption miniButtonWidth = GUILayout.Width(20f);


        private static GUIContent
        useToggleContent = new GUIContent("", "use this gesture"),
        moveButtonContent = new GUIContent("\u21b4", "move down"),
        duplicateButtonContent = new GUIContent("+", "duplicate"),
        deleteButtonContent = new GUIContent("-", "delete"),
        addButtonContent = new GUIContent("+", "add element"),
        neuralNetNoneButtonContent = new GUIContent("+", "click to create a new neural net"),
        trainButtonContent = new GUIContent("TRAIN", "press to train the neural network with the recorded gesture data"),
        detectButtonContent = new GUIContent("DETECT", "press to begin detecting gestures");

        Texture2D bg1;
        Texture2D bg2;

        int tabIndex;

        #endregion


        // GUI MODES
        private bool showSettingsGUI = false;
        enum NeuralNetGUIMode { None, EnterNewNetName, ShowPopup };
        NeuralNetGUIMode neuralNetGUIMode;

        #endregion

        [MenuItem("Tools/VR Infinite Gesture/Settings")]
        public static void Init()
        {
            // get exisiting open window or if none make one
            VRGestureSettingsWindow window = (
                VRGestureSettingsWindow)EditorWindow.GetWindow<VRGestureSettingsWindow>(
                    false, 
                    "VR Gesture", 
                    true );
        }

        void OnEnable()
        {
            hideFlags = HideFlags.HideAndDontSave;

            GetSetGestureSettings();
            SetSerializedObject();

            gestureSettings.RefreshNeuralNetList();

        }

        void OnDestroy()
        {
            if (Utils.GetGestureSettings() != null)
            {
                EditorUtility.SetDirty(gestureSettings);
                AssetDatabase.SaveAssets();
            }
        }

        void OnGUI ()
        {

            serializedObject.Update();

            GetSetGestureSettings();
            SetSerializedObject();

            // TEXTURE SETUP
            bg1 = AssetDatabase.LoadAssetAtPath<Texture2D>("");
            bg2 = AssetDatabase.LoadAssetAtPath<Texture2D>("");

            GUILayout.BeginVertical(GUILayout.Width(EditorGUIUtility.currentViewWidth));

            GUILayout.Space(5);

            ShowToolbar();
            ShowToolbarContent();
            FocusAndClickUpdate();

            GUILayout.EndVertical();

            serializedObject.ApplyModifiedProperties();
        }

        void GetSetGestureSettings()
        {
            if (Utils.GetGestureSettings() == null)
            {
                gestureSettings = CreateGestureSettingsAsset();
            }
            else
            {
                gestureSettings = Utils.GetGestureSettings();
            }
        }

        public static VRGestureSettings CreateGestureSettingsAsset()
        {
            VRGestureSettings instance = CreateInstance<VRGestureSettings>();
            AssetDatabase.CreateAsset(instance, Config.SETTINGS_FILE_PATH);
            return instance;
        }

        void SetSerializedObject()
        {
            if (serializedObject == null)
            {
                serializedObject = new SerializedObject(gestureSettings);
            }
        }

        void ShowToolbar()
        {
            string[] tabs = new string[] { "Neural Networks", "Settings" };
            tabIndex = GUILayout.Toolbar(tabIndex, tabs);
            switch (tabIndex)
            {
                case 0:
                    showSettingsGUI = false;
                    break;
                case 1:
                    showSettingsGUI = true;
                    break;
            }
        }

        void ShowToolbarContent()
        {
            if (showSettingsGUI)
                ShowSettingsTab();
            else
                ShowNeuralNetworksTab();
        }

        void FocusAndClickUpdate()
        {
            //Enter click
            if (Event.current.isKey && Event.current.keyCode == KeyCode.Return && Event.current.type == EventType.KeyUp && IfGestureControl(GUI.GetNameOfFocusedControl()))
            {
                //On return key press
                ChangeGestureName(GUI.GetNameOfFocusedControl());
            }

            //Focus change
            if (GUI.GetNameOfFocusedControl() != selectedFocus)
            {
                //Focus has changed from a Gesture Control.
                if (IfGestureControl(selectedFocus))
                {
                    ChangeGestureName(selectedFocus);
                }
                selectedFocus = GUI.GetNameOfFocusedControl();
            }
        }

        void ChangeGestureName(string controlName)
        {

            int listIndex = Int32.Parse(controlName.Substring(16));
            VRGestureRenameState checkState = gestureSettings.RenameGesture(listIndex);
            if (checkState == VRGestureRenameState.Duplicate)
            {
                EditorUtility.DisplayDialog("Hey, listen!",
                        "You can't have duplicate gesture names.", "ok");
            }
        }

        bool IfGestureControl(string controlName)
        {
            return (controlName.Length > 15 && controlName.Substring(0, 15) == "Gesture Control");
        }

        #region SHOW GUI SECTIONS

        void ShowNeuralNetworksTab()
        {
            // TRAINING SETUP UI
            if (gestureSettings.state != VRGestureUIState.Training)
            {
                // BACKGROUND / STYLE SETUP
                GUIStyle neuralSectionStyle = new GUIStyle();
                neuralSectionStyle.normal.background = bg1;
                GUIStyle gesturesSectionStyle = new GUIStyle();
                gesturesSectionStyle.normal.background = bg1;
                GUIStyle separatorStyle = new GUIStyle();
                //separatorStyle.normal.background = bg2;

                // SEPARATOR
                GUILayout.BeginHorizontal(separatorStyle);
                EditorGUILayout.Separator(); // a little space between sections
                GUILayout.EndHorizontal();

                // NEURAL NET SECTION
                GUILayout.BeginVertical(neuralSectionStyle, 
                    GUILayout.Width(EditorGUIUtility.currentViewWidth)
                    );
                ShowNeuralNets();
                GUILayout.EndVertical();

                // SEPARATOR
                GUILayout.BeginVertical(separatorStyle);
                EditorGUILayout.Separator(); // a little space between sections
                GUILayout.EndVertical();

                // GESTURE SECTION
                GUILayout.BeginVertical(gesturesSectionStyle);
                // if a neural net is selected
                if (neuralNetGUIMode == NeuralNetGUIMode.ShowPopup)
                {
                    ShowGestures();
                }
                GUILayout.EndVertical();

                // SEPARATOR
                GUILayout.BeginHorizontal(separatorStyle);
                EditorGUILayout.Separator(); // a little space between sections
                GUILayout.EndHorizontal();

                // TRAIN BUTTON
                if (gestureSettings.readyToTrain && neuralNetGUIMode == NeuralNetGUIMode.ShowPopup)
                {
                    ShowTrainButton();
                }

            }
            // TRAINING IS PROCESSING UI
            else if (gestureSettings.state == VRGestureUIState.Training)
            {
                ShowTrainingMode();
            }
        }

        void ShowSettingsTab()
        {
            EditorGUILayout.Separator();
            SerializedProperty beginInDetectMode = serializedObject.FindProperty("beginInDetectMode");
            SerializedProperty showVRUI = serializedObject.FindProperty("showVRUI");
            //SerializedProperty playerID = serializedObject.FindProperty("playerID");
            SerializedProperty vrType = serializedObject.FindProperty("vrType");
            SerializedProperty confidenceThreshold = serializedObject.FindProperty("confidenceThreshold");
            SerializedProperty gestureSyncDelay = serializedObject.FindProperty("gestureSyncDelay");
            SerializedProperty minimumGestureAxisLength = serializedObject.FindProperty("minimumGestureAxisLength");

            //EditorGUILayout.PropertyField(playerID);

            EditorGUI.BeginChangeCheck();
            EditorGUILayout.PropertyField(vrType);
            if (EditorGUI.EndChangeCheck())
            {
                Utils.ChangeVRType((VRType)vrType.enumValueIndex);
            }

            EditorGUILayout.PropertyField(beginInDetectMode);
            if (beginInDetectMode.boolValue == true)
            {
                if (gestureSettings.neuralNets.Count > 0)
                {
                    gestureSettings.stateInitial = VRGestureUIState.ReadyToDetect;
                    EditorGUILayout.LabelField("Choose the neural network to detect with");
                    ShowNeuralNetPopup(GetNeuralNetsList());
                }
                else
                {
                    EditorGUILayout.LabelField("You must create and process a neural network before using this option");
                }
            }
            else
            {
                gestureSettings.stateInitial = VRGestureUIState.Idle;
            }
            EditorGUILayout.PropertyField(showVRUI);
            
            // confidence threshold slider
            EditorGUILayout.BeginHorizontal();
            EditorGUILayout.PrefixLabel("Confidence Threshold");
            gestureSettings.confidenceThreshold = (double)EditorGUILayout.Slider((float)confidenceThreshold.doubleValue, 0.80f, 1);
            EditorGUILayout.EndHorizontal();

            // Minimum Gesture Size float
            EditorGUILayout.BeginHorizontal();
            EditorGUILayout.PrefixLabel("Minimum Gesture Size");
            gestureSettings.minimumGestureAxisLength = EditorGUILayout.FloatField(minimumGestureAxisLength.floatValue);
            EditorGUILayout.EndHorizontal();

            EditorGUILayout.PropertyField(gestureSyncDelay);

            // this should come back in a later update
            //EditorGUILayout.PropertyField(serializedObject.FindProperty("vrGestureDetectType"));
        }

        string[] GetNeuralNetsList()
        {
            //gestureSettings.RefreshNeuralNetList(); //--was getting called every frame, should only get called OnEnable or on create new neural network.

            string[] stringArray = new string[0];
            if (gestureSettings.neuralNets.Count > 0)
            {
                stringArray = ConvertStringListPropertyToStringArray("neuralNets");
            }
            return stringArray;
        }

        void ShowNeuralNets()
        {

            // must refresh the neural net list every OnGUI
            // to detect when the neural nets have been deleted from the folder
            gestureSettings.RefreshNeuralNetList();

            string[] neuralNetsArray = GetNeuralNetsList();

            //if (neuralNetsArray.Length == 0)

            // STATE CONTROL
            if (neuralNetGUIMode == NeuralNetGUIMode.EnterNewNetName)
            {
                EditorGUILayout.LabelField("NAME THE NEW NEURAL NET");
                ShowNeuralNetCreateNewOptions();
                if ( GUILayout.Button("Back") )
                {
                    neuralNetGUIMode = NeuralNetGUIMode.None;
                }
            }
            else if (neuralNetsArray.Length == 0) // if the neural nets list is empty show a big + button
            {
                EditorGUILayout.LabelField("CREATE A NEW NEURAL NET");
                neuralNetGUIMode = NeuralNetGUIMode.None;
            }
            else // draw the popup and little plus and minus buttons
            {
                neuralNetGUIMode = NeuralNetGUIMode.ShowPopup;
            }

            // RENDER
            GUILayout.BeginHorizontal();
            switch (neuralNetGUIMode)
            {
                case (NeuralNetGUIMode.None):
                    // PLUS + BUTTON
                    if (GUILayout.Button(neuralNetNoneButtonContent))
                    {
                        newNeuralNetName = "";
                        GUI.FocusControl("Clear");
                        neuralNetGUIMode = NeuralNetGUIMode.EnterNewNetName;
                        newNeuralNetName = "";
                        GUILayout.EndHorizontal();

                    }
                    break;
                // NEURAL NET POPUP
                case (NeuralNetGUIMode.ShowPopup):
                    ShowNeuralNetPopupGroup(neuralNetsArray);
                    GUILayout.EndHorizontal();
                    ShowNeuralNetTrainedGestures();
                    break;
            }
        }

        void ShowNeuralNetTrainedGestures()
        {
            GUIStyle style = EditorStyles.whiteLabel;
            GUILayout.BeginVertical();
            EditorGUILayout.LabelField("PROCESSED GESTURES");
            GUILayout.EndVertical();
            GUILayout.BeginVertical(style);

            //SerializedProperty gesturesList = serializedObject.FindProperty("Gestures");
            //for(int i = 0; i < gesturesList.arraySize; i++)
            //{
            //    EditorGUILayout.LabelField(gesturesList.GetArrayElementAtIndex(i).FindPropertyRelative("name").stringValue, style);
            //}


            foreach (Gesture g in gestureSettings.Gestures)
            {
                EditorGUILayout.LabelField(g.name, style);
            }


            GUILayout.EndVertical();
        }

        void ShowNeuralNetCreateNewOptions()
        {
            newNeuralNetName = EditorGUILayout.TextField(newNeuralNetName);
            if (GUILayout.Button("Create Network"))
            {
                if (string.IsNullOrEmpty(newNeuralNetName))
                {
                    EditorUtility.DisplayDialog("Please give the new neural network a name", " ", "ok");
                }
                else if (gestureSettings.CheckForDuplicateNeuralNetName(newNeuralNetName))
                {
                    EditorUtility.DisplayDialog(
                        "The name " + newNeuralNetName + " is already being used, " +
                        "please name it something else.", " ", "ok"
                    );
                }
                else
                {
                    gestureSettings.CreateNewNeuralNet(newNeuralNetName);
                    //This is incorrect because the list will always be sorted alphabetically
                    //We need to find the list in alphbetical order.

                    //TODO: CALL gestureSettings.RefreshNeuralNetworkList?
                    List<string> sortedList = new List<string>(gestureSettings.neuralNets);
                    sortedList.Sort(
                        delegate (String s1, String s2)
                        {
                            return s1.CompareTo(s2);
                        }
                    );
                    selectedNeuralNetIndex = sortedList.IndexOf(newNeuralNetName);
                    gestureSettings.RefreshNeuralNetList();
                    neuralNetGUIMode = NeuralNetGUIMode.ShowPopup;
                }
            }

        }

        void ShowNeuralNetPopupGroup(string[] neuralNetsArray)
        {
            ShowNeuralNetPopup(neuralNetsArray);

            // + button
            if (GUILayout.Button(duplicateButtonContent, EditorStyles.miniButtonMid, miniButtonWidth))
            {
                newNeuralNetName = "";
                GUI.FocusControl("Clear");
                neuralNetGUIMode = NeuralNetGUIMode.EnterNewNetName;

            }

            // - button
            if (GUILayout.Button(deleteButtonContent, EditorStyles.miniButtonRight, miniButtonWidth))
            {
                if (ShowNeuralNetDeleteDialog(gestureSettings.currentNeuralNet))
                {
                    gestureSettings.DeleteNeuralNet(gestureSettings.currentNeuralNet);
                    if (gestureSettings.neuralNets.Count > 0)
                        selectedNeuralNetIndex = 0;
                }
            }
        }

        /// <summary>
        /// The Neural Network Dropdown List
        /// For selecting the current neural network
        /// </summary>
        /// <param name="neuralNetsArray"></param>
        void ShowNeuralNetPopup(string[] neuralNetsArray)
        {

            selectedNeuralNetIndex = Array.IndexOf(neuralNetsArray, gestureSettings.currentNeuralNet);
            //
            lastSelectedNeuralNetIndex = Array.IndexOf(neuralNetsArray, gestureSettings.currentNeuralNet);

            // If the choice is not in the array then the _choiceIndex will be -1 so set back to 0
            if (selectedNeuralNetIndex < 0)
                selectedNeuralNetIndex = 0;

            if (Event.current.type == EventType.ExecuteCommand)
            {
                //Debug.Log("execute command");
                gestureSettings.RefreshGestureBank(false);
            }

            EditorGUI.BeginChangeCheck();
            selectedNeuralNetIndex = EditorGUILayout.Popup(selectedNeuralNetIndex, neuralNetsArray);
            if (EditorGUI.EndChangeCheck())
            {
                // Update the selected choice in the underlying object
                if (neuralNetsArray.Length > 0)
                {
                    gestureSettings.SelectNeuralNet(neuralNetsArray[selectedNeuralNetIndex]);
                }
                else
                {
                    gestureSettings.gestureBank = null;
                    gestureSettings.currentNeuralNet = null;
                }
            }
        }

        bool ShowNeuralNetDeleteDialog(string neuralNetName)
        {
            return EditorUtility.DisplayDialog("Delete the " + neuralNetName + " neural network?",
                "This cannot be undone.",
                "ok",
                "cancel"
            );
        }

        void ShowGestures()
        {
            if (gestureSettings.gestureBankPreEdit.Count != gestureSettings.gestureBank.Count)
            {
                gestureSettings.RefreshGestureBank(false);
            }

            EditorGUILayout.LabelField("RECORDED GESTURES");

            // then get the gesture bank
            SerializedProperty gesturesList = serializedObject.FindProperty("gestureBank");
            SerializedProperty size = gesturesList.FindPropertyRelative("Array.size");

            // and finally draw the list
            ShowGestureList(gesturesList, EditorListOption.Buttons);

        }

        void ShowTrainButton()
        {
            if (GUILayout.Button("PROCESS \n" + gestureSettings.currentNeuralNet, GUILayout.Height(40f)))
            {
                EventType eventType = Event.current.type;
                if (eventType == EventType.used)
                {
                    gestureSettings.BeginTraining(OnFinishedTraining);
                }
            }
        }

        void ShowTrainingMode()
        {
            string trainingInfo = "Training " + gestureSettings.currentNeuralNet + " is in progress. \n HOLD ON TO YOUR BUTS";

            GUILayout.Label(trainingInfo, EditorStyles.centeredGreyMiniLabel, GUILayout.Height(50f));
            if (GUILayout.Button("QUIT TRAINING"))
            {
                EventType eventType = Event.current.type;
                if (eventType == EventType.used)
                {
                    gestureSettings.EndTraining(OnQuitTraining);
                }
            }
        }

        #endregion

        // callback that VRGestureManager should call upon training finished
        void OnFinishedTraining(string neuralNetName)
        {
        }

        void OnQuitTraining(string neuralNetName)
        {

        }

        string[] ConvertStringListPropertyToStringArray(string listName)
        {
            SerializedProperty sp = serializedObject.FindProperty(listName).Copy();
            if (sp.isArray)
            {
                int arrayLength = 0;
                sp.Next(true); // skip generic field
                sp.Next(true); // advance to array size field

                // get array size
                arrayLength = sp.intValue;

                sp.Next(true); // advance to first array index

                // write values to list
                string[] values = new string[arrayLength];
                int lastIndex = arrayLength - 1;
                for (int i = 0; i < arrayLength; i++)
                {
                    values[i] = sp.stringValue; // copy the value to the array
                    if (i < lastIndex)
                        sp.Next(false); // advance without drilling into children
                }
                return values;
            }
            return null;
        }

        void ShowGestureList(SerializedProperty list, EditorListOption options = EditorListOption.Default)
        {

            bool showListLabel = (options & EditorListOption.ListLabel) != 0;
            bool showListSize = (options & EditorListOption.ListSize) != 0;
            if (showListLabel)
            {
                EditorGUILayout.PropertyField(list);
                EditorGUI.indentLevel += 1;
            }
            if (!showListLabel || list.isExpanded)
            {
                SerializedProperty size = list.FindPropertyRelative("Array.size");
                if (showListSize)
                {
                    EditorGUILayout.PropertyField(list.FindPropertyRelative("Array.size"));
                }
                if (size.hasMultipleDifferentValues)
                {
                    EditorGUILayout.HelpBox("Not showing lists with different sizes.", MessageType.Info);
                }
                else
                {
                    ShowGestureListElements(list, options);
                }
            }
            if (showListLabel)
                EditorGUI.indentLevel -= 1;
        }

        private void ShowGestureListElements(SerializedProperty list, EditorListOption options)
        {
            if (!list.isArray)
            {
                EditorGUILayout.HelpBox(list.name + " is neither an array nor a list", MessageType.Error);
                return;
            }

            bool showElementLabels = (options & EditorListOption.ElementLabels) != 0;
            bool showButtons = (options & EditorListOption.Buttons) != 0;

            // render the list
            for (int i = 0; i < list.arraySize; i++)
            {
                string controlName = "Gesture Control " + i;

                if (showButtons)
                {
                    EditorGUILayout.BeginHorizontal();
                }
                if (showElementLabels)
                {
                    EditorGUILayout.PropertyField(list.GetArrayElementAtIndex(i));
                }
                else
                {
                    //Was is this one?
                    GUI.SetNextControlName(controlName);
                    //We cannot use BeginCheck() on this because that will fire and event on EVERY keystroke.
                    EditorGUILayout.PropertyField(list.GetArrayElementAtIndex(i).FindPropertyRelative("name"), GUIContent.none);
                }
                if (showButtons)
                {
                    ShowGestureListHandedness(i);
                    ShowGestureListTotalExamples(i);
                    ShowGestureListButtons(list, i);
                    EditorGUILayout.EndHorizontal();

                }

            }

            // if the list is empty show the plus + button
            if (showButtons && list.arraySize == 0 && GUILayout.Button(addButtonContent, EditorStyles.miniButton))
            {
                var option = DisplayDialogForGestureCreation();
                switch (option)
                {
                    // Create Single Gesture
                    case 0:
                        gestureSettings.CreateGesture("Gesture 1", false);
                        break;
                    // Create Double Gesture
                    case 1:
                        gestureSettings.CreateGesture("Gesture 1", true);
                        break;
                    // Cancel - do nothing
                    case 2:
                        break;
                }
            }
        }

        private void ShowGestureListHandedness(int index)
        {
            //Sometimes on the first repaint this will still be looking at the previous gestureBank
            //this means we will be checking the index intended for a different array.
            if (index < gestureSettings.gestureBank.Count)
            {
                Gesture g = gestureSettings.gestureBank[index];
                switch (g.isSynchronous)
                {
                    case false:
                        {
                            GUILayout.Label("one", EditorStyles.centeredGreyMiniLabel, GUILayout.Width(35f));
                        }
                        break;
                    case true:
                        {
                            GUILayout.Label("two", EditorStyles.centeredGreyMiniLabel, GUILayout.Width(35f));
                        }
                        break;
                }
            }
        }

        private void ShowGestureListTotalExamples(int index)
        {
            //Sometimes on the first repaint this will still be looking at the previous gestureBank
            //this means we will be checking the index intended for a different array.
            if (index < gestureSettings.gestureBank.Count)
            {
                Gesture g = gestureSettings.gestureBank[index];
                GUILayout.Label(g.exampleCount.ToString(), EditorStyles.centeredGreyMiniLabel, GUILayout.Width(35f));
            }
        }

        private int DisplayDialogForGestureCreation()
        {
            return EditorUtility.DisplayDialogComplex(
                "Create Gesture",
                "Create a one or two handed gesture?",
                "One Handed",
                "Two Handed",
                "cancel");
        }

        private void ShowGestureListButtons(SerializedProperty list, int index)
        {
            // plus button
            if (GUILayout.Button(duplicateButtonContent, EditorStyles.miniButtonMid, miniButtonWidth))
            {
                //Focus has changed from a Gesture Control.
                if (IfGestureControl(selectedFocus))
                {
                    ChangeGestureName(selectedFocus);
                }
                selectedFocus = "";

                var option = DisplayDialogForGestureCreation();
                switch (option)
                {
                    // Create Single Gesture
                    case 0:
                        CreateNewGesture(list, false);
                        break;
                    // Create Double Gesture
                    case 1:
                        CreateNewGesture(list, true);
                        break;
                    // Cancel - do nothing
                    case 2:
                        break;
                }

                
            }
            // minus button
            if (GUILayout.Button(deleteButtonContent, EditorStyles.miniButtonRight, miniButtonWidth))
            {
                // new way to delete using vrGestureManager directly
                string gestureName = list.GetArrayElementAtIndex(index).FindPropertyRelative("name").stringValue;
                gestureSettings.DeleteGesture(gestureName);

                // old way to delete from property
                //int oldSize = list.arraySize;
                //list.DeleteArrayElementAtIndex(index);
                //if (list.arraySize == oldSize)
                //    list.DeleteArrayElementAtIndex(index);
            }
        }

        private void CreateNewGesture(SerializedProperty list, bool isDouble)
        {
            int size = list.arraySize + 1;

            int counter = size;
            bool createdGesture = false;
            while (!createdGesture)
            {
                string newGestureName = "Gesture " + counter;
                counter++;
                if (gestureSettings.CheckForDuplicateGestures(newGestureName))
                {
                    createdGesture = true;
                    gestureSettings.CreateGesture(newGestureName, isDouble);
                }
            }
        }
    }
}

#endif