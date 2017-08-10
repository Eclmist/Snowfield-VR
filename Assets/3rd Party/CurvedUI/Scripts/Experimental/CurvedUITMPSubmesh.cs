using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
#if CURVEDUI_TMP
using TMPro;
#endif

namespace CurvedUI
{
    [ExecuteInEditMode]
    public class CurvedUITMPSubmesh : MonoBehaviour
    {
#if CURVEDUI_TMP

      
        VertexHelper vh;     
        Mesh savedMesh;


        public void UpdateSubmesh(bool tesselate, bool curve)
        {
            TMP_SubMeshUI TMPsub = gameObject.GetComponent<TMP_SubMeshUI>();
            if (TMPsub == null) return;

            CurvedUIVertexEffect crvdVE = gameObject.AddComponentIfMissing<CurvedUIVertexEffect>();

            if (tesselate || savedMesh == null || vh == null || (!Application.isPlaying))
            {
                vh = new VertexHelper(TMPsub.mesh);
                ModifyMesh(crvdVE);

                savedMesh = new Mesh();
                vh.FillMesh(savedMesh);
                crvdVE.TesselationRequired = true;
            }
            else if (curve)
            {
                ModifyMesh(crvdVE);
                vh.FillMesh(savedMesh);
                crvdVE.CurvingRequired = true;
            }

            TMPsub.canvasRenderer.SetMesh(savedMesh);
        }




        void ModifyMesh(CurvedUIVertexEffect crvdVE)
        {
#if UNITY_5_1
		    crvdVE.ModifyMesh(vh.GetUIVertexStream);
#else
            crvdVE.ModifyMesh(vh);
#endif
        }
#endif 
    }
}


