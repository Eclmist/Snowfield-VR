using System.Collections;

using System.Collections.Generic;

using UnityEngine;


[RequireComponent(typeof(Rigidbody))]
[RequireComponent(typeof(AudioSource))]
public abstract class GenericItem : MonoBehaviour, IInteractable, IDamage
{

    protected Rigidbody rigidBody;
    protected Collider itemCollider;
    #region GenericItem
    [SerializeField]
    protected int damage;
    [SerializeField]
    protected string m_name;
    protected AudioSource audioSource;
    [SerializeField]
    protected float range;
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

    #region
    [SerializeField] protected float maxForceVolume;
    #endregion

    public float Range
    {
        get
        {
            return range;
        }
    }
    protected virtual void Awake()
    {
        rigidBody = GetComponent<Rigidbody>();
        itemCollider = GetComponent<Collider>();
        audioSource = GetComponent<AudioSource>();
    }

    public int Damage
    {
        get
        {
            if (LinkedController != null)
                return linkedController.Velocity().magnitude < 5 ? (int)(linkedController.Velocity().magnitude * damage) : damage * 5;
            else if (isFlying)
                return rigidBody.velocity.magnitude < 5 ? (int)(rigidBody.velocity.magnitude * damage) : damage * 5;
            else
                return damage;
        }

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
        else if (referenceCheck.Device.GetTouchUp(SteamVR_Controller.ButtonMask.Trigger))
        {
            StopInteraction(referenceCheck);
        }
    }

    public virtual void StopInteraction(VR_Controller_Custom referenceCheck)
    {
        if (linkedController == referenceCheck)
        {
            linkedController = null;
            referenceCheck.SetInteraction(null);

            rigidBody.velocity = referenceCheck.Device.velocity;
            rigidBody.angularVelocity = referenceCheck.Device.angularVelocity;

            StartCoroutine(Throw(Player.Instance));
        }
    }

    public virtual void StartInteraction(VR_Controller_Custom referenceCheck)
    {
        if (linkedController != null && linkedController != referenceCheck)
            linkedController.SetInteraction(null);

        linkedController = referenceCheck;
    }

    [SerializeField]
    private bool isFlying;

    private IDamagable target;

    public virtual IEnumerator Throw(Actor thrower)
    {
        isFlying = true;
        while (rigidBody.velocity.magnitude > 0.1)
        {
            if (target != null)
            {
                
                thrower.Attack(this, target);
                target = null;
                break;
            }
            yield return new WaitForEndOfFrame();
        }
        target = null;
        isFlying = false;
    }



    protected virtual void OnCollisionEnter(Collision col)
    {
        if (isFlying)
        {
            target = col.transform.GetComponent<IDamagable>();
        }
        if (linkedController != null)
            PlaySound(linkedController.Velocity().magnitude > maxForceVolume ? 1 : linkedController.Velocity().magnitude / maxForceVolume);
    }

    protected void PlaySound(float volume)
    {
        if (audioSource != null)
        {
            audioSource.volume = volume;
            audioSource.Play();
        }
    }
    //public abstract void UpdatePosition();
}
