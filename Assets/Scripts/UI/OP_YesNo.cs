using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

[System.Serializable]
public struct OP_Elements
{
	public VR_Button btnYes;
	public VR_Button btnNo;
	public Text title;
	public Text message;
}

public class OP_YesNo : MonoBehaviour
{
	[SerializeField] [Tooltip("Don't touch this unless you're sam")]
	private OP_Elements paneElements;

	[SerializeField]
	private string title;
	[SerializeField]
	[TextArea(3, 10)]
	private string message;


	protected void Start ()
	{
		paneElements.title.text = title;
		paneElements.message.text = message;
	}

	protected void Update ()
	{
		
	}
}
