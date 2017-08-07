using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossFSM : MonsterFSM
{

    protected enum BossState
    {
        NORMAL = 0,
        ENRAGED = 1
    }

    [SerializeField]
    [Range(0, 1f)]
    protected float enragedHealth;
    [SerializeField]
    protected CoolDown[] attacks;

    protected bool setAttack = false;
    protected int currentAttack;

    protected BossState currentBossState = BossState.NORMAL;

    protected override void UpdateAnyState()
    {
        
        base.UpdateAnyState();
        switch (currentBossState)
        {
            case BossState.NORMAL:
                animator.speed = 1f;
                break;
            case BossState.ENRAGED:
                if(currentState == FSMState.COMBAT)
                animator.speed = 2f;
                break;
        }

        if (CurrentAI.StatContainer.GetStat(Stats.StatsType.HEALTH).Percentage > enragedHealth)
            ChangeBossState(0);
        animator.SetInteger("BossState", (int)currentBossState);
        foreach (CoolDown cd in attacks)
        {
            cd.CurrentCoolDown += Time.deltaTime;
        }
    }

    public override void DamageTaken(Actor attacker)
    {
        base.DamageTaken(attacker);
        if (currentBossState != BossState.ENRAGED && CurrentAI.StatContainer.GetStat(Stats.StatsType.HEALTH).Percentage < enragedHealth)
        {
            animator.SetTrigger("Enraged");
        }
    }

    protected void ChangeBossState(int i)
    {
        currentBossState = (BossState)i;
    }

    protected override void HandleCombatAction()
    {
        base.HandleCombatAction();
        if (!setAttack)
        {
            int random = 0;
            if (target is Actor)
            {
                List<CoolDown> availableAttack = new List<CoolDown>();
                availableAttack.AddRange(attacks);
                availableAttack.RemoveAll(CoolDownSkill => CoolDownSkill.CurrentCoolDown <= CoolDownSkill.MaxCoolDown);

                if (availableAttack.Count > 0)
                {
                    setAttack = true;
                    random = Random.Range(0, availableAttack.Count);
                    for (int i = 0; i < attacks.Length; i++)
                    {
                        if (attacks[i].Equals(availableAttack[random]))
                        {
                            currentAttack = i;
                            random = i;
                        }
                    }
                }
            }
            animator.SetInteger("AttackType", random);
        }
        
    }

    //protected void OnDrawGizmos()
    //{
    //    Collider col = GetComponent<Collider>();
    //    Gizmos.DrawSphere(transform.position + transform.up * col.bounds.extents.y, col.bounds.extents.z + attackRange);
    //}
    protected void Roar()
    {
        setAttack = false;
        ResetCD(currentAttack);
        Vector3 colBounds = CurrentAI.Collider.bounds.extents;
        colBounds.x = 0;
        TextSpawnerManager.Instance.SpawnSoundEffect("\"ニャ!\"", Color.white, transform, transform.rotation * (colBounds * 2), 3, 10);
        Collider[] collider = Physics.OverlapSphere(transform.position + transform.up * colBounds.y, colBounds.z + attackRange,aggroLayer);
        foreach (Collider col in collider)
        {
            IDamagable thisTarget = col.GetComponent<IDamagable>();
            if (thisTarget != null && !(thisTarget is Monster))
            {
                CurrentAI.Attack(null, thisTarget, .25f);
                if (thisTarget is FriendlyAI)
                {
                    FriendlyAI thisFriendlyAI = (thisTarget as FriendlyAI);
                    thisFriendlyAI.Stun();
                    Vector3 distDir = thisFriendlyAI.transform.position - transform.position;
                    Vector3 vel = distDir.normalized * (colBounds.z + attackRange - distDir.magnitude);
                    thisFriendlyAI.SetVelocity(vel * 4);
                }
            }
        }
    }

    protected void Swipe()
    {
        setAttack = false;
        ResetCD(currentAttack);
        Vector3 colBounds = CurrentAI.Collider.bounds.extents;
        colBounds.x = 0;
        Collider[] collider = Physics.OverlapSphere(transform.position +
            transform.forward * colBounds.z, colBounds.z + attackRange ,aggroLayer);
        foreach(Collider col in collider)
        {
            IDamagable thisTarget = col.GetComponent<IDamagable>();
            if (thisTarget != null && !(thisTarget is Monster))
            {
                CurrentAI.Attack(null, thisTarget,5);
            }
        }
    }

  
    protected override void CheckHit()
    {
        base.CheckHit();
        setAttack = false;
    }

    protected void ResetCD(int attackCount)
    {
        if (attackCount >= 0 && attacks.Length > attackCount)
            attacks[attackCount].CurrentCoolDown = 0;
    }

}
