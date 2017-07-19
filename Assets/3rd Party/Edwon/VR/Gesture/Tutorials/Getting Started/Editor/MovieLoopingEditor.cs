using UnityEngine;
using UnityEditor;
using System.Collections;

namespace Edwon.VR.Gesture
{
    [CustomEditor(typeof(MovieLooping))]
    public class MovieLoopingEditor : Editor
    {
        public override void OnInspectorGUI()
        {
            DrawDefaultInspector();

            MovieLooping movieLooping = (MovieLooping)target;

            if (GUILayout.Button("Play Movie"))
            {
                movieLooping.PlayMovie();
            }
            if (GUILayout.Button("Stop Movie"))
            {
                movieLooping.StopMovie();
            }
        }
    }
}