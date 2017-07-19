using UnityEngine;
using System.Collections;

namespace Edwon.VR.Gesture.Examples
{
    public class Example1Player : MonoBehaviour
    {

        public GameObject circle;
        public GameObject triangle;
        public GameObject push;
        public GameObject pull;
        public GameObject nullGO;

        void OnEnable()
        {
            GestureRecognizer.GestureDetectedEvent += OnGestureDetected;
            GestureRecognizer.GestureRejectedEvent += OnGestureRejected;
        }

        void OnDisable()
        {
            GestureRecognizer.GestureDetectedEvent -= OnGestureDetected;
            GestureRecognizer.GestureRejectedEvent -= OnGestureRejected;
        }

        void OnGestureDetected(string gestureName, double confidence, Handedness hand, bool isDouble)
        {
            //string confidenceString = confidence.ToString().Substring(0, 4);
            //Debug.Log("detected gesture: " + gestureName + " with confidence: " + confidenceString);

            switch (gestureName)
            {
                case "Circle":
                    StartCoroutine(AnimateShape(circle));
                    break;             
                case "Triangle":
                    StartCoroutine(AnimateShape(triangle));
                    break;
                case "Push":
                    StartCoroutine(AnimateShape(push));
                    break;
                case "Pull":
                    StartCoroutine(AnimateShape(pull));
                    break;
            }
        }

        void OnGestureRejected(string error, string gestureName = null, double confidenceValue = 0)
        {
            StartCoroutine(AnimateShape(nullGO));
        }

        IEnumerator AnimateShape(GameObject shape)
        {
            Renderer[] renderers = shape.GetComponentsInChildren<Renderer>();
            foreach (Renderer r in renderers)
            {
                r.material.color = Color.red;
            }

            yield return new WaitForSeconds(.6f);

            foreach (Renderer r in renderers)
            {
                r.material.color = Color.white;
            }
        }

    }
}