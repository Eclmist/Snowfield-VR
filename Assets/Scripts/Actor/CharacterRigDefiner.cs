using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterRigDefiner : MonoBehaviour {

    [SerializeField]
    private List<Renderer> hairMaterials = new List<Renderer>();
    [SerializeField]
    private List<Renderer> eyeMaterials = new List<Renderer>();


    public List<Material> HairMat
    {
        get
        {
            List<Material> matList = new List<Material>();
            foreach(Renderer ren in hairMaterials)
            {
                matList.Add(ren.material);
            }

            return matList;
        }
    }

    public List<Material> EyeMaterial
    {
        get
        {
            List<Material> matList = new List<Material>();
            foreach (Renderer ren in eyeMaterials)
            {
                matList.Add(ren.material);
            }

            return matList;
        }
    }
}
