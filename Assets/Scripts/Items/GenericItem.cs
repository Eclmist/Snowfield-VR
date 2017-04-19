using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class GenericItem : MonoBehaviour, IInteractable
{

    [SerializeField]
    protected string m_name;
    [SerializeField]
    protected float weight;
    [SerializeField]
    protected AudioClip sound;
    [SerializeField]
    protected AudioSource audioSource;
    [SerializeField] [Range(1, 6)] private float heptic; //remoove

    private Vector3 colliderBound;
    private VR_Controller_Custom linkedController = null;

    private Rigidbody rigidBody;

    [SerializeField]
    [Range(100, 1000)]
    private float lerpValue;
    protected void Awake()
    {
        rigidBody = GetComponent<Rigidbody>();
    }

    public VR_Controller_Custom LinkedController
    {
        get
        {
            return linkedController;
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

    protected void Start()
    {
        colliderBound = GetComponent<Collider>().bounds.size;
       
    }
    public virtual void Interact(VR_Controller_Custom referenceCheck)
    {
        linkedController = referenceCheck;
        rigidBody.useGravity = false;
        transform.localPosition = referenceCheck.transform.position;
        transform.rotation = referenceCheck.transform.rotation;
        rigidBody.maxAngularVelocity = 100f;
    }

    public virtual void StopInteraction(VR_Controller_Custom referenceCheck)
    {
        if (linkedController == referenceCheck)
        {
            rigidBody.useGravity = true;
            linkedController = null;
            rigidBody.velocity = referenceCheck.Velocity();
            rigidBody.angularVelocity = referenceCheck.AngularVelocity();

        }
    }


    public void UpdatePosition()
    {

        Vector3 PositionDelta = (linkedController.transform.position - transform.position);

        Quaternion RotationDelta = linkedController.transform.rotation * Quaternion.Inverse(this.transform.rotation);
        float angle;
        Vector3 axis;
        RotationDelta.ToAngleAxis(out angle, out axis);

        if (angle > 180)
            angle -= 360;

        rigidBody.angularVelocity = axis * angle * Time.fixedDeltaTime * 40;

        PositionDelta = PositionDelta.magnitude > .1f ? PositionDelta.normalized * .1f : PositionDelta;
        rigidBody.velocity = PositionDelta * 10000 * rigidBody.mass * Time.fixedDeltaTime;
    }

    protected void OnCollisionEnter(Collision collision)
    {

        if (linkedController != null)
        {
            float value = linkedController.Velocity().magnitude <= heptic ? linkedController.Velocity().magnitude : heptic;
            linkedController.Vibrate(value/heptic * 10);

        }

    }

    protected void OnCollisionStay(Collision collision)
    {
        if (linkedController != null)
        {
            float value = Vector3.Distance(transform.rotation.eulerAngles, linkedController.transform.rotation.eulerAngles);

            value = value <= 720 ? value : 720;

            linkedController.Vibrate(value / 720 * 5);
        }
    }

}
