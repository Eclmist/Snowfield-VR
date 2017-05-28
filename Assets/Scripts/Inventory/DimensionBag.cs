using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DimensionBag : GenericItem {

    public class DimensionSlot
    {
        int stackSize;
        GenericItem item;

        public DimensionSlot(GenericItem item)
        {
            this.stackSize = 1;
            this.item = item;   
        }

        public int StackSize { get; set; }
        public GenericItem Item { get; set; }

    }

    public static DimensionBag Instance;

    private List<GenericItem> dimensionItems;
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
        dimensionItems = new List<GenericItem>();
        GameObject g = Instantiate(ItemManager.Instance.Items[12]);
        AddItemToDimension(g.GetComponent<GenericItem>());
        GameObject ga = Instantiate(ItemManager.Instance.Items[12]);
        AddItemToDimension(ga.GetComponent<GenericItem>());

    }
	
    void Update()
    {
        if (Vector3.Distance(Player.Instance.transform.position,transform.position ) < distanceToReact)
        {
            // Play animation



        }

        Debug.Log("number of things in list" + dimensionItems.Count);

    }

    public List<GenericItem> Items
    {
        get { return this.dimensionItems; }
        set { this.dimensionItems = value; }
    }


    public void AddItemToDimension(GenericItem item)
    {
        bool added = false;

        if(item != null)
        {
            // Check for existing items to stack
            foreach(GenericItem d in dimensionItems)
            {
                Debug.Log("d " + d.ID);
                Debug.Log("item " + item.ID);

                if(item.ID == d.ID )//&& d.CurrentStackSize < d.MaxStackSize)
                {
                    Debug.Log("im here");
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

    public void RetrieveItemFromDimension()
    {
        GenericItem d = dimensionItems[itor];
        Debug.Log("sizeeeee" + d.CurrentStackSize);
        d.CurrentStackSize--;

        GameObject g = Instantiate(d.objReference);
        g.GetComponent<GenericItem>().StartInteraction(LinkedController);

        if (d.CurrentStackSize < 1)
        {
            dimensionItems.Remove(d);
            itor = 0;
        }

        

    }

    public GenericItem GetSelectedItem()
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
                StartInteraction(referenceCheck);
            }
        }
        

    }


    public override void StartInteraction(VR_Controller_Custom referenceCheck)
    {
        base.StartInteraction(referenceCheck);
        RetrieveItemFromDimension();

    }

    void OnDrawGizmos()
    {
        Gizmos.DrawWireSphere(transform.position,distanceToReact);
    }

}
