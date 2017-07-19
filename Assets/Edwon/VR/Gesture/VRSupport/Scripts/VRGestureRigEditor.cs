#if UNITY_EDITOR

using UnityEngine;
using UnityEditor;
using System.Collections;

namespace Edwon.VR
{
    [CustomEditor(typeof(VRGestureRig))]
    public class VRGestureRigEditor : Editor
    {
        SerializedProperty head;
        SerializedProperty handLeft;
        SerializedProperty handRight;
        SerializedProperty handLeftModel;
        SerializedProperty handRightModel;
        SerializedProperty mainHand;
        SerializedProperty gestureButton;
        SerializedProperty menuButton;
        SerializedProperty displayGestureTrail;
        SerializedProperty useCustomControllerModels;
        SerializedProperty playerID;

        void OnEnable()
        {

        }

        public override void OnInspectorGUI()
        {
            serializedObject.Update();

            head = serializedObject.FindProperty("head");
            handLeft = serializedObject.FindProperty("handLeft");
            handRight = serializedObject.FindProperty("handRight");
            handLeftModel = serializedObject.FindProperty("handLeftModel");
            handRightModel = serializedObject.FindProperty("handRightModel");
            mainHand = serializedObject.FindProperty("mainHand");
            gestureButton = serializedObject.FindProperty("gestureButton");
            menuButton = serializedObject.FindProperty("menuButton");
            displayGestureTrail = serializedObject.FindProperty("displayGestureTrail");
            useCustomControllerModels = serializedObject.FindProperty("useCustomControllerModels");
            //playerID = serializedObject.FindProperty("playerID");

            VRGestureRig vrGestureRig = (VRGestureRig)target;

            EditorGUILayout.LabelField("HINT: Float over variable names for tooltips");

            #if EDWON_VR_OCULUS || EDWON_VR_STEAM
            if (GUILayout.Button(new GUIContent("Auto Setup",
                "Press Auto Setup to automatically fill in the needed variables from the camera rig")))
            {
                vrGestureRig.AutoSetup();
            }
            #endif

            EditorGUILayout.PropertyField(head, new GUIContent("Head",
                "the head transform on the VR camera rig"));
            EditorGUILayout.PropertyField(handLeft, new GUIContent("Hand Left",
                "the left hand transform on the VR camera rig"));
            EditorGUILayout.PropertyField(handRight, new GUIContent("Hand Right",
                "the right hand transform on the VR camera rig"));
            EditorGUILayout.PropertyField(mainHand, new GUIContent("Main Hand", 
                "hand to record (single handed) gestures with, the VR UI will show up on the opposite hand of this one" ));
            EditorGUILayout.PropertyField(gestureButton, new GUIContent("Gesture Button", 
                "button to record/capture gestures with, see documentation for button mappings on Oculus vs. Vive"));
            EditorGUILayout.PropertyField(menuButton, new GUIContent("Menu Button",
                "button to use for clicking on the VRUI menu, see documentation for button mappings on Oculus vs. Vive"));
            //EditorGUILayout.PropertyField(playerID, new GUIContent("Player ID", 
            //    "Used for multiplayer support, coming soon, for now this should always be zero"));

            EditorGUILayout.PropertyField(displayGestureTrail, new GUIContent("Display Gesture Trail",
                "toggle this to turn off the default line that is drawn while recording/capturing gestures, see documentation for instructions on creating a custom trail"));

            vrGestureRig.spawnControllerModels = EditorGUILayout.Toggle(new GUIContent("Spawn Controller Models",
                "spawns controllers models (included with this plugin) so players can see their controllers in VR, unecessary if you want to use custom controller models or the platform defaults"),
                vrGestureRig.spawnControllerModels);

            if (vrGestureRig.spawnControllerModels)
            {
                EditorGUILayout.PropertyField(useCustomControllerModels, new GUIContent("Use Custom Controller Models",
                "You can use your own custom models, just toggle this on and drop your controller models in the fields below"));
                if (vrGestureRig.useCustomControllerModels)
                {
                    EditorGUILayout.PropertyField(handLeftModel);
                    EditorGUILayout.PropertyField(handRightModel);
                }
            }

            serializedObject.ApplyModifiedProperties();
        }
    }
}

#endif