    using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class TempStartQuest : MonoBehaviour 
{
	public GameObject player;

	protected void Start ()
	{
		
	}
	
	protected void Update ()
	{
		if (Input.GetKeyDown(KeyCode.Q))
		{
			OptionPane options = UIManager.Instance.Instantiate
				(UIType.OP_YES_NO, "Quest", "Assign new quest to " + name, transform.position, Player.Instance.transform, transform);
			
			       // assign events
			options.SetEvent(OptionPane.ButtonType.Yes, new UnityAction(QuestActivated));

		}
	}

	public void QuestActivated()
	{
		Debug.Log("Quest Activated");
	}
}
