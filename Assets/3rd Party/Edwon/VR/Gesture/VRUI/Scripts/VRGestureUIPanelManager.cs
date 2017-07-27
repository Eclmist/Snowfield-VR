using UnityEngine;
using System.Collections.Generic;

namespace Edwon.VR.Gesture
{
    // this script should be placed on the panels parent
    public class VRGestureUIPanelManager : PanelManager
    {
        private VRGestureSettings gestureSettings;

        public string initialPanel;

        new public void Awake()
        {
            base.Awake();

            gestureSettings = Utils.GetGestureSettings();

            if (gestureSettings.stateInitial == VRGestureUIState.ReadyToDetect)
            {
                initialPanel = "Detect Menu";
            }

            // initialize initial panel focused
            if (gestureSettings.neuralNets.Count <= 0)
                FocusPanel("No Neural Net Menu");
            else
                FocusPanel(initialPanel);
        }

    }
}