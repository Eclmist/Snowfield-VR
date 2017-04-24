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

        GameObject paper = transform.Find("Paper").gameObject;
        paper.transform.Find("OrderName").GetComponent<Text>().text = o_name;
        paper.transform.Find("OrderCost").GetComponent<Text>().text = reward.ToString();
        durationText = paper.transform.Find("OrderDuration").GetComponent<Text>();
        paper.transform.Find("OrderImage").GetComponent<Image>().sprite = o.Sprite;
        StartCoroutine(OrderCoroutine());
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
        if (Input.GetKeyDown(KeyCode.U))
        {
            OrderEnd(true);
        }
    }

    private void OrderEnd(bool success)
    {
        StopAllCoroutines();
        callback(success, this);
        Destroy(gameObject);//Do we want to keep a list of the orders? Feels more clean that way
    }



}
