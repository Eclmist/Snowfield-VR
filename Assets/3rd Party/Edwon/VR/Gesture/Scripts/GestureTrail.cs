using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace Edwon.VR.Gesture
{
    
    public class GestureTrail : MonoBehaviour
    {
        CaptureHand registeredHand;
        int lengthOfLineRenderer = 50;
        List<Vector3> displayLine;
        List<Vector3> copyLine;
        LineRenderer currentRenderer;
		LineRenderer endRenderer;

        Color c1;
        Color c2;

        public bool listening;

        bool currentlyInUse;

        bool fading;

        // Use this for initialization
        void Start()
        {
            currentlyInUse = true;
            displayLine = new List<Vector3>();
            copyLine = new List<Vector3>();
			currentRenderer = CreateLineRenderer(Color.yellow, Color.yellow);

            c1 = new Color(0, 0, 1, 1);
            c2 = new Color(0, 1, 1, 1);

			endRenderer = CreateLineRenderer (c1, c2);
        }

        void OnEnable()
        {
            if(registeredHand != null)
            {
                SubscribeToEvents();
            }
        }

        void SubscribeToEvents()
        {
            registeredHand.StartCaptureEvent += StartTrail;
            registeredHand.ContinueCaptureEvent += CapturePoint;
            registeredHand.StopCaptureEvent += StopTrail;
        }

        void OnDisable()
        {
            if (registeredHand != null)
            {
                UnsubscribeFromEvents();
            }
        }

        void UnsubscribeFromEvents()
        {
            registeredHand.StartCaptureEvent -= StartTrail;
            registeredHand.ContinueCaptureEvent -= CapturePoint;
            registeredHand.StopCaptureEvent -= StopTrail;
        }

        void UnsubscribeAll()
        {
            
        }

        void OnDestroy()
        {
            currentlyInUse = false;
        }

        LineRenderer CreateLineRenderer(Color color1, Color color2)
        {
            GameObject myGo = new GameObject("Trail Renderer");
            myGo.transform.parent = transform;
            myGo.transform.localPosition = Vector3.zero;

            LineRenderer lineRenderer = myGo.AddComponent<LineRenderer>();
            lineRenderer.material = new Material(Shader.Find("Particles/Additive"));
            lineRenderer.SetColors(color1, color2);
            lineRenderer.SetWidth(0.01F, 0.05F);
            lineRenderer.SetVertexCount(0);
            return lineRenderer;
        }

        public void RenderTrail(LineRenderer lineRenderer, List<Vector3> capturedLine)
        {
            if (capturedLine.Count == lengthOfLineRenderer)
            {
                lineRenderer.SetVertexCount(lengthOfLineRenderer);
                lineRenderer.SetPositions(capturedLine.ToArray());
            }
        }

        public void StartTrail()
        {
			currentRenderer.enabled = true;

			currentRenderer.SetColors(Color.yellow, Color.yellow);
            displayLine.Clear();
            copyLine.Clear();
            listening = true;
        }

        public void CapturePoint(Vector3 rightHandPoint)
        {
            displayLine.Add(rightHandPoint);
            copyLine.Add(rightHandPoint);

            currentRenderer.SetVertexCount(displayLine.Count);
            currentRenderer.SetPositions(displayLine.ToArray());

            if (displayLine.Count >= 10)
            {
                displayLine.RemoveAt(0);
            }            
        }

        public void CapturePoint(Vector3 myVector, List<Vector3> capturedLine, int maxLineLength)
        {
            if (capturedLine.Count >= maxLineLength)
            {
                capturedLine.RemoveAt(0);
            }
            capturedLine.Add(myVector);
        }

        public void StopTrail()
        {
			currentRenderer.enabled = false;

            c1.a = 1;
            c2.a = 1;
		
			endRenderer.SetColors(c1, c2);
			endRenderer.SetVertexCount(copyLine.Count);
			endRenderer.SetPositions(copyLine.ToArray());

            fading = true;

            listening = false;
        }

        public void ClearTrail()
        {
			currentRenderer.enabled = false;
            currentRenderer.SetVertexCount(0);
        }

        public bool UseCheck()
        {
            return currentlyInUse;
        }

        public void AssignHand(CaptureHand captureHand)
        {
            currentlyInUse = true;
            registeredHand = captureHand;
            SubscribeToEvents();

        }

        private void Update()
        {
            if(fading)
            {
                if(c1.a >= 0 || c2.a >= 0)
                {
                    c1.a -= Time.deltaTime;
                    c2.a -= Time.deltaTime;

					endRenderer.SetColors(c1, c2);
                }
                else
                {
                    fading = !fading;
                }
            }
        }

    }
}
