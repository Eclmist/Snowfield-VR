using System.Collections;

using System.Collections.Generic;

using UnityEngine;


[RequireComponent(typeof(Rigidbody))]
[RequireComponent(typeof(AudioSource))]
public abstract class InteractableItem : MonoBehaviour, IInteractable
{

    protected Rigidbody rigidBody;
    protected Collider itemCollider;
    #region GenericItem
    [SerializeField]
    protected string m_name;
    protected AudioSource audioSource;
    #endregion

    #region IInteractable
    protected VR_Controller_Custom linkedController = null;
    public VR_Controller_Custom LinkedController
    {
        get
        {
            return linkedController;
        }
    }


    #endregion

    protected virtual void Awake()
    {
        rigidBody = GetComponent<Rigidbody>();
        itemCollider = GetComponent<Collider>();
        audioSource = GetComponent<AudioSource>();
    }



    public string Name
    {
        get { return this.m_name; }
        set { this.m_name = value; }
    }

    //protected virtual void Update()
    //{
    //    if (linkedController != null)
    //        UpdatePosition();
    //}

    public virtual void Interact(VR_Controller_Custom referenceCheck)
    {
        if (referenceCheck.Device.GetTouchDown(SteamVR_Controller.ButtonMask.Trigger))
        {
            StartInteraction(referenceCheck);
        }
        else if (referenceCheck.Device.GetTouchUp(SteamVR_Controller.ButtonMask.Trigger) && linkedController == referenceCheck)
        {
            StopInteraction(referenceCheck);
        }
    }

    public virtual void StopInteraction(VR_Controller_Custom referenceCheck)
    {
        linkedController = null;
        referenceCheck.SetInteraction(null);
    }

    public virtual void StartInteraction(VR_Controller_Custom referenceCheck)
    {
        if (linkedController != null && linkedController != referenceCheck)
            linkedController.SetInteraction(null);

        linkedController = referenceCheck;
    }



    //public abstract void UpdatePosition();



}
