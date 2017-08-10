using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ingot : BlacksmithItem {

    #region tutorial
    public static bool pickedUpIngot = false;
    public static bool isHotEnough = false;
    public static bool isPlacedOnAnvil= false;
    #endregion
    
    [SerializeField][Range(1,10)]
    private int oreComposition;

    private Material initialMaterial;
    public Material forgingMaterial; // Material to lerp to when heated
    private TempKillScript tempKill; // To show that ingot failed
    [SerializeField]
    protected MeshRenderer meshRenderer; // To modify the material
    [SerializeField]
    protected float currentTemperature; // Current temperature of the item
    protected const float baseRate = 0.0001f; // The base heat up rate before multiplying the conductivity
    [SerializeField]
    private bool heatSourceDetected;
    private float distFromHeat;
    private float quenchRate = 0.01f;
    protected IngotDeformer ingotDeformer;

    private int currentMorphSteps;
    private int targetMorphSteps;
	private int preNumberOfHits = 3;
	[SerializeField]
	private bool onAnvil;
	[SerializeField]
	private bool isMorphable;

	public bool IsMorphable
	{
		get { return this.isMorphable; }
	}

	public bool OnAnvil
	{
		get { return this.onAnvil; }
		set { this.onAnvil = value; }
	}

	public int PreNumberOfHits
	{
		get { return this.preNumberOfHits; }
		set { this.preNumberOfHits = value; }
	}

    public PhysicalMaterial PhysicalMaterial
    {
        get { return physicalMaterial; }
        set { physicalMaterial = value; }
    }

    protected override void Start()
    {
		base.Start();

		meshRenderer = GetComponent<MeshRenderer>();
        tempKill = GetComponent<TempKillScript>();

		if (meshRenderer == null)
        {
            Debug.Log("no meshrenderer in ingot, ingot will be destroyed");
            Destroy(this);
        }
        else
        {
            ingotDeformer = GetComponent<IngotDeformer>();
			initialMaterial = Instantiate(meshRenderer.material);
            currentTemperature = 0; // Assume 0 to be room temperature for ez calculations
        }

        currentMorphSteps = 0;
    }

	protected override void Update()
	{
		base.Update();
		isMorphable = (currentTemperature > 0.8f && currentMorphSteps >= targetMorphSteps && onAnvil && currentMorphSteps > 0);

        pickedUpIngot = (LinkedController != null);
        isHotEnough = currentTemperature >= 0.95f;
        isPlacedOnAnvil = onAnvil;
	}

	public void LateUpdate()
    {

        //if (Input.GetKeyDown(KeyCode.E))
        //    quenchRate = 10;

        RespondToHeat();
        ControlledHeatingProcess();

        UpdateMorpher();

    }

    public void IncrementMorphStep()
    {
		// If it is the very first hit
        if(currentMorphSteps == 0)
        {

			preNumberOfHits = (int)Random.Range(2, 3);
			// Generate morph chance
			if (WeaponTierManager.Instance.WeaponClassList != null)
				targetMorphSteps = (Random.Range(0, WeaponTierManager.Instance.GetNumberOfTiersInClass(physicalMaterial.type)) + preNumberOfHits);
				//targetMorphSteps = 1; // Random.Range(1,WeaponTierManager.Instance.GetNumberOfTiersInClass(physicalMaterial.type));
			
        }

        currentMorphSteps++;

        if(isMorphable)
        {
            if(Random.Range(1,100) >= WeaponTierManager.Instance.GetSuccessRateForTier(physicalMaterial.type))
            {
                // succeeded
                ItemData itemData = WeaponTierManager.Instance.GetWeapon(physicalMaterial.type, targetMorphSteps - preNumberOfHits);
                if(itemData != null)
                {
    				FakeItem fakeItem = new FakeItem();
    				fakeItem.trueForm = itemData;

                    GameObject g = Instantiate(itemData.ObjectReference.GetComponent<CraftedItem>().GetFakeself(), transform.position, transform.rotation);
    				g.AddComponent<FakeItem>();
    				g.GetComponent<FakeItem>().trueForm = itemData;
                    Destroy(this.gameObject); 
                }

            }
            else
            {   
                // Fail
                tempKill.Kill();
                Destroy(this.gameObject); 
            }
        }
    }



    //--------------- Properties -----------------//

    public int OreComposition
    {
        get { return this.oreComposition; }
    }

    
    protected void UpdateMorpher()
    {
        if (currentTemperature > .8)
            ingotDeformer.enabled = true;
        else
            ingotDeformer.enabled = false;
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
