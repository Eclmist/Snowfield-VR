using UnityEngine;
using UnityEngine.VR;
using System.Collections;
using System.Collections.Generic;
using Edwon.VR;
using Edwon.VR.Input;
using System.IO;
using System;

namespace Edwon.VR.Gesture
{
    public class VRGestureSettings : ScriptableObject
    {
        private VRGestureRig rig;
        public VRGestureRig Rig
        {
            get
            {
                if (rig == null)
                {
                    rig = VRGestureRig.GetPlayerRig(playerID);
                }
                return rig;
            }
        }
        public int playerID = 0;
        public VRType vrType = VRType.SteamVR;

        [Tooltip("if true automatically spawn the VR Gesture UI when the scene starts")]
        public bool showVRUI = true;
        [Tooltip("the button that triggers gesture recognition")]
        public InputOptions.Button gestureButton = InputOptions.Button.Trigger1;
        [Tooltip("the threshold over wich a gesture is considered correctly classified")]
        public double confidenceThreshold = 0.98;
        [Tooltip("the amount of miliseconds allowed between two gestures performed simultaneously")]
        public int gestureSyncDelay = 500;
        [Tooltip("Your gesture must have one axis longer than this length in world size")]
        public float minimumGestureAxisLength = 0.10f;
        [Tooltip("Begin detecting gestures immediately in-game without bringing up the VR Infinite Gesture UI (i.e.when you do standalone builds)")]
        public bool beginInDetectMode = false;
        // whether to track when pressing trigger or all the time
        // continious mode is not supported yet
        // though you're welcome to try it out
        [HideInInspector]
        public VRGestureDetectType vrGestureDetectType;

        [Header("ACTIVE NETWORKS")]
        [Tooltip("the neural net that I am using")]
        public string currentNeuralNet;
        public string lastNeuralNet; // used to know when to refresh gesture bank
        public List<string> neuralNets;
        private List<Gesture> gestures;  // list of gestures already trained in currentNeuralNet
        public List<Gesture> Gestures
        {
            get
            {
                NeuralNetworkStub stub = Utils.ReadNeuralNetworkStub(currentNeuralNet);
                return stub.gestures;
            }
            set
            {
                value = gestures;
            }
        }
        public List<Gesture> gestureBank; // list of recorded gesture for current neural net
        public List<Gesture> gestureBankPreEdit;

        public Trainer currentTrainer { get; set; }

        public VRGestureUIState state = VRGestureUIState.Idle;
        public VRGestureUIState stateInitial;

        public bool readyToTrain
        {
            get
            {
                if (gestureBank != null)
                {
                    if (gestureBank.Count > 0)
                    {
                        foreach (Gesture g in gestureBank)
                        {
                            if (g.exampleCount <= 0)
                                return false;
                        }
                        return true;
                    }
                    else
                        return false;
                }
                return false;
            }
        }

        #region NEURAL NETWORK ACTIVE METHODS
        //This should be called directly from UIController via instance
        //Most of these should be moved into RIG as they are just editing vars in RIG.
        [ExecuteInEditMode]
        public void BeginTraining(Action<string> callback)
        {
            Rig.uiState = VRGestureUIState.Training;
            Rig.currentTrainer = new Trainer(currentNeuralNet, gestureBank);
            Rig.currentTrainer.TrainRecognizer();
            // finish training
            Rig.uiState = VRGestureUIState.Idle;
            callback(currentNeuralNet);
        }

        [ExecuteInEditMode]
        public void EndTraining(Action<string> callback)
        {
            Rig.uiState = VRGestureUIState.Idle;
            callback(currentNeuralNet);
        }
        #endregion

        #region NEURAL NETWORK EDIT METHODS
        [ExecuteInEditMode]
        public bool CheckForDuplicateNeuralNetName(string neuralNetName)
        {
            // if neuralNetName already exists return true
            if (neuralNets.Contains(neuralNetName))
                return true;
            else
                return false;
        }

        // only called by VR UI when creating a new neural net in VR
        public void CreateNewNeuralNet()
        {
            int number = neuralNets.Count + 1;
            string newNeuralNetName = "Neural Net " + number;
            CreateNewNeuralNet(newNeuralNetName);
        }

        [ExecuteInEditMode]
        public void CreateNewNeuralNet(string neuralNetName)
        {
            // create new neural net folder
            Utils.CreateFolder(neuralNetName);
            // create a gestures folder
            Utils.CreateFolder(neuralNetName + "/Gestures/");

            neuralNets.Add(neuralNetName);
            gestures = new List<Gesture>();
            gestureBank = new List<Gesture>();
            gestureBankPreEdit = new List<Gesture>();

            // select the new neural net
            SelectNeuralNet(neuralNetName);
        }

        [ExecuteInEditMode]
        public void RefreshNeuralNetList()
        {
            Utils.CheckCreateNeuralNetFolder();
            neuralNets = new List<string>();
            string path = Application.streamingAssetsPath + Config.NEURAL_NET_PATH;
            foreach (string directoryPath in System.IO.Directory.GetDirectories(path))
            {
                string directoryName = Path.GetFileName(directoryPath);
                if (!neuralNets.Contains(directoryName))
                {
                    neuralNets.Add(directoryName);
                }
            }
        }

        [ExecuteInEditMode]
        public void RefreshGestureBank(bool checkNeuralNetChanged)
        {
            if (checkNeuralNetChanged)
            {
                if (currentNeuralNet == lastNeuralNet)
                {
                    return;
                }
            }

            if (currentNeuralNet != null && currentNeuralNet != "")
            {
                gestureBank = Utils.GetGestureBank(currentNeuralNet);
				foreach (Gesture g in gestureBank) {
					g.exampleCount = Utils.GetGestureExamplesTotal (g, currentNeuralNet);
				}

                gestureBankPreEdit = gestureBank.ConvertAll(gesture => gesture.Clone());
            }
            else
            {
                gestureBank = new List<Gesture>();
                gestureBankPreEdit = new List<Gesture>();
            }
        }

