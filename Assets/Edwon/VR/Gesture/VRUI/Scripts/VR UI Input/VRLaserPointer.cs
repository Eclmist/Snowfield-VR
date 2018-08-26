using UnityEngine;
using UnityEngine.EventSystems;
using Edwon.VR.Gesture;
using Edwon.VR.Input;

namespace Edwon.VR
{
    public class VRLaserPointer : ILaserPointer
    {
        public int playerID = 0;

        public Handedness handType;
        InputOptions.Button selectButton;

        VRGestureRig rig;
        public IInput input;

        protected override void Initialize()
        {
            base.Initialize();
        }

        // called by the VRGestureRig when created
        public void InitRig(VRGestureRig _rig, Handedness _handType)
        {
            rig = _rig;
            handType = _handType;
            input = rig.GetInput(handType);
            selectButton = rig.menuButton;
        }

        new void Update ()
        {
            base.Update();
        }

        public override bool ButtonDown()
        {
            if(input != null)
            {
                bool buttonDown = input.GetButtonDown(selectButton);
                return buttonDown;
            }

            return false;
        }

        public override bool ButtonUp()
        {
            if (input != null)
            {
                bool buttonUp = input.GetButtonUp(selectButton);
                return buttonUp;
            }

            return false;
        }
        
        // BUZZ HAPTICS ON ENTER
        public override void OnEnterControl(GameObject control)
        {
            base.OnEnterControl(control);
            //var device = SteamVR_Controller.Input(_index);
            //device.TriggerHapticPulse(1000);
        }

        public override void OnUpdateControl(GameObject control, PointerEventData pointerData)
        {
            base.OnUpdateControl(control, pointerData);
        }

        // BUZZ HAPTICS ON EXIT
        public override void OnExitControl(GameObject control)
        {
            base.OnExitControl(control);
            //var device = SteamVR_Controller.Input(_index);
            //device.TriggerHapticPulse(600);
        }
    }

}