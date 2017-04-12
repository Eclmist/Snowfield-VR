using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class OrderBoard : MonoBehaviour {

    public static OrderBoard Instance;
    public GameObject order;

    private GameObject canvas;
    private GameObject panel;
    [HideInInspector]
    public const int maxNumberOfOrders = 12;
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
	void Start () {

        canvas = transform.Find("Canvas").gameObject;
        panel = canvas.transform.Find("RequestPanel").gameObject;

        if (panel == null)
            Debug.Log("RequestPanel missing!");

	}
	
	// Update is called once per frame
	void Update () {
		
        if(Input.GetKeyDown(KeyCode.I))
        {
            SpawnOnBoard();
            
        }

        Debug.Log("orders: "+ currentNumberOfOrders);
        
	}
    

    public void SpawnOnBoard()
    {
        if(currentNumberOfOrders < maxNumberOfOrders)
        {
            Instantiate(order, panel.transform);
            currentNumberOfOrders++;
        }

        
    }

    // TO DO
    //public void SpawnOnBoard(Order r)
    //{
    //    Instantiate(order, panel.transform);

    //}
}
