using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlacksmithItem : InteractableItem {

    public Material initialMaterial;
    public Material forgingMaterial; // Material to lerp to when heated
    protected MeshRenderer meshRenderer; // To modify the material
    [SerializeField]
    protected float currentTemperature; // Current temperature of the item
    protected const float baseRate = 0.0001f; // The base heat up rate before multiplying the conductivity
    [SerializeField]
    private bool heatSourceDetected;
    private float distFromHeat;
    private float quenchRate = 1;


    [SerializeField]
    [Range(1, 100)]
    protected float conductivity; // Acts as a multiplier for the heating rate

    protected virtual void Start()
    {
        meshRenderer = GetComponent<MeshRenderer>();
        meshRenderer.material = initialMaterial;
        currentTemperature = 0; // Assume 0 to be room temperature for ez calculations
    }

    protected override void Update()
    {

        base.Update();
        if (Input.GetKeyDown(KeyCode.E))
            quenchRate = 10;

        RespondToHeat();
        ControlledHeatingProcess();

        //Debug.Log(heatSourceDetected);
        //Debug.Log(currentTemperature);
    }

    //--------------- Properties -----------------//

    public float Conductivity
    {
        get { return this.conductivity; }
    }

    public float CurrentTemperature
    {
        get { return this.currentTemperature; }
        set { this.currentTemperature = value; }
    }

    public bool HeatSourceDetected
    {
        get { return this.HeatSourceDetected; }
        set { this.heatSourceDetected = value; }
    }

    public float QuenchRate
    {
        get { return this.quenchRate; }
        set { this.quenchRate = value; }
    }

    //--------------- Properties (END) -----------------//



    public void SetHeatingEnvironment(float distFromHeat)
    {
        heatSourceDetected = true;
        this.distFromHeat = distFromHeat;
    }

    // Called every frame and constantly heats up the item if a heat source is detected
    private void ControlledHeatingProcess()
    {
        if(heatSourceDetected && Mathf.Sign(distFromHeat) == 1 && (currentTemperature < 1))
        {
            currentTemperature += baseRate * conductivity * distFromHeat;
            if (currentTemperature > 1)
                currentTemperature = 1;

        }
        else if(!heatSourceDetected && currentTemperature > 0)
        {
            currentTemperature -= (baseRate * conductivity * quenchRate);
            if (currentTemperature < 0)
                currentTemperature = 0;
        }
       

    }

    // Changes color (Material) when heated
    private void RespondToHeat()
    {
       meshRenderer.material.Lerp(initialMaterial, forgingMaterial, currentTemperature);
    }

    public override void Interact(VR_Controller_Custom referenceCheck)
    {
        rigidBody.useGravity = false;
        base.Interact(referenceCheck);
        rigidBody.maxAngularVelocity = 10f;
    }

    public override void StopInteraction(VR_Controller_Custom referenceCheck)
    {
        if (linkedController == referenceCheck)
        {
            rigidBody.useGravity = true;
            base.StopInteraction(referenceCheck);
            rigidBody.velocity = referenceCheck.Velocity();
            rigidBody.angularVelocity = referenceCheck.AngularVelocity();
        }
    }

    public override void UpdatePosition()
    {
        Vector3 PositionDelta = (linkedController.transform.position - transform.position);

        Quaternion RotationDelta = linkedController.transform.rotation * Quaternion.Inverse(this.transform.rotation);
        float angle;
        Vector3 axis;
        RotationDelta.ToAngleAxis(out angle, out axis);

        if (angle > 180)
            angle -= 360;

        rigidBody.angularVelocity = axis * angle * 0.4f;

        rigidBody.velocity = PositionDelta * 40 * rigidBody.mass;
    }

    protected virtual void OnCollisionEnter(Collision collision)
    {

        if (linkedController != null)
        {
            float value = linkedController.Velocity().magnitude <= 1 ? linkedController.Velocity().magnitude : 1;
            linkedController.Vibrate(value / 10);

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
