using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class PhysicalMaterial : MonoBehaviour {

    [SerializeField]private int costMultiplier;
    [SerializeField]private string m_name;
    [SerializeField]
    private enum TYPE
    {
        IRON,
        COPPER,
        STEEL
    };
    



}
