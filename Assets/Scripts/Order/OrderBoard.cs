using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;

public class OrderBoard : MonoBehaviour {

    
    public static OrderBoard Instance;
    public GameObject test;
    [SerializeField]private float offset;

    [SerializeField]private Vector2 displayableSize;
    [SerializeField] private OrderSlip order;

    private GameObject canvas;
    private GameObject panel;
    private List<OrderSlip> orderList = new List<OrderSlip>();
    private Vector3[] slots;
    private BoxCollider boxCollider;
    [SerializeField]
    private Vector2 padding;
    [SerializeField]
    private Vector2 spacing;
    [SerializeField] private Vector2 cellSize;

    [SerializeField]
    private int colElements;
    [SerializeField]
    private int rowElements;

    [SerializeField] [Range(1,12)] private int maxNumberOfOrders;
    [SerializeField]
    [Range(1, 12)]
    private int rows;

    public int CurrentNumberOfOrders
    {
        get { return orderList.Count; }
    }

    public bool IsMaxedOut
    {
        get { return orderList.Count >= maxNumberOfOrders; }
    }

    void Awake()
    {
        Instance = this;
    }


	// Use this for initialization
	void Start ()
    {
        maxNumberOfOrders = rowElements * colElements;
        slots = new Vector3[maxNumberOfOrders];
        boxCollider = GetComponent<BoxCollider>();
        canvas = transform.Find("Canvas").gameObject;
        panel = canvas.transform.Find("RequestPanel").gameObject;

        if (panel == null)
            Debug.Log("RequestPanel missing!");

        GenerateSlots();

        
        for (int i =0; i<maxNumberOfOrders;i++)
        {
            //Instantiate(test, new Vector3( ,transform.forward * offset + transform.position
            //    + new Vector3(-boxCollider.bounds.extents.x, boxCollider.bounds.extents.y,0)
            //    , test.transform.rotation);
            Instantiate(test, slots[i] + transform.forward * offset
               , test.transform.rotation);
        }

        //Instantiate(test, transform.position + transform.forward * offset, test.transform.rotation);

        //GameObject g  = Instantiate(test, gameObject.transform);
        //g.transform.localPosition += offset;




    }

    //private Vector3 GetEmptySlot(int index)
    //{
       
    //}


    private void GenerateSlots()
    {
        BoxCollider paperCol = test.GetComponent<BoxCollider>();

        Vector3 paper = new Vector3(paperCol.size.x * test.transform.localScale.x,paperCol.size.z * test.transform.localScale.y,1);
        
        Debug.Log("pp"+paper);
        Vector3 extent = boxCollider.bounds.extents;
        Debug.Log(boxCollider.bounds.extents.x);
        float tLength = extent.x * 2;
        float tHeight = extent.y * 2;

        float iLength = tLength - 2 * (padding.x);
        float iHeight = tHeight - 2 * (padding.y);

        Debug.Log(iLength);

        Vector2 gap = Vector2.zero;

        gap.x = ( iLength - (rowElements * paper.x) ) / (rowElements - 1);
        gap.y = (iHeight - (colElements * paper.y) ) / (colElements - 1);

        Debug.Log("Gap is" + gap.x);


        Vector3 spawnSlot = transform.position +  
                (transform.right) * (boxCollider.bounds.extents.x - paper.x/2 - padding.x) +
                (transform.up) * (boxCollider.bounds.extents.y - paper.y/2 - padding.y);
        

        for (int i=0;i< maxNumberOfOrders;i++)
        {
            int finalRowIndex = i % rowElements;
            int finalColIndex = i / rowElements;

            Vector3 v = new Vector3( -((gap.x * finalRowIndex) + (paper.x * finalRowIndex) ), -((gap.y * finalColIndex) + (paper.y * finalColIndex) ));

            Debug.Log("Paper " + i + " : " + v);

            slots[i] = spawnSlot + v ;
        }


    }


    // Spawns an order slip on the order board
    public void SpawnOnBoard(Order o)
    {
        if(orderList.Count < maxNumberOfOrders)
        {
            OrderSlip g = Instantiate(order, panel.transform).GetComponent<OrderSlip>();
            orderList.Add(g);
            g.StartOrder(o, CloseOrder);
        }
    }

    public void CloseOrder(bool success,OrderSlip slip)
    {
        orderList.Remove(slip);
        OrderManager.Instance.CompletedOrder(success, slip.Reward);
    }


    // Generates relevant order information on the order slip


 
}
