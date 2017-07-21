using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class OP_YesNoCancel : OptionPane
{

    public override void SetEvent(ButtonType button, UnityAction func)
    {
        switch (button)
        {
            case ButtonType.Yes:
                if (buttons[0])
                buttons[0].AddOnTriggerReleaseFunction(func);
                break;
            case ButtonType.No:
                if (buttons[1])
                    buttons[1].AddOnTriggerReleaseFunction(func);
                break;
            case ButtonType.Cancel:
                if (buttons[2])
                    buttons[2].AddOnTriggerReleaseFunction(func);
                break;
            
        }
    }



}
