using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;
#endif
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace Edwon.VR.Gesture
{
    public enum VRGestureUIState
    {
        Idle,
        Gestures,
        Editing,
        EnteringRecord,
        ReadyToRecord,
        Recording,
        Training,
        EnteringDetect,
        ReadyToDetect,
        Detecting
    };

    public enum VRGestureDetectType { Button, Continious };

    public class Utils
	{
		public static float FindMaxAxis(List<Vector3> capturedLine)
		{
			//find min and max for X,Y,Z
			float minX, maxX, minY, maxY, minZ, maxZ;
			//init all defaults to first point.
			Vector3 firstPoint = capturedLine[0];
			minX = maxX = firstPoint.x;
			minY = maxY = firstPoint.y;
			minZ = maxZ = firstPoint.z;

			foreach (Vector3 point in capturedLine)
			{
				minX = getMin(minX, point.x);
				maxX = getMax(maxX, point.x);

				minY = getMin(minY, point.y);
				maxY = getMax(maxY, point.y);

				minZ = getMin(minZ, point.z);
				maxZ = getMax(maxZ, point.z);
			}

			//we now have all of our mins and max
			float distX = Mathf.Abs(maxX - minX);
			float distY = Mathf.Abs(maxY - minY);
			float distZ = Mathf.Abs(maxZ - minZ);

			//FIND THE AXIS MAX. This will be the length for all of our AXIS.
			float axisMax = distX;
			axisMax = getMax(axisMax, distY);
			axisMax = getMax(axisMax, distZ);
			return axisMax;
		}

		//Same as DownRes line but this will scale, rather than skew.
		public static List<Vector3> DownScaleLine(List<Vector3> capturedLine)
		{
			//find min and max for X,Y,Z
			float minX, maxX, minY, maxY, minZ, maxZ;
			//init all defaults to first point.
			Vector3 firstPoint = capturedLine[0];
			minX = maxX = firstPoint.x;
			minY = maxY = firstPoint.y;
			minZ = maxZ = firstPoint.z;

			foreach (Vector3 point in capturedLine)
			{
				minX = getMin(minX, point.x);
				maxX = getMax(maxX, point.x);

				minY = getMin(minY, point.y);
				maxY = getMax(maxY, point.y);

				minZ = getMin(minZ, point.z);
				maxZ = getMax(maxZ, point.z);
			}

			//we now have all of our mins and max
			float distX = Mathf.Abs(maxX - minX);
			float distY = Mathf.Abs(maxY - minY);
			float distZ = Mathf.Abs(maxZ - minZ);

			//FIND THE AXIS MAX. This will be the length for all of our AXIS.
			float axisMax = distX;
			axisMax = getMax(axisMax, distY);
			axisMax = getMax(axisMax, distZ);

			//This should make all of our lowest points start at the origin.
			Matrix4x4 translate = Matrix4x4.identity;
			translate[0, 3] = -minX;
			translate[1, 3] = -minY;
			translate[2, 3] = -minZ;

			Matrix4x4 scale = Matrix4x4.identity;
			scale[0, 0] = 1 / axisMax;
			scale[1, 1] = 1 / axisMax;
			scale[2, 2] = 1 / axisMax;


			List<Vector3> localizedLine = new List<Vector3>();
			foreach (Vector3 point in capturedLine)
			{
				//we translate, but maybe we also divide each by the dist?
				Vector3 newPoint = translate.MultiplyPoint3x4(point);
				newPoint = scale.MultiplyPoint3x4(newPoint);
				localizedLine.Add(newPoint);
			}
			//capture way less points
			return localizedLine;
			// ok now we need to create a matrix to move all of these points to a normalized space.
		}

		//This is warping gestures to be on a scale of (0-1) in every axis
		//It stretches the gesture along each axis for how far it goes.
		public static List<Vector3> DownResLine(List<Vector3> capturedLine)
		{
			//find min and max for X,Y,Z
			float minX, maxX, minY, maxY, minZ, maxZ;
			//init all defaults to first point.
			Vector3 firstPoint = capturedLine[0];
			minX = maxX = firstPoint.x;
			minY = maxY = firstPoint.y;
			minZ = maxZ = firstPoint.z;

			foreach (Vector3 point in capturedLine)
			{
				minX = getMin(minX, point.x);
				maxX = getMax(maxX, point.x);

				minY = getMin(minY, point.y);
				maxY = getMax(maxY, point.y);

				minZ = getMin(minZ, point.z);
				maxZ = getMax(maxZ, point.z);
			}

			//we now have all of our mins and max
			float distX = Mathf.Abs(maxX - minX);
			float distY = Mathf.Abs(maxY - minY);
			float distZ = Mathf.Abs(maxZ - minZ);

			//This should make all of our lowest points start at the origin.
			Matrix4x4 translate = Matrix4x4.identity;
			translate[0, 3] = -minX;
			translate[1, 3] = -minY;
			translate[2, 3] = -minZ;

			Matrix4x4 scale = Matrix4x4.identity;
			scale[0, 0] = 1 / distX;
			scale[1, 1] = 1 / distY;
			scale[2, 2] = 1 / distZ;


			List<Vector3> localizedLine = new List<Vector3>();
			foreach (Vector3 point in capturedLine)
			{
				//we translate, but maybe we also divide each by the dist?
				Vector3 newPoint = translate.MultiplyPoint3x4(point);
				newPoint = scale.MultiplyPoint3x4(newPoint);
				localizedLine.Add(newPoint);
			}
			//capture way less points
			return localizedLine;
			// ok now we need to create a matrix to move all of these points to a normalized space.
		}

		public static float getMin(float min, float newMin)
		{
			if (newMin < min) { min = newMin;}
			return min;
		}

		public static float getMax(float max, float newMax)
		{
			if (newMax > max) { max = newMax;}
			return max;
		}

		public static List<Vector3> SubDivideLine(List<Vector3> capturedLine)
		{
			//Make sure list is longer than 11.
			int outputLength = Config.FIDELITY;

			float intervalFloat = Mathf.Round((capturedLine.Count * 1f) / (outputLength * 1f));
			int interval = (int)intervalFloat;
			List<Vector3> output = new List<Vector3>();

			for (int i = capturedLine.Count - 1; output.Count < outputLength; i -= interval)
			{
				if (i > 0) { output.Add(capturedLine[i]);}
				else { output.Add(capturedLine[0]); }
			}
			return output;
		}

		//Format line for NeuralNetwork
		//Might want to pull this out from saving lines.
		//Save huge raw vector lines.
		//Run formatting on them during training.
		//Allow users to changes and train different formats on
		//the same data set.
		public static double[] FormatLine(List<Vector3> capturedLine, Handedness hand)
		{
			capturedLine = SubDivideLine(capturedLine);
			if (Config.USE_RAW_DATA)
			{
				capturedLine = DownScaleLine(capturedLine);
			}
			else
			{
				capturedLine = DownResLine(capturedLine);
			}
			
			List<double> tmpLine = new List<double>();
			tmpLine.Add((int)hand);
			foreach (Vector3 cVector in capturedLine)
			{
				tmpLine.Add(cVector.x);
				tmpLine.Add(cVector.y);
				tmpLine.Add(cVector.z);
			}
			return tmpLine.ToArray();
		}

		public static NeuralNetworkStub ReadNeuralNetworkStub(string networkName)
		{
			string path = Application.streamingAssetsPath + Config.NEURAL_NET_PATH + networkName + "/" + networkName + ".txt";
			if (System.IO.File.Exists(path))
			{
				string[] lines = System.IO.File.ReadAllLines(path);
				NeuralNetworkStub stub = JsonUtility.FromJson<NeuralNetworkStub>(string.Concat(lines));
				return stub;
			}
			else
			{
				NeuralNetworkStub stub = new NeuralNetworkStub();
				stub.gestures = new List<Gesture>();
				return stub;
			}
		}

		//For some reason this is not called from anywhere @deprecated?
		public static List<string> GetNetworksFromFile()
		{
			List<string> networkList = new List<string>();
			string networkPath = Application.streamingAssetsPath + Config.NEURAL_NET_PATH;
			//string[] files = System.IO.Directory.GetFiles(gesturesPath, "*.txt");
			string[] files = System.IO.Directory.GetDirectories(networkPath);
			if (files.Length == 0)
			{
				Debug.Log("no gestures files (recorded data) yet");
				return null;
			}
			foreach (string path in files)
			{
				//paramschar[] sep = { '/'};
				char[] stringSeparators = new char[] { '/' };
				string[] exploded = path.Split(stringSeparators);
				string finalString = exploded[exploded.Length - 1];
				networkList.Add(finalString);
			}
			return networkList;
		}

        //This one now just reads from the gesture bank file.
		public static List<Gesture> GetGestureBankOld(string networkName)
		{
			List<Gesture> gestureBank = new List<Gesture>();
			string gesturesPath = Application.streamingAssetsPath + Config.NEURAL_NET_PATH + networkName + "/gestures/";
			//Check if path exists
			if (System.IO.Directory.Exists(gesturesPath))
			{
				string[] files = System.IO.Directory.GetFiles(gesturesPath, "*.txt");
				if (files.Length == 0)
				{
					return gestureBank;
				}
				foreach (string path in files)
				{
					//paramschar[] sep = { '/'};
					char[] stringSeparators = new char[] { '/' };
					string[] exploded = path.Split(stringSeparators);
					string iCareAbout = exploded[exploded.Length - 1];
					//scrub file extension
					int substrIndex = iCareAbout.LastIndexOf('.');
					string finalString = iCareAbout.Substring(0, substrIndex);
                    Gesture newGesture = new Gesture();
                    newGesture.name = finalString;
                    //Debug.Log(newGesture.name);
					gestureBank.Add(newGesture);
				}
			}
            Debug.Log("GetGestureBankOld: " + gestureBank.Count);
            return gestureBank;
		}

        public static void SaveGestureBank(List<Gesture> gestureBank, string networkName)
        {
            GestureBankStub stub = new GestureBankStub();
            stub.gestures = gestureBank;
            string filePath = Application.streamingAssetsPath + Config.NEURAL_NET_PATH + networkName + "/GestureBank.txt";
            using (System.IO.StreamWriter file = new System.IO.StreamWriter(filePath, false))
            {
                //file.WriteLine(dumbString);
                file.WriteLine(JsonUtility.ToJson(stub, true));
            }
#if UNITY_EDITOR
            AssetDatabase.ImportAsset(filePath);
#endif
        }

        //This one now just reads from the gesture bank file.
        public static List<Gesture> GetGestureBank(string networkName)
        {
            List<Gesture> gestureBank = new List<Gesture>();
            string gesturesPath = Application.streamingAssetsPath + Config.NEURAL_NET_PATH + networkName + "/GestureBank.txt";
            string gesturesFolderPath = Application.streamingAssetsPath + Config.NEURAL_NET_PATH + networkName + "/Gestures/";
            //Check if path exists
            if (System.IO.File.Exists(gesturesPath))
            {
                string[] lines = System.IO.File.ReadAllLines(gesturesPath);
                GestureBankStub stub = JsonUtility.FromJson<GestureBankStub>(string.Concat(lines));
                return stub.gestures;
            }
            else if (System.IO.Directory.Exists(gesturesFolderPath))
            {
                return GetGestureBankOld(networkName);
            }
            else
            {
                GestureBankStub stub = new GestureBankStub();
                stub.gestures = new List<Gesture>();
                Debug.Log("GetGestureBank: " + stub.gestures.Count);
                return stub.gestures;
            }
        }

        //Consider refactoring this by creating an actual GestureBank class.
        public static List<int> GetGestureBankTotalExamples(List<Gesture> gestureList, string networkName)
		{
			List<int> totals = new List<int>();
			foreach(Gesture gesture in gestureList)
			{
				int total = GetGestureExamplesTotal(gesture, networkName);
                gesture.exampleCount = total;
				totals.Add(total);
			}
			return totals;
		}

		public static void CreateGestureFile(string gestureName, string networkName)
		{
			string gestureFileLocation = Application.streamingAssetsPath + Config.NEURAL_NET_PATH + networkName + "/Gestures/";
			// if no gestures folder already
			if (!System.IO.Directory.Exists(gestureFileLocation))
			{
				// create gestures folder
				System.IO.Directory.CreateDirectory(gestureFileLocation);
			}

			// create the gesture file
			string fullPath = gestureFileLocation + gestureName + ".txt";
			System.IO.StreamWriter file = new System.IO.StreamWriter(fullPath, true);
			file.Dispose();
			
#if UNITY_EDITOR
			AssetDatabase.ImportAsset(fullPath);
#endif
		}

		public static void DeleteGestureFile(string gestureName, string networkName)
		{
			string gestureFileLocation = Application.streamingAssetsPath + Config.NEURAL_NET_PATH + networkName + "/Gestures/" + gestureName + ".txt";
#if UNITY_EDITOR
			FileUtil.DeleteFileOrDirectory(gestureFileLocation);
			AssetDatabase.Refresh();
#endif
		}

		public static void DeleteGestureExample(string neuralNetwork, string gesture, int lineNumber)
		{
			string gestureFileLocation = Application.streamingAssetsPath + Config.NEURAL_NET_PATH + neuralNetwork + "/Gestures/" + gesture + ".txt"; ;
			List<string> tmpLines = new List<string>();
			tmpLines.AddRange(System.IO.File.ReadAllLines(gestureFileLocation));
			tmpLines.RemoveAt(lineNumber);
			string[] lines = tmpLines.ToArray();

			System.IO.File.WriteAllLines(gestureFileLocation, lines);
		}

		public static void RenameGestureFile(string gestureOldName, string gestureNewName, string networkName)
		{
			string oldPath = Application.streamingAssetsPath + Config.NEURAL_NET_PATH + networkName + "/Gestures/" + gestureOldName + ".txt";
			string newPath = Application.streamingAssetsPath + Config.NEURAL_NET_PATH + networkName + "/Gestures/" + gestureNewName + ".txt";
			//get all them old gesture
			string[] oldLines = System.IO.File.ReadAllLines(oldPath);
			List<string> newLines = new List<string>();
			foreach(string line in oldLines)
			{
				GestureExample currentGest = JsonUtility.FromJson<GestureExample>(line);
				currentGest.name = gestureNewName;
				newLines.Add(JsonUtility.ToJson(currentGest));
			}
			System.IO.File.WriteAllLines(newPath, newLines.ToArray());
			DeleteGestureFile(gestureOldName, networkName);
		}

		public static void ChangeGestureName(string gestureNameOld, string gestureNameNew, string networkName)
		{
			string path = Application.streamingAssetsPath + Config.NEURAL_NET_PATH + networkName + "/Gestures/" + gestureNameOld + ".txt";
#if UNITY_EDITOR
			AssetDatabase.RenameAsset(path, gestureNameNew);
			string pathUpdated = Application.streamingAssetsPath + Config.NEURAL_NET_PATH + networkName + "/Gestures/" + gestureNameNew + ".txt";
//			AssetDatabase.ImportAsset(pathUpdated);
			AssetDatabase.Refresh();
#endif
		}

		public static List<string> GetGestureFiles(string networkName)
		{
			string gesturesFilePath = Application.streamingAssetsPath + Config.NEURAL_NET_PATH + networkName + "/Gestures/";
			string[] files = System.IO.Directory.GetFiles(gesturesFilePath, "*.txt");
			return files.ToList<string>();
		}

		// get all the examples of this gesture and store in a GestureExample list
		public static List<GestureExample> GetGestureExamples(string gesture, string networkName)
		{
			string[] lines = GetGestureLines(gesture, networkName);
			List<GestureExample> gestures = new List<GestureExample>();
			foreach (string currentLine in lines)
			{
				gestures.Add(JsonUtility.FromJson<GestureExample>(currentLine));
			}
			return gestures;
		}

		// get the total amount of examples of this gesture
		public static int GetGestureExamplesTotal(Gesture gesture, string networkName)
		{
			string[] lines = GetGestureLines(gesture.name, networkName);
			return lines.Length;
		}

		private static string[] GetGestureLines(string gesture, string networkName)
		{
			//read in the file
			string filePath = Application.streamingAssetsPath + Config.NEURAL_NET_PATH + networkName + "/Gestures/";
			string fileName = gesture + ".txt";
			string[] lines = System.IO.File.ReadAllLines(filePath + fileName);
			return lines;
		}

		public static void DeleteNeuralNetFiles(string networkName)
		{
			string path = Application.streamingAssetsPath + Config.NEURAL_NET_PATH + networkName + "/";
			if (System.IO.Directory.Exists(path))
			{
#if UNITY_EDITOR
				FileUtil.DeleteFileOrDirectory(path);
				AssetDatabase.DeleteAsset(path);
#endif
			}
#if UNITY_EDITOR
			AssetDatabase.Refresh();
#endif
		}

		// create a folder in the save file path
		// return true if successful, false if not
		public static bool CreateFolder (string path)
		{
			string folderPathNew = Application.streamingAssetsPath + Config.NEURAL_NET_PATH + path;
			System.IO.Directory.CreateDirectory(folderPathNew);
#if UNITY_EDITOR
			AssetDatabase.ImportAsset(folderPathNew);
#endif
			return true;
		}

        public static void CheckCreateNeuralNetFolder()
        {
            string directoryPath = Application.streamingAssetsPath + Config.NEURAL_NET_PATH;
            if (!Directory.Exists(directoryPath))
            {
                Directory.CreateDirectory(directoryPath);
                #if UNITY_EDITOR
                AssetDatabase.Refresh();
                #endif
            }
        }

        public static void ChangeVRType(VRType vrType)
        {
            #if UNITY_EDITOR
            string defines = PlayerSettings.GetScriptingDefineSymbolsForGroup(BuildTargetGroup.Standalone);
            List<string> definesList = defines.Split(new char[] { ';' }).ToList<string>();

            // go through all the defines and edit the edwon vr specific ones to the new vr type
            for (int i = definesList.Count-1; i >= 0; i--)
            {
                switch (definesList[i])
                {
                    case "EDWON_VR_OCULUS":
                        {
                            if (vrType == VRType.SteamVR)
                            {
                                definesList[i] = "EDWON_VR_STEAM";
                            }
                        }
                        break;
                    case "EDWON_VR_STEAM":
                        {
                            if (vrType == VRType.OculusVR)
                            {
                                definesList[i] = "EDWON_VR_OCULUS";
                            }
                        }
                        break;
                }
            }

            switch (vrType)
            {
                case VRType.SteamVR:
                    {
                        if (!definesList.Contains("EDWON_VR_STEAM"))
                            definesList.Add("EDWON_VR_STEAM");
                    }
                    break;
                case VRType.OculusVR:
                    {
                        if (!definesList.Contains("EDWON_VR_OCULUS"))
                            definesList.Add("EDWON_VR_OCULUS");
                    }
                    break;
            }

            defines = String.Join(";", definesList.ToArray());

            PlayerSettings.SetScriptingDefineSymbolsForGroup(BuildTargetGroup.Standalone, defines);
            #endif
        }

        public static VRGestureSettings GetGestureSettings()
        {
            #if UNITY_EDITOR
            return AssetDatabase.LoadAssetAtPath(Config.SETTINGS_FILE_PATH, typeof(VRGestureSettings)) as VRGestureSettings;
            #else
            Debug.Log("GET GESTURE SETTINGS FROM RESOURCES");
            return Resources.Load(Config.PARENT_PATH + "Settings/Settings", typeof(VRGestureSettings)) as VRGestureSettings;
            #endif
        }

        #region UI

        public static void ToggleCanvasGroup(CanvasGroup cg, bool on)
        {
            if (on)
            {
                // turn panel on
                cg.alpha = 1f;
                cg.interactable = true;
                cg.blocksRaycasts = true;
            }
            else
            {
                // turn panel off
                cg.alpha = 0f;
                cg.interactable = false;
                cg.blocksRaycasts = false;
            }
        }

        public static void ToggleCanvasGroup(CanvasGroup cg, bool on, float alpha)
        {
            if (on)
            {
                // turn panel on
                cg.alpha = alpha;
                cg.interactable = true;
                cg.blocksRaycasts = true;
            }
            else
            {
                // turn panel off
                cg.alpha = alpha;
                cg.interactable = false;
                cg.blocksRaycasts = false;
            }
        }

#endregion
    }

}

