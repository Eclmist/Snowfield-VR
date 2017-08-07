using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[System.Serializable]
public struct StatText
{
    public Text health;
    public Text mana;
    public Text damage;
}

public class Stats_Menu : MonoBehaviour {
    public StatText statTxt;

	// Use this for initialization
	void Start ()
    {
		
	}
	
	// Update is called once per frame
	void Update ()
    {
        ActiveStats health = Player.Instance.StatContainer.GetStat(Stats.StatsType.HEALTH);
        ActiveStats attack = Player.Instance.StatContainer.GetStat(Stats.StatsType.ATTACK);
        if (health != null)
        {
            statTxt.health.text = health.Current + "/" + health.Max;
        }
        statTxt.mana.text = "/";

        if(attack != null)
        statTxt.damage.text = attack.ToString();
	}
}