        [ExecuteInEditMode]
        public void DeleteNeuralNet(string neuralNetName)
        {
            // get this neural nets index so we know which net to select next
            int deletedNetIndex = neuralNets.IndexOf(neuralNetName);

            // delete the net and gestures
            neuralNets.Remove(neuralNetName); // remove from list
            gestureBank.Clear(); // clear the gestures list
            gestureBankPreEdit.Clear();
            Utils.DeleteNeuralNetFiles(neuralNetName); // delete all the files

            if (neuralNets.Count > 0)
                SelectNeuralNet(neuralNets[0]);
        }

        [ExecuteInEditMode]
        public void SelectNeuralNet(string neuralNetName)
        {
            if (currentNeuralNet != null && currentNeuralNet != "" && gestureBank.Count > 0)
            {
                // save last neural network before we switch to next one
                Utils.SaveGestureBank(gestureBank, currentNeuralNet);
            }

            lastNeuralNet = currentNeuralNet;
            currentNeuralNet = neuralNetName;
            RefreshGestureBank(true);
        }

        [ExecuteInEditMode]
        public void CreateGesture(string gestureName, bool isSynchronized = false)
        {
            Gesture newGesture = new Gesture();
            newGesture.name = gestureName;
            newGesture.hand = Handedness.Right;
            newGesture.isSynchronous = isSynchronized;
            newGesture.exampleCount = 0;

            gestureBank.Add(newGesture);
            Utils.CreateGestureFile(gestureName, currentNeuralNet);
            Utils.SaveGestureBank(gestureBank, currentNeuralNet);
            gestureBankPreEdit = gestureBank.ConvertAll(gesture => gesture.Clone());
        }

        public void CreateSingleGesture(string gestureName, Handedness hand, bool isSynchronous)
        {
            Gesture newGesture = new Gesture();
            newGesture.name = gestureName;
            newGesture.hand = hand;
            newGesture.isSynchronous = isSynchronous;
            newGesture.exampleCount = 0;


            gestureBank.Add(newGesture);
            //Maybe name files based on isSync - Hand - name. i.e.: 1R-Helicopter 0B-Rainbow
            Utils.CreateGestureFile(gestureName, currentNeuralNet);
            Utils.SaveGestureBank(gestureBank, currentNeuralNet);
            gestureBankPreEdit = gestureBank.ConvertAll(gesture => gesture.Clone());
        }

        public void CreateSyncGesture(string gestureName)
        {
            CreateSingleGesture(gestureName, Handedness.Left, true);
            CreateSingleGesture(gestureName, Handedness.Right, true);
        }

        [ExecuteInEditMode]
        public Gesture FindGesture(string gestureName)
        {
            //int index = gestureBank.IndexOf(gestureName);
            Predicate<Gesture> gestureFinder = (Gesture g) => { return g.name == gestureName; };
            Gesture gest = gestureBank.Find(gestureFinder);
            return gest;
        }


        [ExecuteInEditMode]
        public void DeleteGesture(string gestureName)
        {
            Predicate<Gesture> gestureFinder = (Gesture g) => { return g.name == gestureName; };
            int index = gestureBank.FindIndex(gestureFinder);
            gestureBank.RemoveAt(index);
            Utils.DeleteGestureFile(gestureName, currentNeuralNet);
            Utils.SaveGestureBank(gestureBank, currentNeuralNet);
            gestureBankPreEdit = gestureBank.ConvertAll(gesture => gesture.Clone());
        }

        // return false if duplicates
        public bool CheckForDuplicateGestures(string newName)
        {
            bool dupeCheck = true;
            int dupeCount = -1;
            foreach (Gesture gesture in gestureBank)
            {
                if (newName == gesture.name)
                {
                    dupeCount++;
                }
            }
            if (dupeCount > 0)
            {
                dupeCheck = false;
            }

            return dupeCheck;
        }

#if UNITY_EDITOR
        [ExecuteInEditMode]
        public VRGestureSettingsWindow.VRGestureRenameState RenameGesture(int gestureIndex)
        {
            string newName = "";
            string oldName = "";

                //check to make sure the name has actually changed.
            if(gestureIndex < gestureBank.Count)
            {
                newName = gestureBank[gestureIndex].name;
                oldName = gestureBankPreEdit[gestureIndex].name;
            }else
            {
                //Debug.LogError("Out of bounds");
            }

            VRGestureSettingsWindow.VRGestureRenameState renameState = VRGestureSettingsWindow.VRGestureRenameState.Good;

            if (oldName != newName)
            {
                if (CheckForDuplicateGestures(newName))
                {
                    //ACTUALLY RENAME THAT SHIZZ
                    Utils.RenameGestureFile(oldName, newName, currentNeuralNet);
                    Utils.SaveGestureBank(gestureBank, currentNeuralNet);

                    gestureBankPreEdit = gestureBank.ConvertAll(gesture => gesture.Clone());
                }
                else
                {
                    //reset gestureBank
                    gestureBank = gestureBankPreEdit.ConvertAll(gesture => gesture.Clone());
                    renameState = VRGestureSettingsWindow.VRGestureRenameState.Duplicate;
                }
            }
            else
            {
                renameState = VRGestureSettingsWindow.VRGestureRenameState.NoChange;
            }

            return renameState;
        }
#endif

        #endregion

    }
}