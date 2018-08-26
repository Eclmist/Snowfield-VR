using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;
public class OP_YesNo : OptionPane
{
    public override void SetEvent(ButtonType button, UnityAction func)
    {
        switch (button)
        {
            case ButtonType.Yes:
                buttons[0].AddOnTriggerReleaseFunction(func);
                break;
            case ButtonType.No:
                buttons[1].AddOnTriggerReleaseFunction(func);
                break;
        }
    }
}
