using UnityEngine;
using UnityEngine.UI;
using System.Collections;

namespace Edwon.VR.Gesture
{
    public class TutorialPanel : Panel
    {

        VRGestureSettings gestureSettings;
        GettingStartedTutorial tutorial;

        public override void Init()
        {
            base.Init();

            if (gestureSettings == null)
            {
                gestureSettings = Utils.GetGestureSettings();
            }
            if (tutorial == null)
            {
                tutorial = GetComponentInParent<GettingStartedTutorial>();
            }
        }

        public override void TogglePanelVisibility(bool _visible)
        {
            //base.TogglePanelVisibility(_visible);

            if (_visible == false)
            {
                ToggleEverything(canvasGroup, false);
                visible = false;
            }
            else
            {
                ToggleEverything(canvasGroup, true);
                visible = true;
            }
        }

        public override void SoloPanelVisibility()
        {
            base.SoloPanelVisibility();

            int step = int.Parse(gameObject.name);
            tutorial.TutorialSettings.currentTutorialStep = step;
            tutorial.GoToTutorialStep(step);
            tutorial.TutorialStateLogic(false);
            tutorial.SaveTutorialSettings(tutorial.TutorialSettings);
        }

        void ToggleEverything(CanvasGroup parentCG, bool enabled)
        {
            Init();

            if (gestureSettings.vrType == VRType.OculusVR)
            {
                if (ToggleByVRType(parentCG, enabled, VRType.OculusVR))
                {
                    ToggleByVRType(parentCG, false, VRType.SteamVR);
                    ToggleOtherStuff(parentCG.transform, enabled);
                }
                else
                {
                    ToggleNormally(parentCG, enabled);
                }
            }

            if (gestureSettings.vrType == VRType.SteamVR)
            {
                if (ToggleByVRType(parentCG, enabled, VRType.SteamVR))
                {
                    ToggleByVRType(parentCG, false, VRType.OculusVR);
                    ToggleOtherStuff(parentCG.transform, enabled);
                }
                else
                {
                    ToggleNormally(parentCG, enabled);
                }
            }

        }

        void ToggleNormally(CanvasGroup parentCG, bool enabled)
        {
            Utils.ToggleCanvasGroup(parentCG, enabled);
            ToggleChildMovies(parentCG.transform, enabled);
        }

        bool ToggleByVRType(CanvasGroup parentCG, bool enabled, VRType vrType)
        {
            CanvasGroup[] cgs = parentCG.GetComponentsInChildren<CanvasGroup>();
            foreach (CanvasGroup cg in cgs)
            {
                if (cg.gameObject.name == vrType.ToString())
                {
                    Utils.ToggleCanvasGroup(cg, enabled);
                    ToggleChildMovies(cg.transform, enabled);
                    return true;
                }
            }
            return false;
        }

        void ToggleChildMovies(Transform parent, bool enabled)
        {
            MovieLooping[] movies = parent.GetComponentsInChildren<MovieLooping>(true);
            if (movies != null)
            {
                foreach (MovieLooping movie in movies)
                {
                    movie.ToggleVisibility(enabled);
                    if (enabled)
                    {
                        movie.PlayMovie();
                    }
                    else
                    {
                        movie.StopMovie();
                    }
                }
            }

        }

        void ToggleOtherStuff(Transform parent, bool enabled)
        {
            if (parent.Find("Nav Buttons") != null)
            {
                CanvasGroup navButtonsCG = parent.Find("Nav Buttons").GetComponent<CanvasGroup>();
                Utils.ToggleCanvasGroup(navButtonsCG, enabled);
            }
            if (parent.Find("Caution Sign") != null)
            {
                CanvasGroup cautionSignCG = parent.Find("Caution Sign").GetComponent<CanvasGroup>();
                Utils.ToggleCanvasGroup(cautionSignCG, enabled);
            }
        }

    }
}
