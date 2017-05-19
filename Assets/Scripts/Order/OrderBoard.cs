
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;

public class OrderBoard : MonoBehaviour
{

    class Slot
    {
        public Vector3 slotPosition;

        public bool isTaken;

        public OrderSlip refOrder;



        public Slot(Vector3 slotPosition)

        {
            this.slotPosition = slotPosition;

            isTaken = false;
        }

    }

   

    public static OrderBoard Instance;
    public GameObject test;
    [SerializeField]
    private float offsetZ;

    [SerializeField]
    private OrderSlip order;

    private List<OrderSlip> orderList = new List<OrderSlip>();
    private Slot[] slots;
    private BoxCollider boxCollider;


    [Header("Specify number of elements in column/row")]
    [SerializeField]

    private int colElements;    // Number of elements in column
    [SerializeField]

    private int rowElements;    // Number of elements in row

    [SerializeField]

    private Vector2 padding;



    private int maxNumberOfOrders;


    public int CurrentNumberOfOrders
    {
        get { return orderList.Count; }
    }

    void Awake()
    {
        Instance = this;
    }

    public bool IsMaxedOut
    {
        get
        {
            return orderList.Count >= maxNumberOfOrders;
        }
    }
    // Use this for initialization
    void Start()
    {
        maxNumberOfOrders = rowElements * colElements;
        slots = new Slot[maxNumberOfOrders];
        boxCollider = GetComponent<BoxCollider>();
        GenerateSlots();
    }

    private void GenerateSlots()
    {
        BoxCollider paperCol = test.GetComponent<BoxCollider>();
        Vector3 paper = new Vector3(paperCol.size.x * test.transform.localScale.x, paperCol.size.z * test.transform.localScale.y, 1);

        Debug.Log("pp" + paper);

        Vector3 extent = boxCollider.bounds.extents;

        Debug.Log(boxCollider.bounds.extents.x);

        float tLength = extent.x * 2;

        float tHeight = extent.y * 2;

        float iLength = tLength - 2 * (padding.x);

        float iHeight = tHeight - 2 * (padding.y);

        Debug.Log(iLength);

        Vector2 gap = Vector2.zero;

        gap.x = (iLength - (rowElements * paper.x)) / (rowElements - 1);

        gap.y = (iHeight - (colElements * paper.y)) / (colElements - 1);

        Debug.Log("Gap is" + gap.x);
        Vector3 spawnSlot = transform.position +
                (transform.right) * (boxCollider.bounds.extents.x - paper.x / 2 - padding.x) +
                (transform.up) * (boxCollider.bounds.extents.y - paper.y / 2 - padding.y);
        for (int i = 0; i < maxNumberOfOrders; i++)
        {
            int finalRowIndex = i % rowElements;
            int finalColIndex = i / rowElements;
            Vector3 v = new Vector3(-((gap.x * finalRowIndex) + (paper.x * finalRowIndex)), -((gap.y * finalColIndex) + (paper.y * finalColIndex)));
            Debug.Log("Paper " + i + " : " + v);
            slots[i] = new Slot(spawnSlot + v);
        }

    }



    private Slot GetAvailableSlot()

    {

        foreach (Slot s in slots)

        {
            if (!s.isTaken)
                return s;
        }
        return null;
    }

    public void RemoveFromBoard(OrderSlip order)
    {
        foreach (Slot s in slots)

        {

            if (s.refOrder == order)

            {

                Destroy(s.refOrder.gameObject);

                s.isTaken = false;

                break;

            }

        }
    }

    public void SpawnOnBoard(Order o)
    {
        if (orderList.Count < maxNumberOfOrders)
        {

            Slot acquiredSlot = GetAvailableSlot();
            OrderSlip g = Instantiate(order, acquiredSlot.slotPosition + transform.forward * offsetZ, order.transform.rotation).GetComponent<OrderSlip>();
            acquiredSlot.isTaken = true;
            acquiredSlot.refOrder = g;
            orderList.Add(g);
            g.StartOrder(o, CloseOrder);


        }
    }

    public void CloseOrder(bool success, OrderSlip slip)
    {
        orderList.Remove(slip);
        OrderManager.Instance.CompletedOrder(success, slip.Reward);
    }


    // Generates relevant order information on the order slip



}
