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
        statTxt.health.text = Player.Instance.Health + "/" + Player.Instance.MaxHealth;
        statTxt.mana.text = "/";
        statTxt.damage.text = Player.Instance.AttackDamage.ToString();
	}
}
