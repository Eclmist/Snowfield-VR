using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlacksmithItem : GenericItem {

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

    public void Update()
    {

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
        transform.localPosition = referenceCheck.transform.position;
        transform.rotation = referenceCheck.transform.rotation;
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

        rigidBody.angularVelocity = axis * angle * Time.fixedDeltaTime * 40;

        rigidBody.velocity = PositionDelta * 4000 * rigidBody.mass * Time.fixedDeltaTime;

    }



















}

public class BlacksmithItem : GenericItem{
    protected PhysicalMaterial physicalMaterial;
    

    public override void UpdatePosition()
    {
        throw new NotImplementedException();
    }