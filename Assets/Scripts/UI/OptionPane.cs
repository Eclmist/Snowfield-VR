using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;

public enum UIType
{
	OP_YES_NO,
	OP_OK
}


public abstract class OptionPane : MonoBehaviour
{
	[System.Serializable]
	public struct OP_Elements
	{
		public Text title;
		public Text message;
	}

	public enum ButtonType
	{
		Yes,
		No,
		Ok
	}

	[SerializeField]
	[Tooltip("Don't touch this unless you're sam")]
	protected OP_Elements paneElements;

	[SerializeField]
	protected string title;
	[SerializeField]
	[TextArea(3, 10)]
	protected string message;

	private bool alreadySetContent = false;

	protected virtual void Start()
	{
		if (!alreadySetContent)
			SetContents(title, message);

		SetActiveButtons(0);
	}

	public void SetContents(string title, string message)
	{
		paneElements.title.text = title;
		paneElements.message.text = message;
		alreadySetContent = true;
	}

	public abstract void SetEvent(ButtonType button, UnityAction func);

	public abstract void SetActiveButtons(int active);

}
