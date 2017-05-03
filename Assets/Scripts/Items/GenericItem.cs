using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public abstract class InteractableItem : MonoBehaviour, IInteractable
{

    protected Rigidbody rigidBody;
    protected Collider itemCollider;
    #region GenericItem
    [SerializeField]
    protected string m_name;
    [SerializeField]
    protected AudioClip sound;
    [SerializeField]
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
    }



    public string Name
    {
        get { return this.m_name; }
        set { this.m_name = value; }
    }

    protected virtual void Update()
    {
        if (linkedController != null)
            UpdatePosition();
    }

    public AudioClip Sound
    {
        get { return this.sound; }
        set { this.sound = value; }
    }

    public AudioSource AudioSource
    {
        get { return this.audioSource; }
    }
    
    public virtual void Interact(VR_Controller_Custom referenceCheck)
    {
        if (linkedController != null && linkedController != referenceCheck)
            linkedController.SetInteraction(null);

        linkedController = referenceCheck;
    }

    public virtual void StopInteraction(VR_Controller_Custom referenceCheck)
    {
        if (linkedController == referenceCheck)
        {
            linkedController = null;
            referenceCheck.SetInteraction(null);
        }
    }


    public abstract void UpdatePosition();

    

}
