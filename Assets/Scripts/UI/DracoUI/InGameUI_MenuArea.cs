using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InGameUI_MenuArea : VR_Button
{
    public static InGameUI_MenuArea Instance;
    [SerializeField] private GameObject menu;
    private Vector3 offset;
    protected void Awake()
    {
        if (Instance == null)
            Instance = this;
        else
            Destroy(this);
    }
    // Use this for initialization
    protected void Start()
    {
        offset = new Vector3(0, 0, 0.1f);
    }

    protected override void OnApplicationMenuPress()
    {
        base.OnApplicationMenuPress();
        if (InGameUI.Instance.GetLastState() != InGameState.PAUSE)
        {
            Instantiate(menu, currentInteractingController.transform.position + offset, Quaternion.Euler(Quaternion.identity.x, currentInteractingController.Device.transform.rot.eulerAngles.y , Quaternion.identity.z));
            InGameUI.Instance.SetGameState(InGameState.PAUSE);
            
        }
    }

    protected override void OnTriggerHold()
    {
        base.OnTriggerPress();
    }







}


