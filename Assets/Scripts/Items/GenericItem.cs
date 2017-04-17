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
    private CharacterJoint joint;
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
        joint = gameObject.AddComponent<CharacterJoint>();
        joint.connectedBody = attachedPoint;
        SoftJointLimitSpring spring = joint.swingLimitSpring;
        spring.spring = 3.0f;
        joint.swingLimitSpring = spring;
        spring = joint.twistLimitSpring;
        spring.spring = 3.0f;
        joint.twistLimitSpring = spring;
        ChangeLimit(false);
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

    protected virtual void OnCollisionEnter(Collision col)
    {
        if (joint != null)
        {
            ChangeLimit(true);
        }
        float currentVelocity = GetVelocity();
    }

    public float GetVelocity()
    {
        if(joint == null)
        {
            return rigidBody.velocity.magnitude;
        }
        else
        {
            return joint.GetComponent<Rigidbody>().velocity.magnitude;
        }
    }

    protected virtual void OnCollisionExit(Collision col)
    {
        if (joint != null)
        {
            ChangeLimit(false);
        }
    }

    private void ChangeLimit(bool colliding)
    {
        if (colliding)
        {
            setLimitValues(maxForceRotation);
        }
        else
        {
            StopAllCoroutines();
            StartCoroutine(LimitCoroutine(maxForceRotation));
        }
            
    }

    private IEnumerator LimitCoroutine(float rotationLimit)
    {

        do
        {
            rotationLimit = Mathf.Lerp(rotationLimit, 0, Time.deltaTime*10);
            setLimitValues(rotationLimit);
            if (rotationLimit <= 1)
            {
                setLimitValues(0);
                break;
            }


            yield return new WaitForEndOfFrame();
        }
        while (rotationLimit > 1);
    }

    private void setLimitValues(float value)
    {
        SoftJointLimit limit = joint.lowTwistLimit;
        limit.limit = value;
        joint.lowTwistLimit = limit;

        limit = joint.highTwistLimit;
        limit.limit = value;
        joint.highTwistLimit = limit;

        limit = joint.swing1Limit;
        limit.limit = value;
        joint.swing1Limit = limit;

        limit = joint.swing2Limit;
        limit.limit = value;
        joint.swing2Limit = limit;
    }


}
