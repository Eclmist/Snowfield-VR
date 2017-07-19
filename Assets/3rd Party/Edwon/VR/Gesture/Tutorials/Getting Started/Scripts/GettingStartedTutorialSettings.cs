using UnityEngine;
using System;
using System.Collections;

namespace Edwon.VR.Gesture
{
    [Serializable]
    public class TutorialSettings
    {
        public string TUTORIAL_SAVE_PATH = @"Assets/Edwon/VR/Gesture/Tutorials/Getting Started/Settings/TutorialSettings.txt";
        public int currentTutorialStep = 1;
        public TutorialState tutorialState = TutorialState.SetupVR;
    }
}