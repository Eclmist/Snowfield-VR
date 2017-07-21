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

    private static GameObject InGameMenu;

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
                if (InGameMenu != null)
                {
                    Destroy(InGameMenu);
                }

                Vector3 localOffset = Vector3.zero;
                localOffset += transform.right * offset.x;
                localOffset += transform.up * offset.y;
                localOffset += transform.forward * offset.z;

                InGameMenu = Instantiate(menu, currentInteractingCtrl.transform.position + localOffset,
                    Quaternion.Euler(Quaternion.identity.x, currentInteractingCtrl.Device.transform.rot.eulerAngles.y, Quaternion.identity.z));
            }

            else if (currentInteractingCtrl.Device.GetPressDown(SteamVR_Controller.ButtonMask.Trigger))
            {
                if (InGameMenu != null && currentInteractingCtrl.UI == null)
                {
                    Destroy(InGameMenu);
                }
            }

        }
    }
}
   

