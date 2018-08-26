using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class OP_Ok : OptionPane 
{
    public override void SetEvent(ButtonType button, UnityAction func)
	{

        buttons[0].AddOnTriggerReleaseFunction(func);
	}
}
