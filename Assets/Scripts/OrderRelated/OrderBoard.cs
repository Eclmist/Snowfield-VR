using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;

public class OrderBoard : MonoBehaviour {

    public static OrderBoard Instance;

    [SerializeField] private OrderSlip order;

    private GameObject canvas;
    private GameObject panel;
    private List<OrderSlip> orderList = new List<OrderSlip>();
    //Do we want to keep a list of the orders? Feels more clean that way

    [SerializeField] [Range(1,12)] private int maxNumberOfOrders;

    public int CurrentNumberOfOrders
    {
        get { return orderList.Count; }
    }

    void Awake()
    {
        Instance = this;
    }


	// Use this for initialization
	void Start ()
    {

        canvas = transform.Find("Canvas").gameObject;
        panel = canvas.transform.Find("RequestPanel").gameObject;

        if (panel == null)
            Debug.Log("RequestPanel missing!");

	}
	
	//void Update ()
 //   {

 //       if(Input.GetKeyDown(KeyCode.I))
 //       {
 //           OrderManager.Instance.NewRequest();
 //       }
        
	//}
    
    // Spawns an order slip on the order board
    public void SpawnOnBoard(Order o)
    {
        if(orderList.Count < maxNumberOfOrders)
        {
            OrderSlip g = Instantiate(order, panel.transform).GetComponent<OrderSlip>();
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
