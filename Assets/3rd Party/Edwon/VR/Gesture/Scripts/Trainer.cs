using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;
#endif
using System.Collections;
using System.Collections.Generic;

namespace Edwon.VR.Gesture
{

    public class Trainer
    {
        //This class will create and train a NeuralNetwork.
        //maybe we pass in a Neural Network
        int numInput;
        int numHidden;
        int numOutput;



        //maybe the trainer is where we need an output of gestures
        Dictionary<string, double[]> outputDict;
        List<string> outputs;
        List<Gesture> gestures;
        public Gesture CurrentGesture { get; set; }
        string recognizerName;

        NeuralNetwork neuralNetwork;

        //The current trainer should keep track of  the Gestures and GestureBank.
        //It should manage all DATA sets etc.
        //Should keep a table of every gesture example types and total counts of each gesture.
        //Should not require a pass in for the gestureList.
        //we almost always want the gesture bank.

        public Trainer( string name, List<Gesture> gestureList = null)
        {
            outputs = new List<string>();

            numInput = 34;
            //This should be a number between input and output.
            //numHidden = 10;
            //AbsVal of numInput-numOutput + min of numInput/numOutput
            numHidden = 10;
            //numHidden = Mathf.Abs(numInput - numOutput) / 3 + System.Math.Min(numInput, numOutput);
            numOutput = 3;
            //numOutput = gestureList.Count;
            recognizerName = name;
            if(gestureList == null)
            {
                //outputs = VRGestureManager.Instance.s.gestureBank;
            }
            else
            {
                gestures = gestureList;
                foreach(Gesture g in gestureList)
                {
                    if (g.isSynchronous)
                    {
                        //add it to the dictionary.
                        outputs.Add(Handedness.Left+"--"+g.name);
                        outputs.Add(Handedness.Right+"--"+g.name);
                    }
                    else
                    {
                        outputs.Add(g.name);
                    }
                }
            }
            BuildOutputDictionary();
        }

        public void BuildOutputDictionary()
        {
            outputDict = new Dictionary<string, double[]>();
            foreach(string gestureName in outputs)
            {
                int gestureIndex = outputs.IndexOf(gestureName);

                //Create output of length numOutputs, zero it out.
                double[] output = new double[outputs.Count];
                for (int i = 0; i < output.Length; i++)
                {
                    output[i] = 0.0;
                }
                output[gestureIndex] = 1.0;
                outputDict.Add(gestureName, output);
            }
        }

        public void Load()
        {

        }

        public void TrainLine(List<Vector3> capturedLine, Handedness hand)
        {
            AddGestureToTrainingExamples(capturedLine, hand);
        }

        //Just Capture Data
        //Pass in an array for data points.
        public void AddGestureToTrainingExamples(List<Vector3> capturedLine, Handedness hand)
        {
            string gestureFileLocation = Application.streamingAssetsPath + Config.NEURAL_NET_PATH + recognizerName + "/Gestures/";
            //we need to check if this directory exists.
            //if not we need to create the directory and file.
            System.IO.Directory.CreateDirectory(gestureFileLocation);

            if (capturedLine.Count >= 11)
            {
                if (!Config.USE_RAW_DATA)
                {
                    capturedLine = Utils.SubDivideLine(capturedLine);
                    capturedLine = Utils.DownResLine(capturedLine);
                }

                GestureExample saveMe = new GestureExample();
                saveMe.name = CurrentGesture.name;
                saveMe.data = capturedLine;
                saveMe.hand = hand;
                saveMe.raw = Config.USE_RAW_DATA;
                saveMe.isSynchronous = CurrentGesture.isSynchronous;
                //System.IO.StreamWriter file = new System.IO.StreamWriter(gestureFileLocation + gestureName + ".txt", true);
                using (System.IO.StreamWriter file = new System.IO.StreamWriter(gestureFileLocation + CurrentGesture.name + ".txt", true))
                {
                    file.WriteLine(JsonUtility.ToJson(saveMe));
                }
                CurrentGesture.exampleCount++;
            }
        }

