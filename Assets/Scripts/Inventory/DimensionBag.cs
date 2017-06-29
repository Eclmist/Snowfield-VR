//using System;
//using System.Collections;
//using System.Collections.Generic;
//using UnityEngine;
//using UnityEngine.UI;

//public class DimensionBag : GenericItem {

//    public static DimensionBag Instance;

//    [SerializeField]
//    private List<GameObject> dimensionItems;

//    private bool isInside;

//    [SerializeField]
//    [Range(1f, 5f)]
//    private float distanceToReact;

//    protected override void Awake()
//    {
//        base.Awake();
//        Instance = this;
//    }
//    // Use this for initialization
//    void Start()
//    {
//        dimensionItems = new List<GameObject>();
//    }
//    void Update()
//    {
//        if (Vector3.Distance(Player.Instance.transform.position, transform.position) < distanceToReact)
//        {
//            // Play animation
//        }
//    }

//    public List<GameObject> Items
//    {
//        get { return this.dimensionItems; }
//        set { this.dimensionItems = value; }
//    }
//    public void RetrieveItemFromDimension()
//    {
//        GameObject g = Instantiate(dimensionItems[UnityEngine.Random.Range(0,dimensionItems.Count)]);
//        //g.GetComponent<GenericItem>().StartInteraction(LinkedController);
   
//    }
//    void OnTriggerStay(Collider other)
//    {
//        if (other.gameObject.GetComponent<VR_Controller_Custom>() != null)
//        {
//            isInside = true;
//        }
//    }
//    void OnTriggerExit(Collider other)
//    {
//        if (other.gameObject.GetComponent<VR_Controller_Custom>() != null)
//        {
//            isInside = false;
//        }
//    }
//    public override void Interact(VR_Controller_Custom referenceCheck)
//    {
//        if (isInside)
//        {
//            if (referenceCheck.Device.GetTouchDown(SteamVR_Controller.ButtonMask.Trigger) && dimensionItems.Count > 0)
//            {
//                this.audioSource.Play();
//                StartInteraction(referenceCheck);
//            }
//        }
//    }
//    public override void StartInteraction(VR_Controller_Custom referenceCheck)
//    {
//        base.StartInteraction(referenceCheck);
//        RetrieveItemFromDimension();
//    }
//    void OnDrawGizmos()
//    {
//        Gizmos.DrawWireSphere(transform.position, distanceToReact);
//    }
//}
