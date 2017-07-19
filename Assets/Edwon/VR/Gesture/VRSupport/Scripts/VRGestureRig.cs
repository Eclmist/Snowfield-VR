using UnityEngine;
using System.Collections;
using System;
using Edwon.VR.Gesture;
using Edwon.VR.Input;
using System.Collections.Generic;

namespace Edwon.VR
{
    public class VRGestureRig : MonoBehaviour
    {
        // need a special enum in this class so you can't see "Both" in the inspector
        // as that is irrelevent

        public Handedness mainHand = Handedness.Right;
        public InputOptions.Button gestureButton = InputOptions.Button.Trigger1;
        public InputOptions.Button menuButton = InputOptions.Button.Trigger1;
        public VRGestureUIState uiState = VRGestureUIState.Idle;
        public VRGestureUIState uiStateLast;
        public bool displayGestureTrail = true;
        public int playerID = 0;

        VRGestureSettings gestureSettings;
        VRGestureSettings GestureSettings
        {
            get
            {
                if (gestureSettings == null)
                {
                    gestureSettings = Utils.GetGestureSettings();
                }
                return gestureSettings;
            }
        }

        //public VRRigAnchors vrRigAnchors;
        [SerializeField]
        public Transform head;
        [SerializeField]
        public Transform handLeft;
        [SerializeField]
        public Transform handRight;

        [SerializeField]
        GameObject leftController;
        [SerializeField]
        GameObject rightController;

        [SerializeField]
        public bool spawnControllerModels = true;
        [SerializeField]
        public bool useCustomControllerModels = false;

        [SerializeField]
        public GameObject handLeftModel;
        [SerializeField]
        public GameObject handRightModel;

        IInput inputLeft = null;
        IInput inputRight = null;

        Transform perpTransform;
        public CaptureHand leftCapture;
        public CaptureHand rightCapture;

        //current NeuralNetwork
        //current Recognizer?
        public GestureRecognizer currentRecognizer;
        public Trainer currentTrainer;
        //current Trainer?

        public static VRGestureRig GetPlayerRig(int _playerID = -1)
        {
            VRGestureRig rig = null;

            VRGestureRig[] rigs = FindObjectsOfType(typeof(VRGestureRig)) as VRGestureRig[];
            foreach (VRGestureRig _rig in rigs)
            {
                if(_rig.playerID == _playerID)
                {
                    rig = _rig;
                }
            }

            //if (FindObjectOfType<VRGestureRig>() != null)
            //{
            //    rig = FindObjectOfType<VRGestureRig>();
            //}
            return rig;
        }
        
        #region INITIALIZATION

        // Reset is called when the user hits the Reset button in the Inspector's context menu
        // or when adding the component the first time. 
        // This function is only called in editor mode. 
        void Reset()
        {
            if (gameObject.GetComponent("OVRCameraRig") != null || gameObject.GetComponent("OVRManager") != null)
            {
                Utils.ChangeVRType(VRType.OculusVR);
                GestureSettings.vrType = VRType.OculusVR;
            }
            if (gameObject.GetComponent("SteamVR_ControllerManager") != null || gameObject.GetComponent("SteamVR_PlayArea") != null)
            {
                Utils.ChangeVRType(VRType.SteamVR);
                GestureSettings.vrType = VRType.SteamVR;
            }
        }

        void Awake()
        {
            Init();
        }

        void Init()
        {
            CreateInputHelper();

            if (GestureSettings.beginInDetectMode)
            {
                BeginDetect();

                if (GestureSettings.showVRUI)
                {
                    CreateVRUI();
                }
            }
            else if (GestureSettings.showVRUI)
            {
                CreateVRUI();
            }

            //maybe only init this if it does not exist.
            //Remove all game objects
            perpTransform = transform.Find("Perpindicular Head");
            if (perpTransform == null)
            {
                perpTransform = new GameObject("Perpindicular Head").transform;
                perpTransform.parent = this.transform;
            }
            GestureTrail leftTrail = null;
            GestureTrail rightTrail = null;

            if (displayGestureTrail)
            {
                leftTrail = gameObject.AddComponent<GestureTrail>();
                rightTrail = gameObject.AddComponent<GestureTrail>();
            }
            leftCapture = new CaptureHand(this, perpTransform, Handedness.Left, leftTrail);
            rightCapture = new CaptureHand(this, perpTransform, Handedness.Right, rightTrail);

            if (leftCapture != null && rightCapture != null)
            {
                SubscribeToEvents();
            }
        }

