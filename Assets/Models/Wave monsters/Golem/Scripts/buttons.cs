using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class buttons : MonoBehaviour {

	public GameObject go;		
	
	private List<string> AniList  = new List<string>();
	private int count;
	
	
	void Start() {
	
		foreach (AnimationState state in go.GetComponentInChildren<Animation>()) {
			AniList.Add(state.name); }

        //count = AniList.Count;
        count = 18;
    }
	
	
 	void OnGUI()		
	{
		for(int i = 0; i < 10; i++) {
            if (GUI.Button(new Rect(20+i*110, 20, 100, 60), AniList[i])){
		 		go.GetComponent<Animation>().wrapMode= WrapMode.Loop;
		  		go.GetComponent<Animation>().CrossFade(AniList[i]);
	  		}
        }
        for (int i = 10; i < count; i++)
        {
            if (GUI.Button(new Rect(20 + (i-10) * 110, 120, 100, 60), AniList[i]))
            {
                go.GetComponent<Animation>().wrapMode = WrapMode.Loop;
                go.GetComponent<Animation>().CrossFade(AniList[i]);
            }
        }
    }
	
}
