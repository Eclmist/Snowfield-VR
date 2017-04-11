using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlacksmithItem : GenericItem {

    public Material initialMaterial;
    public Material forgingMaterial; // Material to lerp to when heated
    protected MeshRenderer meshRenderer; // To modify the material
    protected float currentTemperature; // Current temperature of the item
    protected const float baseRate = 0.0001f; // The base heat up rate before multiplying the conductivity
    private bool heatSourceDetected;
    private float distFromHeat;
    private float quenchRate = 1;


    [SerializeField]
    [Range(1, 10)]
    protected float conductivity; // Acts as a multiplier for the heating rate

    public void Start()
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

        Debug.Log(heatSourceDetected);
        Debug.Log(currentTemperature);
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
        
        }
        else if(!heatSourceDetected && currentTemperature > 0)
        {
            currentTemperature -= baseRate * conductivity * quenchRate;
        }
       

    }



    // Changes color (Material) when heated
    private void RespondToHeat()
    {
       meshRenderer.material.Lerp(initialMaterial, forgingMaterial, currentTemperature);
    }



    







    

    




}
