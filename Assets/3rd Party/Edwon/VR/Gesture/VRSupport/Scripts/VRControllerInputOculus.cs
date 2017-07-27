#if EDWON_VR_OCULUS

using UnityEngine;
using System.Collections;
using Edwon.VR.Gesture;

namespace Edwon.VR.Input
{
    public class VRControllerInputOculus : VRController
    {

        // options for broken oculus touch that doesn't quite reach 1
        OVRInput.Controller controllerType;

        public VRControllerInputOculus Init(Handedness handy)
        {
            handedness = handy;
            controllerType = (handy == Handedness.Left) ? OVRInput.Controller.LTouch : OVRInput.Controller.RTouch;
            return this;
        }

        // GET OCULUS VR CONTROLLER
        // returns controllerType mask for left or right oculus controller
        // @deprecated
        OVRInput.Controller GetOVRController()
        {
            // setup controller type variable (left or right)
            OVRInput.Controller controllerType = OVRInput.Controller.None;
            controllerType = (handedness == Handedness.Left) ? OVRInput.Controller.LTouch : OVRInput.Controller.RTouch;
            return controllerType;
        }

        // PLATFORM SPECIFIC INPUT MAPPING
        void LateUpdate()
        {
            // store the controller input into the class variables
            button1 = OVRInput.Get(OVRInput.Button.One, controllerType);
            button1Down = OVRInput.GetDown(OVRInput.Button.One, controllerType);
            button2 = OVRInput.Get(OVRInput.Button.Two, controllerType);
            button2Down = OVRInput.GetDown(OVRInput.Button.Two, controllerType);

            directional1 = OVRInput.Get(OVRInput.Axis2D.PrimaryThumbstick, controllerType);
            directional1Button = OVRInput.Get(OVRInput.Button.PrimaryThumbstick, controllerType);
            directional1ButtonDown = OVRInput.GetDown(OVRInput.Button.PrimaryThumbstick, controllerType);

            // triggers
            float trigger1Raw = OVRInput.Get(OVRInput.Axis1D.PrimaryIndexTrigger, controllerType); // for broken controller
            trigger1Button = OVRInput.Get(OVRInput.Button.PrimaryIndexTrigger, controllerType);
            trigger1ButtonDown = OVRInput.GetDown(OVRInput.Button.PrimaryIndexTrigger, controllerType);
            trigger1ButtonUp = OVRInput.GetUp(OVRInput.Button.PrimaryIndexTrigger, controllerType);
            float trigger2Raw = OVRInput.Get(OVRInput.Axis1D.PrimaryHandTrigger, controllerType); // for broken controller
            trigger2Button = OVRInput.Get(OVRInput.Button.PrimaryHandTrigger, controllerType);
            trigger2ButtonDown = OVRInput.GetDown(OVRInput.Button.PrimaryHandTrigger, controllerType);
            trigger2ButtonUp = OVRInput.GetUp(OVRInput.Button.PrimaryHandTrigger, controllerType);

            // triggers near touch
            trigger1Touch = OVRInput.Get(OVRInput.Touch.PrimaryIndexTrigger, controllerType);
            trigger1TouchDown = OVRInput.GetDown(OVRInput.Touch.PrimaryIndexTrigger, controllerType);
            trigger2Touch = OVRInput.Get(OVRInput.Touch.SecondaryIndexTrigger, controllerType);
            trigger2TouchDown = OVRInput.GetDown(OVRInput.Touch.SecondaryIndexTrigger, controllerType);
        }
    }
}
#endif