        //Then Actually Train
        public void TrainRecognizer()
        {
            //Based on out list of outputs
            numOutput = outputs.Count;
            int seed = 1; // gives nice demo

            double[][] allData = ReadAllData();

            double[][] trainData;
            double[][] testData;
            SplitTrainTest(allData, 0.80, seed, out trainData, out testData);

            neuralNetwork = new NeuralNetwork(numInput, numHidden, numOutput);

            int maxEpochs = 1000;
            double learnRate = 0.05;
            double momentum = 0.01;

            //Does this still weight properly if I train A SINGLE example at a time.
            double[] weights = neuralNetwork.Train(trainData, maxEpochs, learnRate, momentum);
            SaveNeuralNetwork(weights);
        }


        public double[][] ReadAllData()
        {
            //read in the file
            //technically this should only read files that are also in the gestures list.
            //@TODO - compare files in gestures folder to  ones in list.
            string gesturesFilePath = Application.streamingAssetsPath + Config.NEURAL_NET_PATH + recognizerName + "/Gestures/";
            if (!System.IO.Directory.Exists(gesturesFilePath))
            {
                Debug.Log("No recorded gestures. Please record some gestures in VR.");
                return null;
            }
            string[] files = System.IO.Directory.GetFiles(gesturesFilePath, "*.txt");

            List<string> tmpLines = new List<string>();
            foreach (string fileLocation in files)
            {
                tmpLines.AddRange(System.IO.File.ReadAllLines(fileLocation));
            }
            string[] lines = tmpLines.ToArray();

            double[][] readData = new double[lines.Length][];
            List<double[]> tmpAllData = new List<double[]>();

            foreach (string currentLine in lines)
            {
                //Gesture Example will need to know if it is Sync.
                GestureExample myObject = JsonUtility.FromJson<GestureExample>(currentLine);
                if (Config.USE_RAW_DATA)
                {
                    myObject.data = Utils.SubDivideLine(myObject.data);
                    myObject.data = Utils.DownScaleLine(myObject.data);
                }

                List<double> tmpLine = new List<double>();
                //First Add All Inputs
                tmpLine.Add((int)myObject.hand);
                tmpLine.AddRange(myObject.GetAsArray());
                tmpLine.AddRange(FindOutputVector(myObject.name, myObject.hand, myObject.isSynchronous));

                tmpAllData.Add(tmpLine.ToArray());
            }

            return tmpAllData.ToArray();
        }

        //Should just be a Dict<string, double[]>,  Calculate all of these at the start and save them.
        public double[] FindOutputVector(string gestureName, Handedness hand, bool isSynchronized)
        {
            if (isSynchronized)
            {
                gestureName = hand + "--" + gestureName;
            }

            return outputDict[gestureName];
        }

        void SplitTrainTest(double[][] allData, double trainPct, int seed, out double[][] trainData, out double[][] testData)
        {
            System.Random rnd = new System.Random(seed);
            int totRows = allData.Length;
            int numTrainRows = (int)(totRows * trainPct); // usually 0.80
            int numTestRows = totRows - numTrainRows;
            trainData = new double[numTrainRows][];
            testData = new double[numTestRows][];

            double[][] copy = new double[allData.Length][]; // ref copy of data
            for (int i = 0; i < copy.Length; ++i)
                copy[i] = allData[i];

            //This is duping rows. Not deep copy
            for (int i = 0; i < copy.Length; ++i) // scramble order
            {
                int r = rnd.Next(i, copy.Length); // use Fisher-Yates
                double[] tmp = copy[r];
                copy[r] = copy[i];
                copy[i] = tmp;
            }
            for (int i = 0; i < numTrainRows; ++i)
                trainData[i] = copy[i];

            for (int i = 0; i < numTestRows; ++i)
                testData[i] = copy[i + numTrainRows];
        } // SplitTrainTest

        public void SaveNeuralNetwork(double[] weights)
        {
            NeuralNetworkStub stub = new NeuralNetworkStub();
            stub.numInput = numInput;
            stub.numHidden = numHidden;
            stub.numOutput = numOutput;
            stub.gestures = gestures;
            stub.weights = weights;
            string filePath = Application.streamingAssetsPath + Config.NEURAL_NET_PATH + recognizerName + "/" + recognizerName+".txt";
            using (System.IO.StreamWriter file = new System.IO.StreamWriter(filePath, false))
            {
                //file.WriteLine(dumbString);
                file.WriteLine(JsonUtility.ToJson(stub, true));
            }
#if UNITY_EDITOR
            AssetDatabase.ImportAsset(filePath);
#endif
        }

    }

}


