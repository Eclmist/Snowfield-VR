using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;

public class OrderSlip : VR_Interactable_UI
{

	private string o_name;
	private int reward;
	private int duration;
	private Sprite image;
	private Action<bool, OrderSlip> callback;
	private Text durationText;
	private Order order;
	private AdventurerAI ai;
	[SerializeField] GameObject detailPane;
	[SerializeField] AudioClip orderCompleteSound;
	[SerializeField] AudioClip orderFadeInSound;
	[SerializeField] AudioClip wrongOrderSound;
	OptionPane currentOP;

	public AdventurerAI OrderedAI
	{
		get { return this.ai; }
		set { this.ai = value; }
	}

	GameObject slip;

	public int Reward
	{
		get
		{
			return reward;
		}
	}
	// Use this for initialization
	public void StartOrder(Order o, Action<bool, OrderSlip> _callback)
	{
		order = o;

		o_name = order.Name;
		reward = order.GoldReward;
		duration = order.Duration;
		callback = _callback;

		slip = transform.Find("Slip").gameObject;
		slip.transform.Find("OrderName").GetComponent<Text>().text = o_name;
		slip.transform.Find("OrderCost").GetComponent<Text>().text = reward.ToString();
		durationText = slip.transform.Find("OrderDuration").GetComponent<Text>();
		slip.transform.Find("OrderImage").GetComponent<Image>().sprite = o.Sprite;
		slip.gameObject.SetActive(false);
	}

	protected override void Start()
	{
		base.Start();

		if (orderFadeInSound)
			AudioSource.PlayClipAtPoint(orderFadeInSound, transform.position);

        order.EndTime = Time.realtimeSinceStartup + duration;
	}

    protected override void Update()
    {
        base.Update();
        durationText.text = duration.ToString();
        duration = (int)order.EndTime - (int)Time.realtimeSinceStartup;
        if(duration<=0)
        {
            OrderEnd(false);
        }

    }


    public void ShowOrderInformation()
	{
		slip.gameObject.SetActive(true);
	}



	private void OrderEnd(bool success)
	{
		StopAllCoroutines();
		callback(success, this);
		OrderBoard.Instance.RemoveFromBoard(this);
	}

	Weapon interactingWeapon;

	private void DisplayOptions()
	{
		CloseCurrentPane();

		OptionPane op = UIManager.Instance.InstantiateOptions(transform.position, Player.Instance.transform, transform);

		currentOP = op;
		op.transform.LookAt(Player.Instance.transform);
		if (currentInteractingController.CurrentItemInHand is Weapon)
			interactingWeapon = currentInteractingController.CurrentItemInHand as Weapon;



		op.SetEvent(OptionPane.ButtonType.Yes, TryConfirmOrder);
		op.SetEvent(OptionPane.ButtonType.No, SpawnDetailsPanel);
		op.SetEvent(OptionPane.ButtonType.Cancel, CloseCurrentPane);

	}

	private void CloseCurrentPane()
	{
		if (currentOP)
		{
			currentOP.Destroy();
		}
	}

	private void TryConfirmOrder()
	{

		bool isCorrect = false;


		if (interactingWeapon)
		{
			if (interactingWeapon.ItemID == order.ItemID)
			{
				isCorrect = true;
			}
			else // give the player lesser reward if material type is the same
			{
				ItemData tempData = ItemManager.Instance.GetItemData(interactingWeapon.ItemID);
				CraftedItem tempCraftedItem = tempData.ObjectReference.GetComponent<CraftedItem>();

				if (tempCraftedItem)
				{

					PhysicalMaterial.Type tempType = tempCraftedItem.GetPhysicalMaterial();

					if (order.MaterialType == tempType)
					{

						int reduction = WeaponTierManager.Instance.GetNumberOfTiersInClass(tempType);
						reward /= reduction;
						isCorrect = true;

					}

				}
			}

		}



		if (isCorrect)
		{

			OrderEnd(true);

			CloseCurrentPane();

			GameManager.Instance.AddPlayerGold(reward);
			TextSpawnerManager.Instance.SpawnText("+" + reward, Color.green, transform);

			if (orderCompleteSound)
				AudioSource.PlayClipAtPoint(orderCompleteSound, transform.position);



			interactingWeapon.LinkedController.SetModelActive(true);
			Destroy(interactingWeapon.gameObject);
			currentInteractingController = null;
		}
		else
		{
			TextSpawnerManager.Instance.SpawnText("Try again!", Color.red, transform);
			if (wrongOrderSound)
				AudioSource.PlayClipAtPoint(wrongOrderSound, transform.position);
		}



	}

	private void SpawnDetailsPanel()
	{

		CloseCurrentPane();

		string desc = "Name: " + o_name + "\n\n" + "Material: " + order.MaterialType;

		OptionPane op = UIManager.Instance.InstantiateDetailPane(detailPane, order.Sprite, desc, reward.ToString(), transform.position, Player.Instance.transform, transform);
		op.transform.LookAt(Player.Instance.transform);
		op.SetEvent(OptionPane.ButtonType.Ok, CloseCurrentPane);


		currentOP = op;
	}



	protected override void OnTriggerRelease()
	{
		base.OnTriggerRelease();

		if (currentInteractingController.UI == this)
		{
			DisplayOptions();
		}

	}


	protected override void OnControllerEnter()
	{
		base.OnControllerEnter();

	}

	protected override void OnControllerExit()
	{
		base.OnControllerExit();
	}







}
