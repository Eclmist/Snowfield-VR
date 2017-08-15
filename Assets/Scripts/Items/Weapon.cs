using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class Weapon : Equipment
{

    [Header("Weapon Efficiency")]

    [SerializeField]
    private float chargedMultiplier, manaCost;

    [Header("Weapon Charge")]
    [SerializeField]
    [Range(0, 20)]
    private float chargeDuration = 10;
    [SerializeField]
    private AnimationCurve emissiveCurve;
    [SerializeField]
    [ColorUsageAttribute(true, true, 0f, 8f, 0.125f, 3f)]
    private Color emissiveColor;

    [SerializeField]
    [Range(0, 10)]
    private float fadeSpeed = 5;
    [SerializeField]
    private XftWeapon.XWeaponTrail trail;
	[SerializeField]
	private int materialIndex;
    
    private float timeSinceStartCharge = 0;
    private float emissiveSlider = 0;
	private ModifyRenderer modRen;

	private bool charge = false, powered = false;
    protected override void Awake()
    {
        base.Awake();
        modRen = GetComponent<ModifyRenderer>();
    }

    public bool Powered
    {
        get
        {
            return powered;
        }
    }

    public override float Damage
    {
        get
        {
            if (!powered)
                return base.Damage;
            else
                return base.Damage * chargedMultiplier;
        }
    }
    public override string Description
    {
        get
        {
            return base.Description + "Damage:" + damage;
        }
    }
    protected override void Start()
    {
        base.Start();
        modRen = GetComponent<ModifyRenderer>();

    }


    protected override void Update()
    {
        base.Update();
        if (modRen)
        {
            if (charge)
            {

                if (emissiveSlider < 1)
                {
                    emissiveSlider += fadeSpeed * Time.deltaTime;
                    if (emissiveSlider > 1)
                    {
                        powered = true;
                        emissiveSlider = 1;
                    }
                }

                timeSinceStartCharge += Time.deltaTime;
            }
            else
            {

				if (emissiveSlider > 0)
                {
                    emissiveSlider -= fadeSpeed * Time.deltaTime;
                    if (emissiveSlider < 0)
                    {
                        powered = false;
                        emissiveSlider = 0;
                    }
                }

            }

            Color targetColor = emissiveColor * emissiveCurve.Evaluate(emissiveSlider);

            if (trail != null)
                trail.MyColor = targetColor / 2;
            modRen.SetColor("_EmissionColor", targetColor, materialIndex);

            if (timeSinceStartCharge > chargeDuration + 1)
                charge = false;
        }
    }



    protected override void OnTriggerEnter(Collider collision)
    {
        base.OnTriggerEnter(collision);

        if (LinkedController != null)
        {
            IDamagable target = collision.GetComponent<IDamagable>();
            if (target != null && target is Monster)
            {
                Player.Instance.Attack(this, target);
            }
        }

    }

    public override void Unequip()
    {
        base.Unequip();
        charge = false;
    }
    //void OnDrawGizmos()
    //{
    //	Gizmos.DrawSphere(transform.position, range);
    //}

    public void StartCharge()
    {
        if (!charge)
        {
            timeSinceStartCharge = 0;
            charge = true;
        }
    }

    public void EndCharge()
    {
        if (timeSinceStartCharge > chargeDuration)
        {
            charge = false;
        }
    }

    protected override void OnGripHold()
    {
        if (!charge && Player.Instance.StatContainer.GetStat(Stats.StatsType.MANA).Current >= manaCost)
        {
            Player.Instance.StatContainer.ReduceMana(manaCost);
            StartCharge();
        }
    }

    protected override void OnGripRelease()
    {
        EndCharge();
    }
}