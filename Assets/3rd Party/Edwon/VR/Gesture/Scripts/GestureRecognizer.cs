using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

namespace Edwon.VR.Gesture
{
    public class GestureRecognizer
    {
        public delegate void GestureDetected(string gestureName, double confidence, Handedness hand, bool isDouble=false);
        public static event GestureDetected GestureDetectedEvent;
        public delegate void GestureRejected(string error, string gestureName = null, double confidence = 0);
        public static event GestureRejected GestureRejectedEvent;

        public VRGestureSettings gestureSettings;

        string lastLeftGesture;
        double lastLeftConfidenceValue;
        DateTime lastLeftDetected;
        string lastRightGesture;
        double lastRightConfidenceValue;
        DateTime lastRightDetected;

        public double confidenceThreshold = 0.9;
        public double currentConfidenceValue;
        public double minimumGestureAxisLength = 0.1;

        string LeftHandSyncPrefix = Handedness.Left + "--";
        string RightHandSyncPrefix = Handedness.Right + "--";

        List<Gesture> outputs;
        Dictionary<int, string> outputDict;
        NeuralNetwork neuralNet;
        //save the array of gestures
        //This should always require a name to load.
        public GestureRecognizer(string filename)
        {
            Load(filename);
        }

        //Load a SavedRecognizer from a file
        public void Load(string filename)
        {
            gestureSettings = Utils.GetGestureSettings();
            NeuralNetworkStub stub = Utils.ReadNeuralNetworkStub(filename);
            outputs = stub.gestures;
            BuildOutputDictionary();
            neuralNet = new NeuralNetwork(stub.numInput, stub.numHidden, stub.numOutput);
            neuralNet.SetWeights(stub.weights);
        }

        public void BuildOutputDictionary()
        {
            List<string> outputCount = new List<string>();
            foreach(Gesture g in outputs)
            {
                if (g.isSynchronous)
                {
                    outputCount.Add(LeftHandSyncPrefix+g.name);
                    outputCount.Add(RightHandSyncPrefix+g.name);
                }
                else
                {
                    outputCount.Add(g.name);
                }
            }
            outputDict = new Dictionary<int, string>();
            foreach (string gestureName in outputCount)
            {
                int gestureIndex = outputCount.IndexOf(gestureName);
                int binOut = 1 << gestureIndex;

                outputDict.Add(binOut, gestureName);
            }
        }

        //Almost all of this should get plugged into Recognizer
        public void RecognizeLine(List<Vector3> capturedLine, Handedness hand, VRGestureRig sender)
        {
            if (IsGestureBigEnough(capturedLine))
            {
                //Detect if the captured line meets minimum gesture size requirements
                double[] networkInput = Utils.FormatLine(capturedLine, hand);
                string gesture = GetGesture(networkInput);
                string confidenceValue = currentConfidenceValue.ToString("N3");

                // broadcast gesture detected event
                if (currentConfidenceValue > gestureSettings.confidenceThreshold)
                {
                    if (GestureDetectedEvent != null)
                    {
                        GestureDetectedEvent(gesture, currentConfidenceValue, hand);
                        //Check if the other hand has recently caught a gesture.
                        //CheckForSyncGestures(gesture, hand);
                        if (hand == Handedness.Left)
                        {
                            //leftCapture.SetRecognizedGesture(gesture);
                            lastLeftGesture = gesture;
                            lastLeftConfidenceValue = currentConfidenceValue;
                            lastLeftDetected = DateTime.Now;
                        }
                        else if (hand == Handedness.Right)
                        {
                            //rightCapture.SetRecognizedGesture(gesture);
                            lastRightGesture = gesture;
                            lastRightConfidenceValue = currentConfidenceValue;
                            lastRightDetected = DateTime.Now;

                        }

                        if (CheckForSync(gesture))
                        {
                            gesture = lastLeftGesture.Substring(LeftHandSyncPrefix.Length);
                            GestureDetectedEvent(gesture, (lastLeftConfidenceValue+lastRightConfidenceValue)/2, hand, true);
                        }
                    }

                }
                else
                {
                    if (GestureRejectedEvent != null)
                        GestureRejectedEvent("Confidence Too Low", gesture, currentConfidenceValue);
                }
            }
            else
            {
                //broadcast that a gesture is too small??
                if (GestureRejectedEvent != null)
                    GestureRejectedEvent("Gesture is too small");
            }
        }

        public bool IsGestureBigEnough(List<Vector3> capturedLine)
        {
            float check = Utils.FindMaxAxis(capturedLine);
            return (check > minimumGestureAxisLength);
        }

        public bool CheckForSync(string gesture)
        {
            //Check the diff in time between left and right timestamps.
            TimeSpan lapse = lastLeftDetected.Subtract(lastRightDetected).Duration();
            TimeSpan limit = new TimeSpan(0, 0, 0, 0, gestureSettings.gestureSyncDelay);

            //if gesture starts with an R or an L.
            string gestureA = lastLeftGesture;
            string gestureB = lastRightGesture;
            if (gesture != null)
            {
                if (gesture.Contains(LeftHandSyncPrefix) || gesture.Contains(RightHandSyncPrefix))
                {
                    //strip the gesture
                    if (lastLeftGesture != null)
                        gestureA = lastLeftGesture.Substring(LeftHandSyncPrefix.Length - 1);
                    if (lastRightGesture != null)
                        gestureB = lastRightGesture.Substring(RightHandSyncPrefix.Length - 1);
                }
            }

            if (gestureA == gestureB && lapse.CompareTo(limit) <= 0)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public string GetGesture(double[] input)
        {
            double[] outputVector = neuralNet.ComputeOutputs(input);
            int maxIndex = 0;
            double maxVal = 0;
            for (int i = 0; i < outputVector.Length; i++)
            {
                if (outputVector[i] > maxVal)
                {
                    maxIndex = i;
                    maxVal = outputVector[i];
                }
            }
            currentConfidenceValue = maxVal;

            int output = 1 << maxIndex;

            return outputDict[output];
        }

    }

}

