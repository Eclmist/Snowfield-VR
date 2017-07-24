using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(SteamVR_TrackedObject))]
public class VR_Controller_Custom : MonoBehaviour
{
    [SerializeField]
    private GameObject model;

    public GameObject Model
    {
        get
        {
            return model;
        }
    }

    public enum Controller_Handle
    {
        LEFT,
        RIGHT
    }

    [SerializeField]
    private Controller_Handle handle;
    private SteamVR_TrackedObject trackedObject;
    private SteamVR_Controller.Device device;

    private VR_Interactable interactable;

    private List<VR_Interactable_UI> listOfInteractingUI = new List<VR_Interactable_UI>();

    public VR_Interactable_UI UI
    {
        get
        {

            float closestDist = 10000f;
            VR_Interactable_UI returnUI = null;
            foreach (VR_Interactable_UI ui in listOfInteractingUI)
            {
                float dist = Vector3.Distance(transform.position, ui.transform.position);
                if (dist < closestDist)
                {
                    closestDist = dist;
                    returnUI = ui;
                }
            }

                return returnUI;
        }

    }

    public Controller_Handle Handle
    {
        get
        {
            return handle;
        }
    }

    void Awake()
    {
        trackedObject = GetComponent<SteamVR_TrackedObject>();
        model = transform.Find("Model").gameObject;
    }

    private void FixedUpdate()
    {
        device = SteamVR_Controller.Input((int)trackedObject.index);
        ControllerInput();

    }

    private void Update()
    {
        if (interactable != null)
        {
            if (device.GetPressDown(SteamVR_Controller.ButtonMask.Trigger))
                interactable.OnTriggerPress(this);
            if (device.GetPressUp(SteamVR_Controller.ButtonMask.Trigger))
                interactable.OnTriggerRelease(this);
            if (device.GetPressDown(SteamVR_Controller.ButtonMask.Grip))
                interactable.OnGripPress(this);
            if (device.GetPressUp(SteamVR_Controller.ButtonMask.Grip))
                interactable.OnGripRelease(this);
        }

        listOfInteractingUI.RemoveAll(VR_Interactable_UI => VR_Interactable_UI == null);
    }


    private void ControllerInput()
    {
        if (interactable != null)
        {
            if (device.GetPress(SteamVR_Controller.ButtonMask.Trigger))
                interactable.OnTriggerHold(this);


            if (device.GetPress(SteamVR_Controller.ButtonMask.Grip))
                interactable.OnGripHold(this);

            if (interactable.LinkedController == this)
                interactable.OnInteracting(this);

        }
    }

    private void OnTriggerEnter(Collider collider)
    {
        VR_Interactable currentObject = collider.GetComponentInParent<VR_Interactable>();

        if (currentObject && (interactable == null || interactable.LinkedController != this))
        {
            if (interactable != null)
                interactable.OnControllerExit(this);
            interactable = currentObject;
            interactable.OnControllerEnter(this);
        }

        VR_Interactable_UI interactableUI = collider.GetComponent<VR_Interactable_UI>();
        if (interactableUI)
            listOfInteractingUI.Add(interactableUI);
    }

    private void OnTriggerStay(Collider collider)
    {
        if (interactable != null)
            interactable.OnControllerStay(this);
    }

    private void OnTriggerExit(Collider other)
    {
        if (interactable != null && interactable.LinkedController != this)
        {
            interactable.OnControllerExit(this);
            interactable = null;
        }

        VR_Interactable_UI interactableUI = other.GetComponent<VR_Interactable_UI>();
        if (interactableUI)
            listOfInteractingUI.Remove(interactableUI);
    }

    public Vector3 Velocity
    {
        get { return device.velocity; }
    }

    public Vector3 AngularVelocity
    {
        get { return device.angularVelocity; }
    }

    public void Vibrate(float val)//pass in 1 - 10
    {
        if (val > 0 && val <= 10)
        {
            ushort passVal = (ushort)(val / 10 * 3999);
            if (passVal > 0)
                device.TriggerHapticPulse(passVal);
        }
    }

    public void SetInteraction(VR_Interactable _interacted)
    {
        interactable = _interacted;
    }

    public SteamVR_Controller.Device Device
    {
        get
        {
            return device;
        }
    }
}