using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using System.Collections;
#if UNITY_EDITOR
using UnityEditor;
#endif

namespace Edwon.VR.Gesture
{
    public enum TutorialState { SetupVR, InVR };

    [ExecuteInEditMode]
    public class GettingStartedTutorial : MonoBehaviour
    {
        TutorialSettings tutorialSettings;
        public TutorialSettings TutorialSettings
        {
            get
            {
                if (tutorialSettings == null)
                {
                    tutorialSettings = new TutorialSettings();
                    return tutorialSettings;
                }
                return tutorialSettings;                
            }
            set
            {
                tutorialSettings = value;
            }
        }

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

        TutorialUIPanelManager panelManager;
        TutorialUIPanelManager PanelManager
        {
            get
            {
                if (panelManager == null)
                {
                    panelManager = GetComponentInChildren<TutorialUIPanelManager>();
                }
                return panelManager;
            }
        }

        Camera cameraUI;
        public Camera CameraUI
        {
            get
            {
                if (cameraUI == null)
                {
                    cameraUI = transform.GetComponentInChildren<Camera>();
                    return cameraUI;
                }
                else
                {
                    return cameraUI;
                }
            }
        }
        Canvas _canvas;
        Canvas Canvas
        {
            get
            {
                if (_canvas == null)
                {
                    _canvas = GetComponent<Canvas>();
                    return _canvas;
                }
                return _canvas;
            }
        }

        int inVRStep = 9; // starting at this step enter into VR

        public bool demoBuildMode; 

        void Start()
        {
#if UNITY_EDITOR
            // start - when play mode starts
            if (EditorApplication.isPlaying)
            {

                // if no file yet
                if (ReadTutorialSettings() == null)
                {
                    //if first time go to step to
                    GoToTutorialStep(2);
                }
                else
                {
                    RefreshTutorialSettings();
                }


                // load tutorial settings from file
                TutorialStateLogic(true);
            }

            // start - when edit mode starts
            if (!EditorApplication.isPlaying)
            {
                RefreshTutorialSettings();

                // if at the VR transition step
                if (TutorialSettings.currentTutorialStep == inVRStep)
                {
                    // enter VR
                    SwitchTutorialState(TutorialState.InVR);
                }
                else
                {
                    TutorialStateLogic(false);
                }

                RefreshTutorialSettings();
                GoToTutorialStep(TutorialSettings.currentTutorialStep);
            }
#else
            TutorialSettings.TUTORIAL_SAVE_PATH = Application.persistentDataPath + "TutorialSettings.txt";
            GoToTutorialStep(inVRStep+1);
            RefreshTutorialSettings();
            TutorialStateLogic(true);
#endif
        }

        public void TutorialStateLogic(bool includeStepLogic)
        {
            TutorialSettings = ReadTutorialSettings();
            if (TutorialSettings.currentTutorialStep == 1)
            {
                SwitchTutorialState(TutorialState.SetupVR);
                if (includeStepLogic)
                {
                    GoToTutorialStep(2);
                }
            }
            // set to vr setup mode
            else if (TutorialSettings.currentTutorialStep >= 1
                && TutorialSettings.currentTutorialStep < inVRStep)
            {
                SwitchTutorialState(TutorialState.SetupVR);
                if (includeStepLogic)
                {
                    GoToTutorialStep(TutorialSettings.currentTutorialStep);
                }
            }

            // if at the VR transition step
            else if (TutorialSettings.currentTutorialStep == inVRStep)
            {
#if UNITY_EDITOR
                if (PlayerSettings.virtualRealitySupported == true)
                {
                    // enter VR
                    SwitchTutorialState(TutorialState.InVR);
                    if (includeStepLogic)
                    {
                        GoToTutorialStep(inVRStep + 1);
                    }
                }
#endif
            }
            else if (TutorialSettings.currentTutorialStep >= inVRStep)
            {
                SwitchTutorialState(TutorialState.InVR);
                if (includeStepLogic)
                {
                    GoToTutorialStep(TutorialSettings.currentTutorialStep);
                }
            }
        }

        void Update()
        {
            if (UnityEngine.Input.GetKeyDown(KeyCode.RightArrow))
            {
                GoToTutorialStep(TutorialSettings.currentTutorialStep + 1);
            }
            if (UnityEngine.Input.GetKeyDown(KeyCode.LeftArrow))
            {
                if (TutorialSettings.currentTutorialStep - 1 >= 1)
                {
                    GoToTutorialStep(TutorialSettings.currentTutorialStep - 1);
                }
            }
        }