        void SubscribeToEvents()
        {
            leftCapture.StartCaptureEvent += StartCapturing;
            leftCapture.StopCaptureEvent += StopCapturing;
            rightCapture.StartCaptureEvent += StartCapturing;
            rightCapture.StopCaptureEvent += StopCapturing;
        }

        void OnDisable()
        {
            leftCapture.StartCaptureEvent -= StartCapturing;
            leftCapture.StopCaptureEvent -= StopCapturing;
            rightCapture.StartCaptureEvent -= StartCapturing;
            rightCapture.StopCaptureEvent -= StopCapturing;
        }
        #endregion

        #region UPDATE
        void Update()
        {
            //if (uiState != uiStateLast)
            //{
            //    Debug.Log(uiState);
            //}
            uiStateLast = uiState;

            if (leftCapture != null)
            {
                leftCapture.Update();
            }
            if (rightCapture != null)
            {
                rightCapture.Update();
            }
        }

        void StartCapturing()
        {
            if (uiState == VRGestureUIState.ReadyToRecord)
            {
                uiState = VRGestureUIState.Recording;
            }
            else if (uiState == VRGestureUIState.ReadyToDetect)
            {
                uiState = VRGestureUIState.Detecting;
            }
        }

        void StopCapturing()
        {
            if (leftCapture.state == VRGestureCaptureState.Capturing || rightCapture.state == VRGestureCaptureState.Capturing)
            {
                //do nothing
            }
            else
            {
                //set state to READY
                if (uiState == VRGestureUIState.Recording)
                {
                    uiState = VRGestureUIState.ReadyToRecord;
                }
                else if (uiState == VRGestureUIState.Detecting)
                {
                    uiState = VRGestureUIState.ReadyToDetect;
                }
            }
        }

        #endregion

        #region LINE CAPTURE
        public void LineCaught(List<Vector3> capturedLine, Handedness hand)
        {
            if (uiState == VRGestureUIState.Recording || uiState == VRGestureUIState.ReadyToRecord)
            {
                currentTrainer.TrainLine(capturedLine, hand);
            }
            else if (uiState == VRGestureUIState.Detecting || uiState == VRGestureUIState.ReadyToDetect)
            {
                currentRecognizer.RecognizeLine(capturedLine, hand, this);
            }
        }
        #endregion

        public void AutoSetup()
        {
            #if EDWON_VR_OCULUS
                if (GetComponent<OVRCameraRig>() != null)
                {
                    OVRCameraRig ovrCameraRig = GetComponent<OVRCameraRig>();
                    head = ovrCameraRig.centerEyeAnchor;
                    handLeft = ovrCameraRig.leftHandAnchor;
                    handRight = ovrCameraRig.rightHandAnchor;
                }
                else
                {
                    Debug.Log(
                        "Could not setup OculusVR rig, is this script on the top level of your OVRCameraRig?\nDid you define EDWON_VR_OCULUS in Project Settings > Player ?"
                        );
                }
            #endif
            #if EDWON_VR_STEAM
                if (GetComponent<SteamVR_ControllerManager>() != null)
                {
                    SteamVR_ControllerManager steamVRControllerManager = GetComponent<SteamVR_ControllerManager>();
                    head = GetComponentInChildren<SteamVR_Camera>().transform;
                    //TODO: CHECK IN EARLIER VERSION OF UNITY. MIGHT NEED A OR for SteamVR_Gameview
                    handLeft = steamVRControllerManager.left.transform;
                    handRight = steamVRControllerManager.right.transform;
                }
                else
                {
                    Debug.Log(
                        "Could not setup SteamVR rig, is this script on the top level of your SteamVR camera prefab?\nDid you define EDWON_VR_STEAM in Project Settings > Player ?"
                        );
                }
            #endif
        }

        public Transform GetHand(Handedness handedness)
        {
            if (handedness == Handedness.Left)
            {
                return handLeft;
            }
            else
            {
                return handRight;
            }
        }

        //These are still expecting a proper HAND.
        //They will not find them because Inputs are not connected to HANDS.
        //These are now part of VR GestureManager maybe.
        /// <summary>
        /// 
        /// </summary>
        /// <param name="handedness"></param>
        /// <returns></returns>
        public IInput GetInput(Handedness handedness)
        {
            if (handedness == Handedness.Left)
            {
                return inputLeft;
            }
            else
            {
                return inputRight;
            }
        }

