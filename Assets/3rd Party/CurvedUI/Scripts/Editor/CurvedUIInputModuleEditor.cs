using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEditor.UI;


namespace CurvedUI { 

	[CustomEditor(typeof(CurvedUIInputModule))]
	public class CurvedUIInputModuleEditor : Editor {

		public override void OnInspectorGUI()
		{
			//to be expanded at a later date.

			DrawDefaultInspector ();
		}
	}

}
