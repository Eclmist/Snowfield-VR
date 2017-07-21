using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StoragePanel : Inventory {

    public static StoragePanel Instance;

    private void Awake()
    {
        Instance = this;   
    }

    private List<InventorySlot> slotReferenceList = new List<InventorySlot>();
    [SerializeField]
	private GameObject interactiveSlot;
	[SerializeField]
	private GameObject slotPanel;	// Contains the gridLayoutGroup
    private GridLayoutGroup glp;

    private int numberOfHoveredSlots;
    private bool safeToUse;

    public int NumberOfHoveredSlots
    {
        get { return this.numberOfHoveredSlots; }
        set { this.numberOfHoveredSlots = value; }
    }

    public bool SafeToUse
    {
        get { return this.safeToUse; }
    }

	protected override void Start()
	{
        base.Start();

        glp = slotPanel.GetComponent<GridLayoutGroup>();
		InitializeInteractableSlots ();
	}


	void Update()
	{
        safeToUse = numberOfHoveredSlots <= 1;

        if(Input.GetKeyDown(KeyCode.Space))
        {
            Debug.Log(StoreInAvailableSlot(ItemManager.Instance.GetItemData(6),2));
        }
	}

    


	private void InitializeInteractableSlots()
	{
		foreach (InventorySlot slot in InventoryItems)
		{
			GameObject g = Instantiate (interactiveSlot);
            g.transform.SetParent(glp.transform,false);
            g.transform.localPosition = Vector3.zero;
            g.transform.localScale = Vector3.one;

            g.GetComponent<InteractableSlot>().Slot = slot;
            slotReferenceList.Add(g.GetComponent<InteractableSlot>().Slot);

		}

	}

    public bool StoreInAvailableSlot(IStorable item,int quantity)
    {
        foreach(InventorySlot slot in slotReferenceList)
        {
            if(slot.StoredItem == null)
            {
                slot.StoredItem = item;
                slot.CurrentStack = quantity;
                return true;

            }
        }

        return false;
    }














}
