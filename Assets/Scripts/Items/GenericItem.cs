using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class GenericItem : MonoBehaviour,IInteractable {

    [SerializeField]    protected string name;
    [SerializeField]    protected float weight;
    [SerializeField]    protected AudioClip sound;
    [SerializeField]    protected AudioSource audioSource;
    [SerializeField]    protected bool hasPivot;
    [SerializeField]    protected Vector3 positionalOffset, rotationalOffset;

    private Rigidbody rigidBody;

    protected void Awake()
    {
        rigidBody = GetComponent<Rigidbody>();
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
        get { return this.name; }
        set { this.name = value; }
    }

    public float Weight
    {
        get { return this.weight; }
        set { this.weight = value; }
    }

    public virtual void Interact()
    {
        rigidBody.useGravity = false;
    }

    public virtual void StopInteraction()
    {
        rigidBody.useGravity = true;
    }



	
}
