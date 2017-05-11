﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ingot : BlacksmithItem {

    [SerializeField]
    private int oreComposition;

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


    public PhysicalMaterial PhysicalMaterial
    {
        get { return physicalMaterial; }
        set { physicalMaterial = value; }
    }

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

    public int OreComposition
    {
        get { return this.oreComposition; }
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
            currentTemperature += baseRate * physicalMaterial.Conductivity * distFromHeat;
            if (currentTemperature > 1)
                currentTemperature = 1;

        }
        else if(!heatSourceDetected && currentTemperature > 0)
        {
            currentTemperature -= (baseRate * physicalMaterial.Conductivity * quenchRate);
            if (currentTemperature < 0)
                currentTemperature = 0;
        }
       

    }

    // Changes color (Material) when heated
    private void RespondToHeat()
    {
       meshRenderer.material.Lerp(initialMaterial, forgingMaterial, currentTemperature);
    }


}