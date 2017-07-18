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

	protected override void Start()
	{
        base.Start();

        slotPanel = panel.transform.Find("slotPanel").gameObject;
        glp = slotPanel.GetComponent<GridLayoutGroup>();
		InitializeInteractableSlots ();
	}


	void Update()
	{

        

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
