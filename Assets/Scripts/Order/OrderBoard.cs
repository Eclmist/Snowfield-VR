using System;
using System.Collections.Generic;
using UnityEngine;

public class OrderBoard : MonoBehaviour,ICanSerialize
{
    [SerializeField]
    private List<Transform> t;

	private class Slot
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

	[SerializeField]
	private float offsetZ;

    [SerializeField]
    private GameObject orderG;

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

	private void Awake()
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

    public string SerializedFileName
    {
        get
        {
            return "OrderBoard";
        }
    }

    // Use this for initialization
    private void Start()
	{
		maxNumberOfOrders = rowElements * colElements;
		slots = new Slot[maxNumberOfOrders];
		boxCollider = GetComponent<BoxCollider>();
		GenerateSlots2();

        Load();
    }

	public void RemoveFromBoard(OrderSlip order)
	{
		foreach (Slot s in slots)
		{
			if (s.refOrder == order)
			{
				Destroy(s.refOrder.gameObject);
				s.isTaken = false;
                Debug.Log("removed");
				break;
			}
		}
	}


	public void SpawnOnBoard(Order o)
	{
		if (orderList.Count < maxNumberOfOrders)
		{
			Slot acquiredSlot = GetAvailableSlot();
			OrderSlip g = Instantiate(orderG, acquiredSlot.slotPosition + transform.forward * offsetZ, transform.rotation).GetComponentInChildren<OrderSlip>();
			acquiredSlot.isTaken = true;
			acquiredSlot.refOrder = g;
			orderList.Add(g);
			g.StartOrder(o, CloseOrder);
		}
	}


	public void CloseOrder(bool success, OrderSlip slip)
	{
		orderList.Remove(slip);
		OrderManager.Instance.CompletedOrder(success, slip.OrderData);
	}

	private void GenerateSlots()
	{
		BoxCollider paperCol = order.GetComponent<BoxCollider>();
		Vector3 paper = new Vector3(paperCol.size.x * order.transform.localScale.x, paperCol.size.z * order.transform.localScale.y, 1);

		//Calculate the total dimensions
		Vector3 extent = boxCollider.bounds.extents;
		float tLength = extent.x * 2;
		float tHeight = extent.y * 2;

		// Calculate dimensions without padding
		float iLength = tLength - 2 * (padding.x);
		float iHeight = tHeight - 2 * (padding.y);

		// Calculate gaps/spacing between each order
		Vector2 gap = Vector2.zero;
		gap.x = (iLength - (rowElements * paper.x)) / (rowElements - 1);
		gap.y = (iHeight - (colElements * paper.y)) / (colElements - 1);

		// First spawn point for an order
		Vector3 spawnSlot = transform.position +
				(transform.right) * (boxCollider.bounds.extents.x - paper.x / 2 - padding.x) +
				(transform.up) * (boxCollider.bounds.extents.y - paper.y / 2 - padding.y);


		// Shift the spawn point for each subsequent order
		for (int i = 0; i < maxNumberOfOrders; i++)
		{
			int finalRowIndex = i % rowElements;
			int finalColIndex = i / rowElements;
			Vector3 v = new Vector3(-((gap.x * finalRowIndex) + (paper.x * finalRowIndex)), -((gap.y * finalColIndex) + (paper.y * finalColIndex)));
			slots[i] = new Slot(spawnSlot + v);
		}

	}

    private void GenerateSlots2()
    {
        for(int i = 0; i < maxNumberOfOrders; i ++)
        {
            slots[i] = new Slot(t[i].position);
        }
    }


	// Returns a slot on the board that is not occupied
	private Slot GetAvailableSlot()

	{
		foreach (Slot s in slots)

		{
			if (!s.isTaken)
				return s;
		}
		return null;
	}

    public void Save()
    {
        List<Order> currentListOfOrders = new List<Order>();
        
        foreach(Slot s in slots)
        {
            if(s.isTaken)
            {
                currentListOfOrders.Add(s.refOrder.OrderData);
            }
        }


        SerializeManager.Save(SerializedFileName, currentListOfOrders);
    }

    public void Load()
    {
        List<Order> tempOrderList = new List<Order>();
        object o = SerializeManager.Load(SerializedFileName);
        if (o != null)
            tempOrderList = (List<Order>)o;

        if(tempOrderList.Count > 0)
        {
            foreach(Order tempOrder in tempOrderList)
            {
                SpawnOnBoard(tempOrder);
            }
        }
    }
}