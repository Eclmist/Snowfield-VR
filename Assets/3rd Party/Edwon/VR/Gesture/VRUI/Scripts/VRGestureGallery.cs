using UnityEngine;
using System.Collections.Generic;
using Edwon.VR.Input;

namespace Edwon.VR.Gesture
{
    [RequireComponent(typeof (CanvasGroup))]
    public class VRGestureGallery : MonoBehaviour
    {
        public VRGestureGalleryGrid gridPrefab;
        public VRGestureGalleryExample examplePrefab;

        public List<VRGestureGalleryGrid> grids;
        public List<GestureExample> allExamples; // gesture examples for left and right hand


        public float distanceFromHead = 2f;
        public float gestureDrawSize = 40f; // local size of one gesture drawing
        public int gridMaxColumns = 10;
        public float lineWidth = 0.01f;
        public Vector3 galleryPosition;
        private Vector3 galleryStartPosition;
        public float grabVelocity = 650f;

        public Gesture currentGesture;
        [HideInInspector]
        public string currentNeuralNet;

        public enum GestureGalleryState { Visible, NotVisible };
        [HideInInspector]
        public GestureGalleryState galleryState;

        Rigidbody galleryRB;

        Transform vrHand; // the hand to use to grab and move the gallery
        VRGestureRig rig;
        IInput vrHandInput;
        VRGestureUI vrGestureUI;

        [HideInInspector]
        public CanvasGroup canvasGroup;
        private VRGestureSettings gestureSettings;

        // INIT
        void Start()
        {
            gestureSettings = Utils.GetGestureSettings();

            galleryPosition = new Vector3(0, 90, 0);

            canvasGroup = GetComponent<CanvasGroup>();

            galleryStartPosition = transform.position;

            vrGestureUI = transform.parent.GetComponent<VRGestureUI>();

            galleryRB = GetComponent<Rigidbody>();

            galleryState = GestureGalleryState.NotVisible;

            GetHands();
        }

        void GetHands()
        {
            //rig = VRGestureManager.Instance.rig;
            rig = VRGestureRig.GetPlayerRig(gestureSettings.playerID);
            vrHand = rig.GetHand(rig.mainHand);
            vrHandInput = rig.GetInput(rig.mainHand);
        }

        void RefreshGestureExamples()
        {
            allExamples = Utils.GetGestureExamples(currentGesture.name, currentNeuralNet);
            List<GestureExample> tmpList = new List<GestureExample>();
            foreach (GestureExample gesture in allExamples)
            {
                if (gesture.raw)
                {
                    gesture.data = Utils.SubDivideLine(gesture.data);
                    gesture.data = Utils.DownScaleLine(gesture.data);
                }

            }
        }

        void PositionGestureGallery()
        {
            // set position
            Vector3 forward = rig.head.forward;
            forward = Vector3.ProjectOnPlane(forward, Vector3.up);
            Vector3 position = rig.head.position + (forward * distanceFromHead);
            galleryRB.MovePosition( position );

            // set rotation
            Vector3 toHead = position - rig.head.position;
            Quaternion rotation = Quaternion.LookRotation(-toHead, Vector3.up);
            galleryRB.MoveRotation(rotation);
        }

        void PositionGestureGallery(Vector3 position)
        {
            galleryRB.MovePosition(position);
        }

        void DestroyGestureGalleryGrids()
        {
            galleryState = GestureGalleryState.Visible;
            galleryRB.MovePosition(galleryStartPosition);

            for (int i = grids.Count - 1; i >= 0; i--)
            {
                grids[i].DestroyThisGrid();
            }

            grids.Clear();
        }

        void CreateGestureGalleryGrids()
        {
            // if double handed
            if (currentGesture.isSynchronous)
            {
                // create two grids for left and right
                List<GestureExample> examplesR = FilterExamplesByHandedness(allExamples, Handedness.Right);
                CreateGestureGalleryGrid(examplesR, lineNumbersRightHand);
                List<GestureExample> examplesL = FilterExamplesByHandedness(allExamples, Handedness.Left);
                CreateGestureGalleryGrid(examplesL, lineNumbersLeftHand);
            }
            // if single handed
            else
            {
                // create one grid
                lineNumbersBothHands.Clear();
                for (int i = 0; i < allExamples.Count; i++)
                {
                    lineNumbersBothHands.Add(i);
                }
                CreateGestureGalleryGrid(allExamples, lineNumbersBothHands);
            }
        }

