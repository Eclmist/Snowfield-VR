using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
public class Player : Actor,ICanSerialize
{

    public float height = 1.75f;//Default hardcoded player heigh

    [SerializeField]
    public AudioClip dink;

    [SerializeField]
    private Transform vivePosition;

    public static Player Instance;
    [SerializeField]
    protected PlayerData data;

	[SerializeField]
	protected Image healthBarImage, manaBarImage;
	[SerializeField]
	protected Text healthBarText, manaBarText;
	[SerializeField]
	protected Image leftHandAttackImage, leftHandShieldImage, leftHandMagicImage;

	[SerializeField]
	protected Image rightHandAttackImage, rightHandShieldImage, rightHandMagicImage;

	[SerializeField]
	protected Text leftHandAttackText, leftHandShieldText, leftHandMagicText;

	[SerializeField]
	protected Text goldText, manaCratesText;



	protected float currentGroundHeight = 1;

    public float CurrentGroundHeight
    {
        get {
            return currentGroundHeight;
        }
        set {
            currentGroundHeight = value;
        }
    }

	protected void Update()
	{
		UpdateUI();
	}

	protected void UpdateUI()
	{
		ActiveStats health = statsContainer.GetStat(Stats.StatsType.HEALTH);
		ActiveStats mana = statsContainer.GetStat(Stats.StatsType.MANA);
		healthBarImage.fillAmount = Mathf.Lerp(healthBarImage.fillAmount,health.Percentage,Time.deltaTime);
		manaBarImage.fillAmount = Mathf.Lerp(manaBarImage.fillAmount, mana.Percentage, Time.deltaTime);

		healthBarText.text = health.Current + "/" + health.Max;
		manaBarText.text = mana.Current + "/" + mana.Max;
		
		
	}

	public override GenericItem returnSlotItem(EquipSlot.EquipmentSlotType slot)
	{
		switch (slot)
		{
			case EquipSlot.EquipmentSlotType.LEFTHAND:
				if (VR_Controller_Custom.Left.CurrentItemInHand is GenericItem)
					return VR_Controller_Custom.Left.CurrentItemInHand as GenericItem;
				break;

			case EquipSlot.EquipmentSlotType.RIGHTHAND:
				if (VR_Controller_Custom.Right.CurrentItemInHand is GenericItem)
					return VR_Controller_Custom.Right.CurrentItemInHand as GenericItem;
				break;

		}
		return null;
	}
	public string SerializedFileName
    {
        get
        {
            return "PlayerData";
        }
    }


    public override ActorData Data
    {
        get
        {
            return data;
        }

        set
        {
            data = (PlayerData)value;
        }
    }

    public int Gold
    {
        get
        {
            return data.Gold;
        }
        set
        {
            data.Gold = value;
        }
    }
    
    public override void Notify(AI ai)
    {//Unimplemented .. test code
        AudioSource ad = GetComponent<AudioSource>();
        ad.Play();
    }

   
    public override bool CheckConversingWith(Actor target)
    {
        Vector3 rotation1 = transform.forward;
        Vector3 rotation2 = target.transform.forward;
        rotation1.y = rotation2.y = 0;
       
        return (Mathf.Abs(Vector3.Angle(rotation1, rotation2) - 180) < 30) && Vector3.Distance(transform.position,target.transform.position) < 5;
    }

    protected override void Awake()
    {
        base.Awake();
        if (!Instance)
        {
            Instance = this;
            PlayerData _data = (PlayerData)SerializeManager.Load(SerializedFileName);
            if (_data != null)
            {
                data = _data;
            }
            else
            {
				data = new PlayerData(data, "Player", null);
				Job j = Data.AddJob(JobType.BLACKSMITH);
                Stats s = new Stats(Stats.StatsType.ATTACK, 2);
                j.AddStats(s);
            }
        }
        else
        {
            Debug.Log("There should only be one instanc of Player.cs in the scene!");
            Destroy(this);
        }
        float h = PlayerPrefs.GetFloat("PlayerHeight", -1);
        if (h != -1)
        {
            height = h;
        }
    }

    public virtual void CastSpell(float value, IDamagable target)
    {
        if (target != null && target.CanBeAttacked)
        {
            float randomVal = UnityEngine.Random.Range(0.8f, 1.2f);
            DealDamage(value * randomVal, target, JobType.MAGIC);
        }
    }
    public override void Die()
    {
        SteamVR_Fade.Start(Color.clear, 0);
        SteamVR_Fade.Start(Color.black, 3);
		StartCoroutine(DieRoutine());
    }

	protected IEnumerator DieRoutine()
	{
		yield return new WaitForSecondsRealtime(3);
		SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
	}

    public void Save()
    {
        SerializeManager.Save(SerializedFileName, data);
    }

    public int Tax
    {
        get
        {
            return data.Tax;
        }
        set
        {
            data.Tax = value;
        }
    }

    public void AddGold(int value)
    {
        Gold += value;
    }

    public int EXPCrates
    {
        get
        {
            return data.EXPCrates;
        }
        set
        {
            data.EXPCrates = value;
        }
    }

    //void OnDrawGizmos()
    //{
    //    Gizmos.DrawSphere(interactableArea.position, 1);
    //}

    public override Transform transform
    {
        get
        {
            return vivePosition;
        }
    }

    
}