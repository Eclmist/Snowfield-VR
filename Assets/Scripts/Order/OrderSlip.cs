using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;

public class OrderSlip : MonoBehaviour {

    private string o_name;
    private int reward;
    private int duration;
    private Sprite image;
    private Action<bool,OrderSlip> callback;
    private Text durationText;

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
        o_name = o.Name;
        reward = o.GoldReward;
        duration = o.Duration;
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

    void Update()
    {
        //if (Input.GetKeyDown(KeyCode.U))
        //{
        //    OrderEnd(true);
        //}
    }


    private void OrderEnd(bool success)
    {
        StopAllCoroutines();
        callback(success, this);
        OrderBoard.Instance.RemoveFromBoard(this);
    }



}