        void CreateGestureGalleryGrid(List<GestureExample> withExamples, List<int> lineNumbers)
        {
            if (withExamples != null && withExamples.Count > 0)
            {
                GameObject newGridGO = Instantiate(gridPrefab.gameObject);
                VRGestureGalleryGrid newGrid = newGridGO.GetComponent<VRGestureGalleryGrid>();

                newGridGO.transform.parent = transform;
                newGridGO.transform.position = transform.position;
                newGridGO.transform.rotation = transform.rotation;
                newGridGO.transform.localScale = Vector3.one;

                newGrid.Init(this, withExamples, lineNumbers);
                grids.Add(newGrid);
            }
        }

        public List<int> lineNumbersBothHands;
        public List<int> lineNumbersLeftHand;
        public List<int> lineNumbersRightHand;

        List<GestureExample> FilterExamplesByHandedness(List<GestureExample> _examples, Handedness handedness)
        {
            // first clear the lineNumbers list we're working with
            switch (handedness)
            {
                case Handedness.Left:
                    lineNumbersLeftHand.Clear();
                    break;
                case Handedness.Right:
                    lineNumbersRightHand.Clear();
                    break;
            }

            List<GestureExample> examplesFiltered = new List<GestureExample>();
            for (int i = _examples.Count - 1; i >= 0; i--)
            {
                if (_examples[i].hand == handedness)
                {
                    examplesFiltered.Add(_examples[i]);
                    switch (handedness)
                    {
                        case Handedness.Left:
                            lineNumbersLeftHand.Add(i);
                            break;
                        case Handedness.Right:
                            lineNumbersRightHand.Add(i);
                            break;
                    }
                }
            }
            return examplesFiltered;
        }

        public void DeleteGestureExample(GestureExample gestureExample, int lineNumber)
        {
            Gesture g = gestureSettings.FindGesture(gestureExample.name);
            g.exampleCount--;

            Utils.DeleteGestureExample(currentNeuralNet, gestureExample.name, lineNumber);
            allExamples.Remove(gestureExample);
            for (int i = grids.Count - 1; i >= 0 ; i--)
            {
                // delete the gesture example in the grid
                if (grids[i].examples.Contains(gestureExample))
                {
                    int gestureExampleIndex = grids[i].examples.IndexOf(gestureExample);
                    grids[i].examples.RemoveAt(gestureExampleIndex);
                }

                // delete the gesture gallery example in the grid
                for (int j = grids[i].galleryExamples.Count - 1; j >= 0; j--)
                { 
                    if (grids[i].galleryExamples[j].example == gestureExample)
                    {
                        Destroy(grids[i].galleryExamples[j].gameObject);
                        grids[i].galleryExamples.RemoveAt(j);
                    }
                }

                Vector3 lastPosition = galleryRB.position;
                DestroyGestureGalleryGrids();
                CreateGestureGalleryGrids();
                PositionGestureGallery(lastPosition);
            }
        }

        // GRAB AND MOVE THE GALLERY

        void FixedUpdate()
        {
            FixedUpdateGrabAndMove();
        }

        Vector3 lastHandPos; // used to calculate velocity of the vrHand to move the gesture gallery
        
        void FixedUpdateGrabAndMove()
        {
            if (galleryState == GestureGalleryState.Visible)
            {
                if (vrHandInput.GetButton(InputOptions.Button.Trigger2))
                {
                    // ADD UP/DOWN/LEFT/RIGHT FORCE
                    Vector3 velocity = vrHand.position - lastHandPos;
                    Vector3 velocityFlat = Vector3.ProjectOnPlane(velocity, -transform.forward);
                    velocityFlat *= grabVelocity;
                    galleryRB.AddForce(velocityFlat);

                    // ADD Z SPACE FORCE
                    Vector3 zVelocity = Vector3.ProjectOnPlane(velocity, -transform.right); // flatten left/right
                    zVelocity = Vector3.ProjectOnPlane(zVelocity, transform.up); // flatten up/down
                    zVelocity *= grabVelocity; // multiply
                    galleryRB.AddForce(zVelocity);
                }
            }
            lastHandPos = vrHand.position;
        }

        // EVENTS

        void OnEnable()
        {
            VRGestureUIPanelManager.OnPanelFocusChanged += PanelFocusChanged;
            gestureSettings = Utils.GetGestureSettings();
        }

        void OnDisable()
        {
            VRGestureUIPanelManager.OnPanelFocusChanged -= PanelFocusChanged;
        }

        void PanelFocusChanged(Panel panel)
        {
            if (panel.name == "Editing Menu")
            {
                Utils.ToggleCanvasGroup(canvasGroup, true);
                currentGesture = rig.currentTrainer.CurrentGesture;
                currentNeuralNet = gestureSettings.currentNeuralNet;
                RefreshGestureExamples();
                PositionGestureGallery();
                CreateGestureGalleryGrids();
            }
            else if (panel.name == "Gestures Menu")
            {
                Utils.ToggleCanvasGroup(canvasGroup, false);
                DestroyGestureGalleryGrids();
            }

        }

    }
}
