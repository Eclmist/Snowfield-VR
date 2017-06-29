//using System;
//using System.Collections;
//using System.Collections.Generic;
//using UnityEngine;
//using UnityEngine.UI;

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
    public GameObject thoughtBubble;

    private List<DimensionSlot> dimensionItems;
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
        dimensionItems = new List<DimensionSlot>();


        AddItemToDimension(ItemManager.Instance.GetItem(12));
        AddItemToDimension(ItemManager.Instance.GetItem(12));

    }
	
    void Update()
    {
        if (Vector3.Distance(Player.Instance.transform.position,transform.position ) < distanceToReact)
        {
            // Play animation

        }

        thoughtBubble.SetActive(isInside);


        Debug.Log(isInside);
        
        


    }

    public List<DimensionSlot> Items
    {
        get { return this.dimensionItems; }
        set { this.dimensionItems = value; }
    }

//    public List<DimensionSlot> Items
//    {
//        get { return this.dimensionItems; }
//        set { this.dimensionItems = value; }
//    }


        if(item != null)
        {


            // Check for existing items to stack
            foreach (DimensionSlot d in dimensionItems)
            {
                if (item.ID == d.Item.ID && d.StackSize < d.Item.MaxStackSize)
                {
                    d.StackSize++;
                    added = true;
                    break;
                }
            }


            if(!added)
            {
                DimensionSlot slot = new DimensionSlot(item);
                slot.Item = item;
                slot.StackSize = 1;
                dimensionItems.Add(slot);
            }

        }
            
    }

    public void RetrieveItemFromDimension()
    {

        DimensionSlot d = dimensionItems[itor];
        d.StackSize--;

        GameObject g = Instantiate(d.Item.objReference);
        g.GetComponent<GenericItem>().StartInteraction(LinkedController);

        if (d.StackSize < 1)
        {
            dimensionItems.Remove(d);
            itor = 0;
        }

        

    }

    public GenericItem GetSelectedItem()
    {

        if (dimensionItems.Count < 1)
            return null;
        else
            return dimensionItems[itor].Item;

    }

    void OnTriggerStay(Collider other)
    {
        if(other.gameObject.GetComponent<VR_Controller_Custom>() != null)
        {
            isInside = true;
        }
         
    }

    void OnTriggerExit(Collider other)
    {
        if (other.gameObject.GetComponent<VR_Controller_Custom>() != null)
        {
            isInside = false;
        }
            
    }

//    public override void Interact(VR_Controller_Custom referenceCheck)
//    {


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
                this.audioSource.Play();
                StartInteraction(referenceCheck);
            }
        }
        

    }


//    public override void StartInteraction(VR_Controller_Custom referenceCheck)
//    {
//        base.StartInteraction(referenceCheck);
//        RetrieveItemFromDimension();

//    }

    void OnDrawGizmos()
    {
        Gizmos.DrawWireSphere(transform.position,distanceToReact);
    }

//}


public class DimensionBag : GenericItem {

    public static DimensionBag Instance;

    [SerializeField]
    private List<GameObject> dimensionItems;

    private bool isInside;

    [SerializeField]
    [Range(1f, 5f)]
    private float distanceToReact;

    protected override void Awake()
    {
        base.Awake();
        Instance = this;
    }

    // Use this for initialization
    void Start()
    {
        dimensionItems = new List<GameObject>();
    }
    void Update()
    {
        if (Vector3.Distance(Player.Instance.transform.position, transform.position) < distanceToReact)
        {
            // Play animation
        }
    }

    public List<GameObject> Items
    {
        get { return this.dimensionItems; }
        set { this.dimensionItems = value; }
    }
    public void RetrieveItemFromDimension()
    {
        GameObject g = Instantiate(dimensionItems[UnityEngine.Random.Range(0,dimensionItems.Count)]);
        g.GetComponent<GenericItem>().StartInteraction(LinkedController);
   
    }

    void OnTriggerStay(Collider other)
    {
        if (other.gameObject.GetComponent<VR_Controller_Custom>() != null)
        {
            isInside = true;
        }

    }
    void OnTriggerExit(Collider other)
    {
        if (other.gameObject.GetComponent<VR_Controller_Custom>() != null)
        {
            isInside = false;
        }

    }
        if (isInside)
        {
            if (referenceCheck.Device.GetTouchDown(SteamVR_Controller.ButtonMask.Trigger) && dimensionItems.Count > 0)
            {
                this.audioSource.Play();
                StartInteraction(referenceCheck);
            }
        }

    }
    void OnDrawGizmos()
    {
        Gizmos.DrawWireSphere(transform.position, distanceToReact);
    }