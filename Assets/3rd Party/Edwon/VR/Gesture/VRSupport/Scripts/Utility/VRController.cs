using UnityEngine;
using System.Collections;
using Edwon.VR.Gesture;

// this class checks the global static VRTYPE variable
// and outputs controller input accordingly
// (oculus touch controller, playstation vr controller, etc...)
// it will search for a hand and if there is one
// it will send controller input values to its interface
namespace Edwon.VR.Input
{
    public class VRController : MonoBehaviour, IInput
    {
        public Handedness handedness;

        protected bool button1;
        protected bool button1Down;
        protected bool button2;
        protected bool button2Down;

        protected Vector2 directional1;
        protected bool directional1Button;
        protected bool directional1ButtonDown;
        protected float trigger1;
        protected bool trigger1Button;
        protected bool trigger1ButtonDown;
        protected bool trigger1ButtonUp;
        protected bool trigger1Touch;
        protected bool trigger1TouchDown;
        protected float trigger2;
        protected bool trigger2Button;
        protected bool trigger2ButtonDown;
        protected bool trigger2ButtonUp;
        protected bool trigger2Touch;
        protected bool trigger2TouchDown;

        //This is shared between both Steam and Oculus, this could be put into VRController.
        //Also this should be a dictionary instead of a monster if statement.
        public bool GetButton(InputOptions.Button button)
        {
            if (button == InputOptions.Button.Button1)
                return button1;
            if (button == InputOptions.Button.Button2)
                return button2;
            if (button == InputOptions.Button.Thumbstick)
                return directional1Button;
            if (button == InputOptions.Button.Trigger1)
                return trigger1Button;
            if (button == InputOptions.Button.Trigger2)
                return trigger2Button;
            return false;
        }

        //Shared functionality VRController.
        public bool GetButtonDown(InputOptions.Button button)
        {
            if (button == InputOptions.Button.Button1)
                return button1Down;
            if (button == InputOptions.Button.Button2)
                return button2Down;
            if (button == InputOptions.Button.Thumbstick)
                return directional1ButtonDown;
            if (button == InputOptions.Button.Trigger1)
                return trigger1ButtonDown;
            if (button == InputOptions.Button.Trigger2)
                return trigger2ButtonDown;
            return false;
        }

        public bool GetButtonUp(InputOptions.Button button)
        {
            if (button == InputOptions.Button.Trigger1)
                return trigger1ButtonUp;
            if (button == InputOptions.Button.Trigger2)
                return trigger2ButtonUp;
            return false;
        }

        public bool GetTouch(InputOptions.Touch touch)
        {
            if (touch == InputOptions.Touch.Trigger1)
                return trigger1Touch;
            if (touch == InputOptions.Touch.Trigger2)
                return trigger2Touch;
            return false;
        }

        public bool GetTouchDown(InputOptions.Touch touchDown)
        {
            if (touchDown == InputOptions.Touch.Trigger1)
                return trigger1TouchDown;
            if (touchDown == InputOptions.Touch.Trigger2)
                return trigger2TouchDown;
            return false;
        }

        public float GetAxis1D(InputOptions.Axis1D axis1D)
        {
            if (axis1D == InputOptions.Axis1D.Trigger1)
                return trigger1;
            if (axis1D == InputOptions.Axis1D.Trigger2)
                return trigger2;
            return 0;
        }

        public Vector2 GetAxis2D(InputOptions.Axis2D axis2D)
        {
            if (axis2D == InputOptions.Axis2D.Thumbstick)
                return directional1;
            return Vector2.zero;
        }

        public virtual void InputUpdate()
        {
            Debug.Log("GENERIC LATE UPDATE");
        }

        private void Start()
        {

        }

        //This should be seperated out into Child Classes
        void Update()
        {

        }

        void LateUpdate()
        {

        }



        //Shared, shared, shared.
        float ButtonDownTimer(float time)
        {
            float buttonValue = 0;

            Debug.Log(buttonValue);
            return buttonValue;
        }

        float ButtonUpTimer(float time)
        {
            float buttonValue = 0;

            return buttonValue;
        }


        //@deprecated
        protected IEnumerator LerpTimer(float time, float from, float to, System.Action<float> callback)
        {
            //Debug.Log("coroutine started");
            float elapsedTime = 0;
            while (elapsedTime <= time)
            {
                float valueToReturn = Mathf.Lerp(from, to, (elapsedTime / time));
                callback(valueToReturn);
                //Debug.Log("value: " + valueToReturn) ;
                elapsedTime += Time.deltaTime;
                yield return null;
            }
        }

    }
}
