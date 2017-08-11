using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterRigDefiner : MonoBehaviour {

    [SerializeField]
    private List<Renderer> hairMaterials = new List<Renderer>();


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

   
}
