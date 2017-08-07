using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SpawnedText : MonoBehaviour {

    [SerializeField]
    private Text text;
    [SerializeField]
    private Animator anim;


    public void SetText(string s)
    {
        text.text = s;
    }

    public void SetColor(Color c)
    {
        text.color = c;
    }
}
