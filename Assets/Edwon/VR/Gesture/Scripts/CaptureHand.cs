using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Edwon.VR.Input;
using System;

namespace Edwon.VR.Gesture
{
    public enum VRGestureCaptureState {EnteringCapture, ReadyToCapture, Capturing };

    public class CaptureHand {

        public VRGestureRig rig;
        IInput input;
        Transform playerHead;
        Transform playerHand;
        Transform perpTransform;

        Handedness hand;

        //Maybe have two states.
        //One that is: Record, Detect, Idle, Edit, Train
        //Another that is EnteringCapture, ReadyToCapture, Capturing
        public VRGestureCaptureState state;

        GestureTrail myTrail;
        List<Vector3> currentCapturedLine;

        float nextRenderTime = 0;
        float renderRateLimit = Config.CAPTURE_RATE;

        public string lastGesture;
        public DateTime lastDetected;

        public delegate void StartCapture();
        public event StartCapture StartCaptureEvent;
        public delegate void ContinueCapture(Vector3 capturePoint);
        public event ContinueCapture ContinueCaptureEvent;
        public delegate void StopCapture();
        public event StopCapture StopCaptureEvent;

        public CaptureHand (VRGestureRig _rig, Transform _perp, Handedness _hand, GestureTrail _myTrail = null)
        {
            rig = _rig;
            hand = _hand;
            playerHand = rig.GetHand(hand);
            playerHead = rig.head;
            perpTransform = _perp;
            input = rig.GetInput(hand);
            currentCapturedLine = new List<Vector3>();
            if (_myTrail != null)
            {
                myTrail = _myTrail;
                myTrail.AssignHand(this);
            }

            Start();
        }

        public bool CheckForSync(string gesture)
        {
            TimeSpan lapse = DateTime.Now.Subtract(lastDetected);
            TimeSpan limit = new TimeSpan(0, 0, 0, 0, 500);

            //if gesture starts with an R or an L.
            if(gesture.Contains("L--") || gesture.Contains("R--"))
            {
                //strip the gesture
                gesture = gesture.Substring(2);
                lastGesture = lastGesture.Substring(2);
            }


            if (gesture == lastGesture && lapse.CompareTo(limit) <= 0)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public void SetRecognizedGesture(string newGesture)
        {
            lastGesture = newGesture;
            lastDetected = DateTime.Now;
        }

        // Use this for initialization
        void Start() {
            if(myTrail != null)
            {
                //myTrail.AssignHand(this);
            }
            
        }

        public void LineCaught(List<Vector3> capturedLine)
        {
            rig.LineCaught(capturedLine, hand);
            //This should send out an EVENT.
            //Remove dependency from Instance.
        }

        //This will get points in relation to a users head.
        public Vector3 getLocalizedPoint(Vector3 myDumbPoint)
        {
            perpTransform.position = playerHead.position;
            perpTransform.rotation = Quaternion.Euler(0, playerHead.eulerAngles.y, 0);
            return perpTransform.InverseTransformPoint(myDumbPoint);
        }

        #region UPDATE
        // Update is called once per frame
        public void Update()
        {
            //get the position from the left anchor.
            //draw a point.

            if (rig != null)
            {

                if (rig.uiState == VRGestureUIState.Recording || rig.uiState == VRGestureUIState.ReadyToRecord)
                {
                    UpdateRecord();
                }
                else if (rig.uiState == VRGestureUIState.Detecting || rig.uiState == VRGestureUIState.ReadyToDetect)
                {
                    UpdateRecord();
                }
                else
                {
                    myTrail.ClearTrail();
                }
            }
        }

        void UpdateRecord()
        {
            // get input
            if(input == null)
            {
                input = rig.GetInput(hand);
            }

            // if not pressing gesture button stop recording
            if (!input.GetButton(rig.gestureButton))
            {
                state = VRGestureCaptureState.ReadyToCapture;
                if (input.GetButtonUp(rig.gestureButton))
                {
                    StopRecording();
                }
            }

            // if pressed button start recording
            if (input.GetButtonDown(rig.gestureButton) && state == VRGestureCaptureState.ReadyToCapture)
            {
                state = VRGestureCaptureState.Capturing;
                StartRecording();
            }

            // if capturing, capture points
            if (state == VRGestureCaptureState.Capturing)
            {
                CapturePoint();
            }

        }

        void StartRecording()
        {
            nextRenderTime = Time.time + renderRateLimit / 1000;
            if (StartCaptureEvent != null)
            {
                StartCaptureEvent();
            }
            CapturePoint();
        }

        void CapturePoint()
        {
            Vector3 rightHandPoint = playerHand.position;
            Vector3 localizedPoint = getLocalizedPoint(rightHandPoint);
            currentCapturedLine.Add(localizedPoint);
            if (ContinueCaptureEvent != null)
                ContinueCaptureEvent(rightHandPoint);
        }

        void StopRecording()
        {
            if (currentCapturedLine.Count > 0)
            {
                LineCaught(currentCapturedLine);
                currentCapturedLine.RemoveRange(0, currentCapturedLine.Count);
                currentCapturedLine.Clear();

                if (StopCaptureEvent != null)
                    StopCaptureEvent();
            }

        }

        #endregion
    }
}
