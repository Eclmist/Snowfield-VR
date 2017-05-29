using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InGameUI_MenuArea : MonoBehaviour, IInteractable
{
    private VR_Controller_Custom linkedController = null;
    private Vector3 pos, posStart;

    private bool menuCoroutineStarted = false;
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
        if (gameObject.name == "MainMenuArea")
        {
            if (referenceCheck.Device.GetTouchDown(SteamVR_Controller.ButtonMask.ApplicationMenu))
            {
                Debug.Log("Interacting");

                if (!menuCoroutineStarted)
                    StartCoroutine(MenuCoroutine(referenceCheck));

            }
            if (referenceCheck.Device.GetTouchUp(SteamVR_Controller.ButtonMask.ApplicationMenu))
            {

            }

        }
        else if (referenceCheck.Device.GetTouchDown(SteamVR_Controller.ButtonMask.Trigger))
        {

        }
    }

    IEnumerator MenuCoroutine(VR_Controller_Custom referenceCheck)
    {
        posStart = referenceCheck.Device.transform.pos;
        menuCoroutineStarted = true;

        yield return new WaitForSeconds(0.7f);

        pos = referenceCheck.Device.transform.pos;
        ///
        ///
        ///
        if ((float)(pos.x - posStart.x) / (pos.y - posStart.y) < 0.2f && pos.y - posStart.y >= 0.7)
        {
            Debug.Log("Main Menu appear");
        }
        menuCoroutineStarted = false;
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
        VR_Controller_Custom controller = col.GetComponent<VR_Controller_Custom>();
        if (controller != null)
            StopInteraction(controller);
    }

    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        Debug.Log(menuCoroutineStarted);
        Debug.Log((float)(pos.x - posStart.x));
    }


    private void ActivateMainMenuArea(VR_Controller_Custom referenceCheck)
    {
        InGameUI.Instance.ActivateMM();
    }
}


