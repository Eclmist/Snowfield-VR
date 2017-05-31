using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class InGameMenuButton : VR_Button {
    
    private Animator anim;
    // Use this for initialization
    void Start ()
    {
        anim = GetComponentInChildren<Animator>();
	}

    protected override void OnControllerEnter()
    {
        base.OnControllerEnter();
        anim.SetBool("CharacterBtnHover", true);
    }

    protected override void OnControllerExit()
    {
        base.OnControllerExit();
        anim.SetBool("CharacterBtnHover", false);
    }






}