        public void GoToTutorialStep(int step)
        {
            TutorialSettings.currentTutorialStep = step;
            SaveTutorialSettings(TutorialSettings);

            PanelManager.FocusPanel(step.ToString());

            SaveTutorialSettings(tutorialSettings);
        }

        public void SwitchTutorialState(TutorialState state)
        {
            switch (state)
            {
                case TutorialState.SetupVR:
                    {
                        CameraUI.enabled = true;
                        GetComponent<EventSystem>().enabled = true;
                        Canvas.worldCamera = transform.GetComponentInChildren<Camera>();
                        #if UNITY_EDITOR
                        PlayerSettings.virtualRealitySupported = false;
                        #endif
                        GestureSettings.showVRUI = false;
                        if (GestureSettings.Rig != null)
                        {
                            #if EDWON_VR_OCULUS
                            GestureSettings.Rig.GetComponent<OVRCameraRig>().enabled = false;
                            GestureSettings.Rig.GetComponent<OVRManager>().enabled = false;
                            #endif
                            CameraUI.tag = "MainCamera";
                            GestureSettings.Rig.head.GetComponent<Camera>().tag = "Untagged";
                            GestureSettings.Rig.head.GetComponent<Camera>().enabled = false;
                            GestureSettings.Rig.enabled = false;
                        }
                    }
                    break;
                case TutorialState.InVR:
                    {
                        CameraUI.enabled = false;
                        GetComponent<EventSystem>().enabled = false;
                        // set the tutorial canvas UI camera to the VR ui camera
                        if (FindObjectOfType<VRGestureUI>() != null)
                        {
                            VRGestureUI ui = FindObjectOfType<VRGestureUI>();
                            LaserPointerInputModule laserPointerInput = ui.GetComponent<LaserPointerInputModule>();
                            Canvas.worldCamera = laserPointerInput.UICamera;
                        }
                        #if UNITY_EDITOR
                        PlayerSettings.virtualRealitySupported = true;
                        #endif
                        GestureSettings.showVRUI = true;
                        if (GestureSettings.Rig != null)
                        {
                            #if EDWON_VR_OCULUS
                            GestureSettings.Rig.GetComponent<OVRCameraRig>().enabled = true;
                            GestureSettings.Rig.GetComponent<OVRManager>().enabled = true;
                            #endif
                            #if EDWON_VR_STEAM
                            GestureSettings.Rig.transform.position = new Vector3(0, -1.25f, 1.25f);
                            GestureSettings.Rig.transform.rotation = Quaternion.identity;
                            #endif
                            CameraUI.tag = "Untagged";
                            GestureSettings.Rig.head.GetComponent<Camera>().tag = "MainCamera";
                            GestureSettings.Rig.head.GetComponent<Camera>().enabled = true;
                            GestureSettings.Rig.enabled = true;

                        }
                    }
                    break;
            }

            TutorialSettings.tutorialState = state;
            SaveTutorialSettings(TutorialSettings);
        }

        public void SaveTutorialSettings(TutorialSettings instance)
        {
            string json = JsonUtility.ToJson(instance, true);
            System.IO.File.WriteAllText(TutorialSettings.TUTORIAL_SAVE_PATH, json);
        }

        void RefreshTutorialSettings()
        {
            TutorialSettings = ReadTutorialSettings();
        }

        public TutorialSettings ReadTutorialSettings()
        {
            if (System.IO.File.Exists(TutorialSettings.TUTORIAL_SAVE_PATH))
            {
                string text = System.IO.File.ReadAllText(TutorialSettings.TUTORIAL_SAVE_PATH);
                return JsonUtility.FromJson<TutorialSettings>(text);
            }
            return null;
        }

#region BUTTONS

        public void OnRestartTutorial()
        {
            if (demoBuildMode)
            {
                GoToTutorialStep(inVRStep + 1);
            }
            else
            {
                GoToTutorialStep(1);
            }
        }

        public void OnButtonNext()
        {
            GoToTutorialStep(TutorialSettings.currentTutorialStep + 1);
        }

        public void OnButtonBack()
        {
            GoToTutorialStep(TutorialSettings.currentTutorialStep - 1);
        }

        public void OnOculusButton()
        {
            GestureSettings.vrType = VRType.OculusVR;
            Utils.ChangeVRType(VRType.OculusVR);
            OnButtonNext();
        }

        public void OnSteamButton()
        {
            GestureSettings.vrType = VRType.SteamVR;
            Utils.ChangeVRType(VRType.SteamVR);
            OnButtonNext();
        }

#endregion

    }
}