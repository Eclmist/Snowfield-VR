using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;

public class OrderSlip : VR_Interactable_UI {

    private string o_name;
    private int reward;
    private int duration;
    private Sprite image;
    private Action<bool,OrderSlip> callback;
    private Text durationText;
    private int compareID;
    private Order order;
    private AdventurerAI ai;

    public AdventurerAI OrderedAI
    {
        get { return this.ai; }
        set { this.ai = value; }
    }

    GameObject slip;

    public int Reward
    {
        get
        {
            return reward;
        }
    }
	// Use this for initialization
	public void StartOrder(Order o,Action<bool,OrderSlip> _callback)
    {
        order = o;

        o_name = order.Name;
        reward = order.GoldReward;
        duration = order.Duration;
        callback = _callback;

        slip = transform.Find("Slip").gameObject;
        slip.transform.Find("OrderName").GetComponent<Text>().text = o_name;
        slip.transform.Find("OrderCost").GetComponent<Text>().text = reward.ToString();
        durationText = slip.transform.Find("OrderDuration").GetComponent<Text>();
        slip.transform.Find("OrderImage").GetComponent<Image>().sprite = o.Sprite;
        slip.gameObject.SetActive(false);
        StartCoroutine(OrderCoroutine());
    }

    public void ShowOrderInformation()
    {
        slip.gameObject.SetActive(true);
    }

    

    private IEnumerator OrderCoroutine()
    {
        while (true)
        {   
            durationText.text = duration.ToString();
            yield return new WaitForSeconds(1);
            duration--;
            if(duration <= 0)
            {
                OrderEnd(false);
            }
        }
    }



    private void OrderEnd(bool success)
    {
        StopAllCoroutines();
        callback(success, this);
        OrderBoard.Instance.RemoveFromBoard(this);
    }

    protected override void OnTriggerPress()
    {
        if (currentInteractingController.UI == this)
        {

            Weapon weapon = currentInteractingController.GetComponentInChildren<Weapon>();

            if (weapon)
            {
                if (weapon.ItemID == order.ItemID)
                {
                    OrderEnd(true);
                    Destroy(weapon.gameObject);
                    currentInteractingController.Model.SetActive(true);
                    Debug.Log("correcy");
                }
                    
            }
            else
            {
                Debug.Log("wrong?");
            }


        }

    }

    


    protected override void OnControllerEnter()
    {
        base.OnControllerEnter();
        if (currentInteractingController)
        {
            currentInteractingController.UI = this;
            Debug.Log("interacring with an order");
        }

    }

    protected override void OnControllerExit()
    {
        if (currentInteractingController.UI == this)
            currentInteractingController.UI = null;
        base.OnControllerExit();
    }







}
