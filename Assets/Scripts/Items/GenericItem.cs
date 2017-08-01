using System.Collections;





using UnityEngine;





[RequireComponent(typeof(AudioSource))]


public class GenericItem : VR_Interactable_Object, IDamage


{

	protected bool isColliding = false;

	[SerializeField] protected float directionalMultiplier = 5f, maxLerpForce = 10f;
	protected float collisionVibrationMagnitude = 0.002F;

    protected JobType jobType;


    public JobType JobType
	{
		get { return this.jobType; }
    }
		
    protected Collider itemCollider;

	//protected Vector3 positionOffset;
	//protected Quaternion rotationOffset;

    #region itemData

    [SerializeField] private int itemID = -1;

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
		base.OnTriggerRelease(referenceCheck);

		rigidBody.velocity = Vector3.zero;
		rigidBody.angularVelocity = Vector3.zero;
		rigidBody.AddForceAtPosition(currentReleaseVelocity, targetPositionPoint.transform.position, ForceMode.Impulse);

        gameObject.layer = LayerMask.NameToLayer("Interactable");
		rigidBody.useGravity = true;
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



	public override void OnFixedUpdateInteraction(VR_Controller_Custom referenceCheck)
	{
		base.OnFixedUpdateInteraction();

		Vector3 PositionDelta = targetPositionPoint.transform.position - transform.position;

		if (!isColliding)
		{
			rigidBody.MovePosition(targetPositionPoint.transform.position);
			rigidBody.MoveRotation(targetPositionPoint.transform.rotation);
		}
		else
		{
			referenceCheck.Vibrate(collisionVibrationMagnitude);

			float currentForce = maxLerpForce;

			rigidBody.velocity =
				PositionDelta.magnitude * directionalMultiplier > currentForce ?
				(PositionDelta).normalized * currentForce : PositionDelta * directionalMultiplier;


			// Rotation ----------------------------------------------

			//Quaternion targetRotation = referenceCheck.transform.rotation * rotationOffset;

			Quaternion RotationDelta =  targetPositionPoint.transform.rotation * Quaternion.Inverse(this.transform.rotation);
			float angle;
			Vector3 axis;
			RotationDelta.ToAngleAxis(out angle, out axis);

			if (angle > 180)
				angle -= 360;

			float angularVelocityNumber = .2f;

			// -------------------------------------------------------
			rigidBody.angularVelocity = axis * angle * angularVelocityNumber;
			  	
		}
	}





    public override void OnTriggerPress(VR_Controller_Custom controller)
    {

		controller.SetModelActive(false);
		rigidBody.useGravity = false;
		rigidBody.isKinematic = false;
        gameObject.layer = LayerMask.NameToLayer("Player");
		base.OnTriggerPress(controller);

    }





    protected virtual void OnCollisionEnter(Collision col)
    {
        if (isFlying)
        {
            target = col.transform.GetComponent<Monster>();
        }

		isColliding = true;

		if (currentInteractingController != null)
		{
			// float value = currentInteractingController.Velocity.magnitude <= collisionVibrationMagnitude ? currentInteractingController.Velocity.magnitude : collisionVibrationMagnitude;
			currentInteractingController.Vibrate(triggerEnterVibration);
		}
    }

	protected virtual void OnCollisionStay(Collision collision)
	{
	}

	protected virtual void OnCollisionExit(Collision collision)
	{
		isColliding = false;
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