        /// <summary>
        /// This will check to see if an IInput interface is attached to the controller.
        /// If not it will attach the default VRControllerInput from EdwonVR
        /// </summary>
        /// <param name="hand"></param>
        /// <returns></returns>
        public void CreateInputHelper()
        {
#if EDWON_VR_STEAM

            SteamVR_ControllerManager[] steamVR_cm = FindObjectsOfType<SteamVR_ControllerManager>();
            //What happens when we get here and we ONLY have 1 controller online.
            //Do both controllers end up getting the LEFT controller?

            leftController = steamVR_cm[0].left;
            rightController = steamVR_cm[0].right;

            inputLeft = gameObject.AddComponent<VRControllerInputSteam>().Init(Handedness.Left, leftController);
            inputRight = gameObject.AddComponent<VRControllerInputSteam>().Init(Handedness.Right, rightController);

#endif

#if EDWON_VR_OCULUS

            inputLeft = handLeft.gameObject.AddComponent<VRControllerInputOculus>().Init(Handedness.Left);
            inputRight = handRight.gameObject.AddComponent<VRControllerInputOculus>().Init(Handedness.Right);

            #endif

            if (GestureSettings.showVRUI)
            {
                VRLaserPointer laserLeft = handLeft.gameObject.AddComponent<VRLaserPointer>();
                laserLeft.InitRig(this, Handedness.Left);
                VRLaserPointer laserRight = handRight.gameObject.AddComponent<VRLaserPointer>();
                laserRight.InitRig(this, Handedness.Right);
            }

            if (spawnControllerModels)
            {
                SpawnControllerModels();
            }
        }

        public void CreateVRUI()
        {
            Instantiate(Resources.Load(Config.PARENT_PATH + "VRUI/VR Gesture UI"));
        }

        public void SpawnControllerModels ()
        {
            if (useCustomControllerModels == false)
            {
                if (GestureSettings.vrType == VRType.OculusVR)
                {
                    handLeftModel = Resources.Load(
                        Config.PARENT_PATH + "VR Controller Art/Oculus_Simple_Left") as GameObject;
                    handRightModel = Resources.Load(
                        Config.PARENT_PATH + "VR Controller Art/Oculus_Simple_Right") as GameObject;
                }
                else if (GestureSettings.vrType == VRType.SteamVR)
                {
                    handLeftModel = Resources.Load(
                        Config.PARENT_PATH + "VR Controller Art/Vive_Simple") as GameObject;
                    handRightModel = Resources.Load(
                        Config.PARENT_PATH + "VR Controller Art/Vive_Simple") as GameObject;
                }
            }
            Transform leftModel = GameObject.Instantiate(handLeftModel).transform;
            Transform rightModel = GameObject.Instantiate(handRightModel).transform;
            leftModel.parent = handLeft;
            rightModel.parent = handRight;
            leftModel.localPosition = Vector3.zero;
            rightModel.localPosition = Vector3.zero;
            leftModel.localRotation = Quaternion.identity;
            rightModel.localRotation = Quaternion.identity;

            // rotate because steam vr is weird
            if (GestureSettings.vrType == VRType.SteamVR)
            {
                leftModel.Rotate(-300, 0, 180);
                rightModel.Rotate(-300, 0, 180);
            }
        }


        #region RECORDING/DETECTING
        public void BeginReadyToRecord(string gesture)
        {
            currentTrainer = new Trainer(GestureSettings.currentNeuralNet, GestureSettings.gestureBank);
            currentTrainer.CurrentGesture = GestureSettings.FindGesture(gesture); ;
            uiState = VRGestureUIState.ReadyToRecord;
            leftCapture.state = VRGestureCaptureState.EnteringCapture;
            rightCapture.state = VRGestureCaptureState.EnteringCapture;
        }

        public void BeginEditing(string gesture)
        {
            currentTrainer = new Trainer(GestureSettings.currentNeuralNet, GestureSettings.gestureBank);
            currentTrainer.CurrentGesture = GestureSettings.FindGesture(gesture);
        }

        public void BeginDetect()
        {
            uiState = VRGestureUIState.ReadyToDetect;
            currentRecognizer = new GestureRecognizer(GestureSettings.currentNeuralNet);
        }


        #endregion
    }


}









