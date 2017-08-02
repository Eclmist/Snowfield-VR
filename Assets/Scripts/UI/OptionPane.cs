using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;


public abstract class OptionPane : MonoBehaviour
{
	[System.Serializable]
	public struct OP_Elements
	{
		public Text title;
		public Text message;
		public Text message2;
		public Image image;
		
	}

	public enum ButtonType
	{
		Yes,
		No,
		Ok,
        Cancel,
	}

	[SerializeField]
	[Tooltip("Don't touch this unless you're sam")]
	protected OP_Elements paneElements;
    [SerializeField] protected VR_Button[] buttons;

    [SerializeField]
	protected string title;
	[SerializeField]
	[TextArea(3, 10)]
	protected string message;


    private bool alreadySetContent = false;
    private Animator anim;

    void Update()//Fully for debugging
    {
        if (Input.GetKeyDown(KeyCode.Y))
        {
            if (buttons.Length > 0 && buttons[0])
            buttons[0].SendMessage("OnTriggerRelease");
        }else if (Input.GetKeyDown(KeyCode.N))
        {
            if(buttons.Length > 1)
            {
                if (buttons[1])
                    buttons[1].SendMessage("OnTriggerRelease");
            }
        }
    }
	protected virtual void Start()
	{
		if (!alreadySetContent)
			SetContents(title, message);

        anim = GetComponent<Animator>();

        foreach (VR_Button button in buttons)
        {
            if (button)
            button.AddOnTriggerReleaseFunction(new UnityAction(ClosePane));
        }

        SetActiveButtons(0);
	}

	public virtual void SetContents(string title, string message)
	{
        if (paneElements.title)
		    paneElements.title.text = title;
        if (paneElements.message)
            paneElements.message.text = message;
		alreadySetContent = true;
	}

	public virtual void SetContents(string title, string message, string message2 = null, Sprite icon = null)
	{
		if (paneElements.title)
			paneElements.title.text = title;
		if (paneElements.message)
			paneElements.message.text = message;
		if (paneElements.message2)
			paneElements.message2.text = message2;
		if (paneElements.image)
			paneElements.image.sprite = icon;
		
		alreadySetContent = true;
	}



	public abstract void SetEvent(ButtonType button, UnityAction func);

    public virtual void SetActiveButtons(int active)
    {
        foreach (VR_Button btn in buttons)
        {
            if (btn)
            btn.interactable = active == 1 ? true : false;
        }
    }

    public virtual void ClosePane()
    {
        if (anim)
            anim.SetTrigger("Close");
        else
            Destroy();
    }

    public virtual void Destroy()
    {
        Destroy(gameObject);
    }

}
