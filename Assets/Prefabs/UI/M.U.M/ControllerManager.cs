using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ControllerManager : MonoBehaviour
{

    public static ControllerManager Instance;
    [SerializeField]
    private VR_Controller_Custom leftController;
    [SerializeField]
    private VR_Controller_Custom rightController;


    void Awake()
    {
        Instance = this;
    }

    public VR_Controller_Custom GetController(VR_Controller_Custom.Controller_Handle handleType)
    {
        switch (handleType)
        {
            case VR_Controller_Custom.Controller_Handle.LEFT:
                return leftController;
            case VR_Controller_Custom.Controller_Handle.RIGHT:
                return rightController;
        }

        return null;
    }
}
