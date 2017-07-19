#if UNITY_EDITOR
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEditor;
using UnityEditor.SceneManagement;

namespace Edwon.VR.Gesture
{
    [InitializeOnLoad]
    public class GettingStartedTutorialInitializeOnLoad : MonoBehaviour
    {
        static string sceneName = null;
        static string gettingStartedTutorialSceneName = "Getting Started";

        static GettingStartedTutorialInitializeOnLoad()
        {
            EditorApplication.update += EditorUpdate;
        }

        static void EditorUpdate()
        {
            if (sceneName != EditorSceneManager.GetActiveScene().name)
            {
                sceneName = EditorSceneManager.GetActiveScene().name;
                if (sceneName == gettingStartedTutorialSceneName)
                {
                    if (!EditorApplication.isPlaying)
                    {
                        SetCamera2DForTutorial();
                        Debug.Log("PRESS PLAY TO BEGIN THE TUTORIAL!");
                        Debug.Log("if the scene starts in VR, reset the scene until it's 2D");
                        Debug.Log("make sure the 'Game' window is big enough to see the back and next buttons");
                    }
                }
            }
        }

        static void SetCamera2DForTutorial()
        {
            SceneView view = GetSceneView();
            if (view != null)
            {
                view.orthographic = true;
                view.in2DMode = true;
                GameObject tutorialGO = FindObjectOfType<GettingStartedTutorial>().gameObject;
                Selection.activeGameObject = tutorialGO;
                view.FrameSelected();
                view.LookAt(tutorialGO.transform.position, Quaternion.identity);
                view.size = 2f;
            }
        }

        static SceneView GetSceneView()
        {
            SceneView view = null;
            if (SceneView.lastActiveSceneView)
            {
                view = SceneView.lastActiveSceneView;
            }
            else if (SceneView.sceneViews.Count > 0)
            {
                view = (SceneView)SceneView.sceneViews[0];
            }
            //view.Focus();
            return view;
        }

    }
}
#endif