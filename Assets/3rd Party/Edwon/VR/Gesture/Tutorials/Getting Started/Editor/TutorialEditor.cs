using UnityEngine;
using UnityEditor;
using System.Collections;

namespace Edwon.VR.Gesture
{
    [CustomEditor(typeof(GettingStartedTutorial))]
    [CanEditMultipleObjects]
    public class GettingStartedTutorialEditor : Editor
    {
        GettingStartedTutorial tutorial;

        public override void OnInspectorGUI()
        {
            serializedObject.Update();
            tutorial = (GettingStartedTutorial)target;

            EditorGUILayout.LabelField("current tutorial state is: " + 
                tutorial.TutorialSettings.tutorialState.ToString());
            EditorGUILayout.LabelField("current tutorial step is: " + 
                tutorial.TutorialSettings.currentTutorialStep.ToString());

            SerializedProperty demoBuildMode = serializedObject.FindProperty("demoBuildMode");
            EditorGUILayout.PropertyField(demoBuildMode);

            if (GUILayout.Button("Restart Tutorial"))
            {
                tutorial.OnRestartTutorial();
            }

            if (GUILayout.Button("Next Step"))
            {
                tutorial.OnButtonNext();
            }

            if (GUILayout.Button("Previous Step"))
            {
                tutorial.OnButtonBack();
            }

            serializedObject.ApplyModifiedProperties();
        }
    }
}
