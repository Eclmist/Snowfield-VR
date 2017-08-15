using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Edwon.VR.Gesture;
using Edwon.VR;

public class SpellController : MonoBehaviour
{
    protected VR_Controller_Custom controllerRef;

    protected CaptureHand handRef;

    protected bool isChecking = false, isActivated = false;

    protected float forceToDisable = .2f;

    protected float timeToActivateInSeconds = 1f;
    [SerializeField]
    protected SpellHandler spellHandlerPrefab;

    protected SpellHandler currentHandler;
    protected float timer = 0f;
    protected void Start()
    {
        //if (rig == null)
        //{
        //	Debug.Log("there is no VRGestureRig in the scene, please add one");
        //}

        controllerRef = GetComponent<VR_Controller_Custom>();
        if (!controllerRef)
        {
            Debug.Log("No Controller");
            Destroy(this);
        }
    }

    protected void Update()
    {
        if (handRef == null)
        {
            if (controllerRef.Handle == VR_Controller_Custom.Controller_Handle.LEFT)
                handRef = VRGestureRig.leftCapture;
            else if (controllerRef.Handle == VR_Controller_Custom.Controller_Handle.RIGHT)
                handRef = VRGestureRig.rightCapture;
        }
        else if (controllerRef.Device != null)
        {
            if (controllerRef.CurrentItemInHand != null && handRef.HandState == CaptureHand.State.ACTIVATED)
                return;

            CheckForStateChange();

            if (isActivated)
            {
                if (controllerRef.Device.GetPressDown(SteamVR_Controller.ButtonMask.Trigger) && handRef.HandState == CaptureHand.State.DEACTIVATED)
                    handRef.HandState = CaptureHand.State.ACTIVATED;
                else if (controllerRef.Device.GetPressUp(SteamVR_Controller.ButtonMask.Trigger) && handRef.HandState == CaptureHand.State.ACTIVATED)
                    handRef.HandState = CaptureHand.State.DEACTIVATED;
            }
            else if (handRef.HandState == CaptureHand.State.ACTIVATED)
            {
                handRef.HandState = CaptureHand.State.DEACTIVATED;
            }
        }

    }

    protected void CheckForStateChange()
    {

        if (controllerRef.Device.GetPressDown(SteamVR_Controller.ButtonMask.Trigger))
            isChecking = true;
        else if (controllerRef.Device.GetPressUp(SteamVR_Controller.ButtonMask.Trigger))
            isChecking = false;

        if (isChecking && controllerRef.Velocity.magnitude > forceToDisable)
        {
            isChecking = false;
        }

        if (isChecking)
            timer += Time.deltaTime;
        else
            timer = 0;

        if (timer > timeToActivateInSeconds)
        {
            SwitchState();
        }

    }

    protected void SwitchState()
    {
        isActivated = !isActivated;
        if (isActivated)
            currentHandler = Instantiate(spellHandlerPrefab, transform.position, transform.rotation);
        else
            Destroy(currentHandler.gameObject);
        controllerRef.Vibrate(1);
        timer = 0;
        isChecking = false;
    }

    void OnEnable()
    {
        GestureRecognizer.GestureDetectedEvent += OnGestureDetected;
    }

    void OnDisable()
    {
        GestureRecognizer.GestureDetectedEvent -= OnGestureDetected;
    }

    void OnGestureDetected(string gestureName, double confidence, Handedness hand, bool isDouble = false)
    {

        if ((hand == Handedness.Left && controllerRef.Handle == VR_Controller_Custom.Controller_Handle.LEFT) || (hand == Handedness.Right && controllerRef.Handle == VR_Controller_Custom.Controller_Handle.RIGHT))
        {
            Spell spell = SpellManager.Instance.GetSpell(gestureName);
            if (spell)
            {
                controllerRef.Vibrate(10);

                currentHandler.CastSpell(spell, controllerRef, Player.Instance);
            }
        }
    }

}
