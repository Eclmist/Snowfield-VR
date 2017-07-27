using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InGameUIActivation : MonoBehaviour
{


    [SerializeField]
    private GameObject menu;

    [SerializeField]
    private Vector3 offset = new Vector3(0, 0, 0.1f);

    private VR_Controller_Custom currentInteractingCtrl;

    //private static GameObject InGameMenu;

    private bool visibility = false;

    // Use this for initialization
    protected void Start()
    {
        currentInteractingCtrl = GetComponent<VR_Controller_Custom>();
    }

    protected void Update()
    {
        if (currentInteractingCtrl)
        {
            if (currentInteractingCtrl.Device.GetPressDown(SteamVR_Controller.ButtonMask.ApplicationMenu))
            {
                if (visibility != true)
                {
                    visibility = true;
                    menu.SetActive(visibility);
                }

                Vector3 localOffset = Vector3.zero;
                localOffset += transform.right * offset.x;
                localOffset += transform.up * offset.y;
                localOffset += transform.forward * offset.z;

                menu.transform.position = currentInteractingCtrl.transform.position + localOffset;
                menu.transform.rotation = Quaternion.Euler(Quaternion.identity.x, currentInteractingCtrl.Device.transform.rot.eulerAngles.y, Quaternion.identity.z);
                
            }

            else if (currentInteractingCtrl.Device.GetPressDown(SteamVR_Controller.ButtonMask.Trigger))
            {
                if (visibility != false && currentInteractingCtrl.UI == null)
                {
                    visibility = false;
                    menu.SetActive(!visibility);
                }
            }

        }
    }
}
   

