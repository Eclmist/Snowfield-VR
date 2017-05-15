using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Settings_Button : MonoBehaviour, IInteractable {

    Vector3 posMid;
    protected VR_Controller_Custom linkedController = null;
    private int percentage = 0;

    void Awake()
    {
        posMid = transform.localPosition;
        
    }

    public VR_Controller_Custom LinkedController
    {
        get
        {
            return linkedController;
        }
        set
        {
            linkedController = value;
        }
    }

    public virtual void Interact(VR_Controller_Custom referenceCheck)
    {

        if (referenceCheck.Device.GetTouchDown(SteamVR_Controller.ButtonMask.Trigger))
        {
            Debug.Log("Triggered");
            Collider coll = GetComponent<Collider>();
            StartInteraction(referenceCheck);
        }
        else if (referenceCheck.Device.GetTouchUp(SteamVR_Controller.ButtonMask.Trigger))
        {
            StopInteraction(referenceCheck);
        }


    }

    public virtual void StartInteraction(VR_Controller_Custom referenceCheck)
    {
        if (linkedController != null && linkedController != referenceCheck)
            linkedController.SetInteraction(null);

        linkedController = referenceCheck;
    }

    public void StopInteraction(VR_Controller_Custom referenceCheck)
    {
        if (linkedController == referenceCheck)
        {
            linkedController = null;
            referenceCheck.SetInteraction(null);
        }
    }
    protected virtual void OnTriggerExit(Collider col)
    {
        Debug.Log("I am fucking exiting.");
        VR_Controller_Custom controller = col.GetComponent<VR_Controller_Custom>();
        if (controller != null)
            StopInteraction(controller);
    }

    // Use this for initialization
    void Start () {
		
	}
	
	// Update is called once per frame
	void Update ()
    {
        posMid.x = Mathf.Clamp(transform.localPosition.x, -0.13f, 0.28f);
        transform.localPosition = new Vector3(posMid.x, transform.localPosition.y, transform.localPosition.z);
        percentage = (int)(((transform.localPosition.x - (-0.13f)) / 0.41f )* 100);

        if (percentage < 0)
            percentage = 0;
        else if (percentage > 100)
            percentage = 100;

        Settings.Instance.SetFill(percentage);
	}
}
