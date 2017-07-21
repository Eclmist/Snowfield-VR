using System.Collections;

using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public abstract class GenericItem : VR_Interactable_Object, IDamage

{
    protected JobType jobType;

    public JobType JobType

    {
        get { return this.jobType; }
    }

    protected Collider itemCollider;

    #region itemData

    [SerializeField]private int itemID;

    public int ItemID
    {
        get { return this.itemID; }
        set { this.itemID = value; }
    }


    #endregion itemData


    #region GenericItem

    [SerializeField]
    protected int damage;

    [SerializeField]
    protected string m_name;

    protected AudioSource audioSource;

    #endregion GenericItem

    #region IInteractable

    private bool isFlying;

    private IDamagable target;

    [SerializeField]
    private int maxVelocityDamageMultiplier = 5;

    #endregion IInteractable

    #region

    [SerializeField]
    protected float maxSwingForce;

    #endregion

    protected override void Awake()

    {
        itemCollider = GetComponentInChildren<Collider>();

        audioSource = GetComponent<AudioSource>();

        base.Awake();
    }

    public int Damage
    {
        get
        {
            if (currentInteractingController != null)

                return currentInteractingController.Velocity.magnitude < maxVelocityDamageMultiplier ? (int)(currentInteractingController.Velocity.magnitude * damage) : damage * maxVelocityDamageMultiplier;
            else if (isFlying)

                return rigidBody.velocity.magnitude < maxVelocityDamageMultiplier ? (int)(rigidBody.velocity.magnitude * damage) : damage * maxVelocityDamageMultiplier;
            else

                return damage;
        }
    }

    public string Name

    {
        get { return this.m_name; }

        set { this.m_name = value; }
    }

    public override void OnTriggerRelease(VR_Controller_Custom referenceCheck)

    {
        referenceCheck.Model.SetActive(true);

        base.OnTriggerRelease(referenceCheck);

        StartCoroutine(Throw(Player.Instance));
    }

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


    public override void OnTriggerPress(VR_Controller_Custom controller)
    {
        controller.Model.SetActive(false);
        base.OnTriggerPress(controller);
    }

    protected virtual void OnCollisionEnter(Collision col)

    {
        if (isFlying)

        {
            target = col.transform.GetComponent<IDamagable>();
        }

        //if (col.gameObject.GetComponent<Rigidbody>() == null)

        //    PlaySound(rigidBody.velocity.magnitude > maxForceVolume ? 1 : rigidBody.velocity.magnitude / maxForceVolume);

        //Sound Needs fixing
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