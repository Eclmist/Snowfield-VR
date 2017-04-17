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
    [SerializeField]
    private float maxForceRotation;
    private HingeJoint joint;
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

    public virtual void Interact(Transform referenceCheck, Rigidbody attachedPoint)
    {
        linkedTransform = referenceCheck;
        rigidBody.useGravity = false;
        transform.localPosition = referenceCheck.position;
        transform.rotation = Quaternion.Euler(referenceCheck.rotation.eulerAngles);
        joint = gameObject.AddComponent<HingeJoint>();
        joint.connectedBody = attachedPoint;
    }

    public virtual void StopInteraction(Transform referenceCheck)
    {
        if (linkedTransform == referenceCheck)
        {
            rigidBody.useGravity = true;
            DestroyImmediate(GetComponent<CharacterJoint>());
            joint = null;
        }
    }

    //protected virtual void OnCollisionEnter(Collision col)
    //{
    //    if (joint != null)
    //    {
    //        ChangeLimit(true);
    //    }
    //    float currentVelocity = GetVelocity();//Do whateve u want
    //}

    public float GetVelocity()
    {
        if (joint == null)
        {
            return rigidBody.velocity.magnitude;
        }
        else
        {
            return joint.GetComponent<Rigidbody>().velocity.magnitude;
        }
    }

    //protected virtual void OnCollisionExit(Collision col)
    //{
    //    if (joint != null)
    //    {
    //        ChangeLimit(false);
    //    }

    //    Debug.Log("hoit");
    //}

    //private void ChangeLimit(bool colliding)
    //{
    //    if (colliding)
    //    {
    //        setLimitValues(maxForceRotation);
    //    }
    //    else
    //    {
    //        setLimitValues(0);
    //    }

    //}
    //private IEnumerator LimitCoroutine(float rotationLimit)
    //{

    //    do
    //    {
    //        rotationLimit = Mathf.Lerp(rotationLimit, 0, Time.deltaTime * 10);
    //        setLimitValues(rotationLimit);
    //        if (rotationLimit <= 1)
    //        {
    //            setLimitValues(0);
    //            break;
    //        }000000000000000000000000000000000000000000000000000000000000000000


    //        yield return new WaitForEndOfFrame();
    //    }
    //    while (rotationLimit > 1);
    //}




}
