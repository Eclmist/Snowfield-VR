using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TempGoldCounter : MonoBehaviour {

    [SerializeField]
    Text goldCountText;

    // Use this for initialization
    void Start()
    {
        if (goldCountText == null)
        {
            Destroy(this);
        }

    }

	
	// Update is called once per frame
	void Update () {

        goldCountText.text = Player.Instance.Gold.ToString() + " gold remaining";


	}
}
