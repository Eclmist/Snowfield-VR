using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StoragePanel : MonoBehaviour {

    public static StoragePanel Instance;

    private void Awake()
    {
        Instance = this;   
    }


    [SerializeField]
    private int numberOfSlots;
    [SerializeField]
    private int numberOfPages;
    private Inventory inventory;
    private int currentPageNumber = 0;
    //private List<InventorySlot> slotReferenceList = new List<InventorySlot>();
    [SerializeField]
	private GameObject interactiveSlot;
	[SerializeField]
	private GameObject slotPanel;	// Contains the gridLayoutGroup
    private GridLayoutGroup glp;

    public int NumberOfSlotsPerPage
    {
        get { return this.numberOfSlots; }
    }

    public int NumberOfPages
    {
        get { return this.numberOfPages; }
    }

    public int CurrentPageNumber
    {
        get { return this.currentPageNumber; }
    }

    public Inventory _Inventory
    {
        get { return this.inventory; }
    }

    private void Start()
	{
        inventory = new Inventory();
        glp = slotPanel.GetComponent<GridLayoutGroup>();
		InitializeInteractableSlots ();
	}


	void Update()
	{
        if(Input.GetKeyDown(KeyCode.Space))
        {
            inventory.AddToInventory(ItemManager.Instance.GetItemData(2));
        }
	}

    

    // Create the intereactable slots
	private void InitializeInteractableSlots()
	{
		for(int i = 0;i<numberOfSlots; i++)
		{
			GameObject g = Instantiate (interactiveSlot);
            g.transform.SetParent(glp.transform,false);
            g.transform.localPosition = Vector3.zero;
            g.transform.localScale = Vector3.one;

            g.GetComponent<InteractableSlot>().Index = i;

            //g.GetComponent<InteractableSlot>().Slot = slot;
            //slotReferenceList.Add(g.GetComponent<InteractableSlot>().Slot);

        }

	}


    public void DisplayNextPage()
    {
        if (currentPageNumber < numberOfPages-1)
            currentPageNumber++;
    }

    public void DisplayPrevPage()
    {
        if(currentPageNumber > 0 )
        {
            currentPageNumber--;
        }
    }
    
    


}
