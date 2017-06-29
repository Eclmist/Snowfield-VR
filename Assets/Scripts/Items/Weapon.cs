using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class Weapon : Equipment
{

    [SerializeField]
    protected float range;

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

    private float timeSinceStartCharge = 0;
    private float emissiveSlider = 0;
    private ModifyRenderer modRen;
    private bool charge = false;
    protected override void Awake()
    {
        base.Awake();
        modRen = GetComponent<ModifyRenderer>();
    }


	protected override void Start()
    {
		base.Start();
        modRen = GetComponent<ModifyRenderer>();

    }

    protected void Update()
    {
        if (modRen)
        {

            if (charge)
            {
                if (emissiveSlider < 1)
                {
                    emissiveSlider += fadeSpeed * Time.deltaTime;
                    if (emissiveSlider > 1)
                        emissiveSlider = 1;
                }

                timeSinceStartCharge += Time.deltaTime;
            }
            else
            {
                if (emissiveSlider > 0)
                {
                    emissiveSlider -= fadeSpeed * Time.deltaTime;
                    if (emissiveSlider < 0)
                        emissiveSlider = 0;
                }

            }

            Color targetColor = emissiveColor * emissiveCurve.Evaluate(emissiveSlider);

            if (trail != null)
                trail.MyColor = targetColor / 2;
            modRen.SetColor("_EmissionColor", targetColor);

            if (timeSinceStartCharge > chargeDuration + 1)
                charge = false;
        }
    }

    public float Range
    {
        get
        {
            return range;
        }
        set
        {
            range = value;
        }
    }

    private Action<IBlock> IfBlocked;

    public void SetBlockable(Action<IBlock> ifblocked = null)
    {
        IfBlocked = ifblocked;
    }

    protected override void UseItem()
    {
        base.UseItem();
    }

    protected override void OnTriggerEnter(Collider collision)
    {
        base.OnTriggerEnter(collision);

        if (LinkedController != null)
        {
            IDamagable target = collision.GetComponent<IDamagable>();
            Debug.Log(target);
            if (target != null)
            {
                target.TakeDamage(Damage, Player.Instance);
            }
        }
        if (IfBlocked != null)
        {
            

            IBlock item = collision.GetComponentInParent<IBlock>();
            if (item != null && item.IsBlocking)
            {
                IfBlocked(item);
            }
        }
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

    public override void OnGripHold(VR_Controller_Custom controller)
    {
        StartCharge();
    }

    public override void OnGripRelease(VR_Controller_Custom controller)
    {
        EndCharge();
    }
}