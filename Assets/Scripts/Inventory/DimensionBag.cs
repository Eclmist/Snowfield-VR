using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DimensionBag : GenericItem {

    public static DimensionBag Instance;

    private List<IDimensionable> dimensionItems;
    private int itor = 0;
    private bool isInside;

    [SerializeField]
    [Range(1f,5f)]
    private float distanceToReact;

    protected override void Awake()
    {
        base.Awake();
        Instance = this;
    }

	// Use this for initialization
	void Start ()
    { 
        dimensionItems = new List<IDimensionable>();

            

	}
	
    void Update()
    {
        if (Vector3.Distance(Player.Instance.transform.position,transform.position ) < distanceToReact)
        {
            // Play animation



        }
            
    }

    public List<IDimensionable> Items
    {
        get { return this.dimensionItems; }
        set { this.dimensionItems = value; }
    }


    public void AddItemToDimension(IDimensionable item)
    {
        bool added = false;

        if(item != null)
        {
            // Check for existing items to stack
            foreach(IDimensionable d in dimensionItems)
            {
                if(item.Name == d.Name && d.CurrentStackSize < d.MaxStackSize)
                {
                    d.CurrentStackSize++;
                    added = true;
                    break;
                }
            }


            if(!added)
            {
                dimensionItems.Add(item);
                item.CurrentStackSize++;
            }

        }
            
    }

    public void RetrieveItemFromDimension(Transform t)
    {
        IDimensionable d = dimensionItems[itor];
        d.CurrentStackSize--;

        Instantiate(d.objReference, t);

        if (d.CurrentStackSize < 1)
        {
            dimensionItems.Remove(d);
        }

    }

    public IDimensionable GetSelectedItem()
    {

        return dimensionItems[itor];

    }

    void OnTriggerStay(Collider other)
    {
        isInside = true;
    }

    void OnTriggerExit(Collider other)
    {
        isInside = false;
    }

    public override void Interact(VR_Controller_Custom referenceCheck)
    {


        if(isInside)
        {
            if (referenceCheck.Device.GetPress(SteamVR_Controller.ButtonMask.Touchpad))
            {
                Vector2 touchpad = (referenceCheck.Device.GetAxis(Valve.VR.EVRButtonId.k_EButton_Axis0));

                if (referenceCheck.Device.GetAxis(Valve.VR.EVRButtonId.k_EButton_Axis0)[0] > 0.6f
                && referenceCheck.Device.GetPressDown(Valve.VR.EVRButtonId.k_EButton_SteamVR_Touchpad)
                && itor < dimensionItems.Count-1)
                {
                    itor++;
                }

                if (referenceCheck.Device.GetAxis(Valve.VR.EVRButtonId.k_EButton_Axis0)[0] < -0.6f
                && referenceCheck.Device.GetPressDown(Valve.VR.EVRButtonId.k_EButton_SteamVR_Touchpad)
                && itor > 0)
                {
                    itor--;
                }

            }


            if (referenceCheck.Device.GetTouchDown(SteamVR_Controller.ButtonMask.Trigger) && dimensionItems.Count > 0)
            {
                RetrieveItemFromDimension(LinkedController.transform);
            }
        }
        

    }

    public override void StopInteraction(VR_Controller_Custom referenceCheck)
    {
        base.Interact(referenceCheck);

    }

    public override void StartInteraction(VR_Controller_Custom referenceCheck)
    {
        base.Interact(referenceCheck);
 
        
    }

    void OnDrawGizmos()
    {
        Gizmos.DrawWireSphere(transform.position,distanceToReact);
    }

}
