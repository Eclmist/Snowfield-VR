using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DimensionBag : GenericItem {

    private List<IDimensionable> dimensionItems;
    private int itor = 0;
    private bool isInside;

    [SerializeField]
    private float distanceToReact;

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
            Debug.Log("Open dimension bag!");
        }
            
    }

    public List<IDimensionable> Items
    {
        get { return this.dimensionItems; }
        set { this.dimensionItems = value; }
    }


    public void AddItemToDimension(IDimensionable item)
    {
        if(item != null)
            dimensionItems.Add(item);
    }

    public GameObject RetrieveItemFromDimension()
    {
        return dimensionItems[itor].objReference;
    }

    public IDimensionable ShowSelectedItem()
    {

        return dimensionItems[itor];

    }

    protected override void OnCollisionEnter(Collision col)
    {
        isInside = true;
    }

    void OnCollisionExit(Collision col)
    {
        isInside = false;
    }

    public override void Interact(VR_Controller_Custom referenceCheck)
    {

        base.Interact(referenceCheck);

        if(isInside)
        {
            if (referenceCheck.Device.GetPress(SteamVR_Controller.ButtonMask.Touchpad))
            {
                Vector2 touchpad = (referenceCheck.Device.GetAxis(Valve.VR.EVRButtonId.k_EButton_Axis0));

                if (touchpad.x > 0.7f && itor < dimensionItems.Count)
                    itor++;
                else if (touchpad.x < -0.7f && itor > 1)
                    itor--;

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