namespace Edwon.VR.Gesture
{
	[Serializable]
	public class GestureExample
	{
		public string name;
		public bool trained;
		public bool raw;
        public bool isSynchronous = false;
		public Handedness hand = Handedness.Right;
        public List<Vector3> data;

        public double[] GetAsArray()
		{
			List<double> tmpLine = new List<double>();
			//gestures.Add(JsonUtility.FromJson<GestureExample>(currentLine));
			foreach (Vector3 currentPoint in data)
			{
				tmpLine.Add(currentPoint.x);
				tmpLine.Add(currentPoint.y);
				tmpLine.Add(currentPoint.z);
			}
			return tmpLine.ToArray();
		}
	}

	[Serializable]
	public class Gesture
	{
		public string name;
		public Handedness hand = Handedness.Right;
		public bool isSynchronous = false;
		public int exampleCount = 0;

        public Gesture Clone()
        {
            Gesture g = new Gesture();
            g.name = name;
            g.hand = hand;
            g.isSynchronous = isSynchronous;
            g.exampleCount = exampleCount;
            return g;
        }
	}


    [Serializable]
    public class GestureBankStub
    {
        public List<Gesture> gestures;
    }

    [Serializable]
	public class NeuralNetworkStub
	{
		public int numInput;
		public int numHidden;
		public int numOutput;
		public List<Gesture> gestures;
		public double[] weights;
		//confidenceThreshold
		//minimumGestureLength
	}
}


