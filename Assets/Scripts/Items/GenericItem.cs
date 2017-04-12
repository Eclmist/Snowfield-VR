using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class GenericItem : MonoBehaviour,IInteractable {

    [SerializeField]    protected string m_name;
    [SerializeField]    protected float weight;
    [SerializeField]    protected AudioClip sound;
    [SerializeField]    protected AudioSource audioSource;
    [SerializeField]    protected bool hasPivot;
    [SerializeField]    protected Vector3 positionalOffset, rotationalOffset;

    private Transform linkedTransform = null;

    private Rigidbody rigidBody;

    protected void Awake()
    {
        rigidBody = GetComponent<Rigidbody>();
    }

    public Transform LinkedTransform
    {
        get
        {
            return linkedTransform;
        }
    }
       
    public bool HasPivot
    {
        get
        {
            return hasPivot;
        }
    }

    public Vector3 PositionalOffset
    {
        get
        {
            return positionalOffset;
        }
    }

    public Vector3 RotationalOffset
    {
        get
        {
            return rotationalOffset;
        }
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

    public virtual void Interact(Transform referenceCheck)
    {
        linkedTransform = referenceCheck;
        rigidBody.useGravity = false;
        if (hasPivot)
        {
            transform.localPosition = referenceCheck.position + positionalOffset;
            transform.rotation = Quaternion.Euler(referenceCheck.rotation.eulerAngles + rotationalOffset);
        }
    }

    public virtual void StopInteraction(Transform referenceCheck)
    {
        if (linkedTransform == referenceCheck)
        {
            rigidBody.useGravity = true;
        }
    }



	
}
