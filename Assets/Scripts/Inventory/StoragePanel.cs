using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StoragePanel : Inventory {

	[SerializeField]
	private GameObject interactiveSlot;
	[SerializeField]
	private GameObject panel;	// The physical storage panel
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

        slotPanel = panel.transform.Find("slotPanel").gameObject;
        glp = slotPanel.GetComponent<GridLayoutGroup>();
		InitializeInteractableSlots ();
	}


	void Update()
	{
        safeToUse = numberOfHoveredSlots <= 1;
	}

    


	private void InitializeInteractableSlots()
	{
		foreach (InventorySlot slot in InventoryItems)
		{
			GameObject g = Instantiate (interactiveSlot);
            g.transform.SetParent(glp.transform);
            g.transform.localPosition = Vector3.zero;
            g.transform.localScale = Vector3.one;

            g.GetComponent<InteractableSlot>().Slot = slot;

		}

	}












}
