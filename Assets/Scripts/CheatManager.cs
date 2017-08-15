using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CheatManager : MonoBehaviour {

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
        if(Input.GetKeyDown(KeyCode.Alpha0)) // 100% success rate for all ingots (ONE TIME)
        {

            WeaponTierManager.Instance.SetAlwaysSuccessRate();

        }
        else if (Input.GetKeyDown(KeyCode.Alpha1)) // give full set of ingots
        {
            for (int i = 0; i < 6; i++)
                StoragePanel.Instance._Inventory.AddToInventory(ItemManager.Instance.GetItemData(i), 64);

        }
        else if (Input.GetKeyDown(KeyCode.Alpha2))  // give 10k gold
        {
            Player.Instance.AddGold(10000);
        }
        else if (Input.GetKeyDown(KeyCode.Alpha3))  //God Mode
        {
            Player.Instance.StatContainer.GetStat(Stats.StatsType.HEALTH).Max = 9999999;
            Player.Instance.StatContainer.GetStat(Stats.StatsType.HEALTH).Current = 999999;
            Player.Instance.StatContainer.GetStat(Stats.StatsType.HEALTHREGENERATION).Max = 9999999;
            Player.Instance.StatContainer.GetStat(Stats.StatsType.HEALTHREGENERATION).Current = 999999;
     
        }
        else if (Input.GetKeyDown(KeyCode.Alpha4)) // Infinite mana
        {
            Player.Instance.StatContainer.GetStat(Stats.StatsType.MANA).Max = 9999999;
            Player.Instance.StatContainer.GetStat(Stats.StatsType.MANA).Current = 999999;
            Player.Instance.StatContainer.GetStat(Stats.StatsType.MANAREGENERATION).Max = 9999999;
            Player.Instance.StatContainer.GetStat(Stats.StatsType.MANAREGENERATION).Current = 999999;
        }
        else if (Input.GetKeyDown(KeyCode.Alpha5))
        {
            
        }
        else if (Input.GetKeyDown(KeyCode.Alpha6))
        {

        }
        else if (Input.GetKeyDown(KeyCode.Alpha7))
        {

        }
        else if (Input.GetKeyDown(KeyCode.Alpha8))
        {

        }
        else if (Input.GetKeyDown(KeyCode.Alpha9))
        {

        }


    }
}
