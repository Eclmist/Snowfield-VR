using UnityEngine;
using UnityEditor;
using System.Collections;

namespace Edwon.VR.Gesture
{
    [CustomEditor(typeof(Panel))]
    public class PanelEditor : Editor
    {
        public override void OnInspectorGUI()
        {
            DrawDefaultInspector();

            Panel myPanel = (Panel)target;

            if (GUILayout.Button("Toggle Panel"))
            {
                if (myPanel.visible)
                {
                    myPanel.TogglePanelVisibility(false);
                }
                else
                {
                    myPanel.TogglePanelVisibility(true);
                }
            }
            if (GUILayout.Button("Solo Panel"))
            {
                myPanel.SoloPanelVisibility();
            }

        }
    }
}