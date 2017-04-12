using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class OrderBoard : MonoBehaviour {

    public static OrderBoard Instance;

    [SerializeField] private OrderSlip order;

    private GameObject canvas;
    private GameObject panel;
    [HideInInspector]
    public const int maxNumberOfOrders = 12;
    //Do we want to keep a list of the orders? Feels more clean that way
    private int currentNumberOfOrders;


    public int CurrentNumberOfOrders
    {
        get { return this.currentNumberOfOrders; }
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
        if(currentNumberOfOrders < maxNumberOfOrders)
        {
            OrderSlip g = Instantiate(order, panel.transform).GetComponent<OrderSlip>();
            g.StartOrder(o);
            currentNumberOfOrders++;
        }
    }

    public void CloseOrder()
    {
        currentNumberOfOrders--;
    }
    // Generates relevant order information on the order slip
 
}
