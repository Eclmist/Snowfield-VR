using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public abstract class GenericItem : MonoBehaviour, IInteractable
{

    protected Rigidbody rigidBody;
    protected Collider itemCollider;
    #region GenericItem
    [SerializeField]
    protected string m_name;
    [SerializeField]
    protected float weight;
    [SerializeField]
    protected AudioClip sound;
    [SerializeField]
    protected AudioSource audioSource;
    #endregion

    #region IInteractable
    [SerializeField] [Range(0.01f, 6)] private float heptic; //remoove
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

    public float Weight
    {
        get { return this.weight; }
        set { this.weight = value; }
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

    protected virtual void OnCollisionEnter(Collision collision)
    {

        if (linkedController != null)
        {
            float value = linkedController.Velocity().magnitude <= heptic ? linkedController.Velocity().magnitude : heptic;
            linkedController.Vibrate(value / heptic * 10);

        }

    }

    protected virtual void OnCollisionStay(Collision collision)
    {
        if (linkedController != null)
        {
            float value = Vector3.Distance(transform.rotation.eulerAngles, linkedController.transform.rotation.eulerAngles);

            value = value <= 720 ? value : 720;

            linkedController.Vibrate(value / 720 * 5);
        }
    }

}
