using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TargetRevenue : MonoBehaviour
{

    [SerializeField]
    private Text revenueText;
	
	// Update is called once per frame
	void Update ()
    {
        //revenueText.text = "Upcoming tax:" + GameManager.Instance.Tax + "\n" + "Gold after deduction:" + (Player.Instance.Gold - GameManager.Instance.Tax);
	}
}
