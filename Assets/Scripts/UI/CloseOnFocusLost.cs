using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CloseOnFocusLost : MonoBehaviour
{

    private VR_Controller_Custom left;
    private VR_Controller_Custom right;

    private void Start()
    {
        left = ControllerManager.Instance.GetController(VR_Controller_Custom.Controller_Handle.LEFT);
        right = ControllerManager.Instance.GetController(VR_Controller_Custom.Controller_Handle.RIGHT);
    }

    // Update is called once per frame
    void Update () {
        Debug.Log(left.Device);
        if (left.Device.GetPressDown(SteamVR_Controller.ButtonMask.Trigger))
        {
            if (left.UI == null)
            {
                Destroy(this);
            }
        }
        else if (right.Device.GetPressDown(SteamVR_Controller.ButtonMask.Trigger))
        {
            if (right.UI == null)
            {
                Destroy(this);
            }
        }
    }
}
