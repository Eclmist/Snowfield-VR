using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class OrderSlip : MonoBehaviour {

    private string o_name;
    private int reward;
    private int duration;
    private Sprite image;

    private Text durationText;
	// Use this for initialization
	public void StartOrder(Order o)
    {
        o_name = o.Name;
        reward = o.GoldReward;
        duration = o.Duration;

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
            Debug.Log(duration);
            durationText.text = duration.ToString();
            yield return new WaitForSeconds(1);
            duration--;
            if(duration <= 0)
            {
                OrderEnd();
            }
        }
    }

    private void OrderEnd()
    {
        StopAllCoroutines();
        OrderBoard.Instance.CloseOrder();
        Destroy(gameObject);//Do we want to keep a list of the orders? Feels more clean that way
    }


}
