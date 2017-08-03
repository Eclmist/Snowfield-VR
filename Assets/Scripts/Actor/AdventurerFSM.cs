
using System.Collections.Generic;
using UnityEngine;

public class AdventurerFSM : FriendlyAIFSM
{

    protected Weapon currentUseWeapon;

    public override void NewSpawn()
    {
        base.NewSpawn();
        visitedShop.Clear();
    }

    protected override void Awake()
    {
        base.Awake();
        currentAdventurerAI = GetComponent<AdventurerAI>();
    }
 


    public virtual void EndAttack()
    {
        if (currentUseWeapon != null)
        {
            currentUseWeapon.EndCharge();
        }
    }

    

    protected override void UpdateAnyState()
    {
        base.UpdateAnyState();
        if (WaveManager.Instance.HasMonster)
        {
            Monster mob = WaveManager.Instance.GetClosestMonster(transform.position);
            if (mob != null && Vector3.Distance(transform.position, mob.transform.position) < detectionDistance)
            {
                ChangeState(FSMState.COMBAT);
                target = mob;
            }
        }
    }


   
    [SerializeField]
    protected float attackTimer;

    public override void ChangeState(FSMState state)
    {

        FSMState previousState = currentState;
        currentAdventurerAI.StopAllInteractions();
        if (state != FSMState.COMBAT)
            currentAdventurerAI.UnEquipWeapons();
        base.ChangeState(state);
        if (previousState != currentState)
        {
            switch (state)
            {
                
                case FSMState.COMBAT:
                    currentAdventurerAI.EquipRandomWeapons();
                    attackTimer = 20;
                    //animator.SetBool("KnockBack", true);//Wrong place
                    //timer = 5;
                    return;

            }
        }
    }

   
    protected override void UpdateCombatState()
    {
        attackTimer -= Time.deltaTime;
        if (attackTimer < 0)
            attackTimer = 0;
        
        base.UpdateCombatState();
    }

    protected override void HandleCombatAction()
    {
        if (animator.GetCurrentAnimatorStateInfo(0).IsName("Cast"))
            animator.SetBool("Attacking", false);
        else
            animator.SetBool("Attacking", true);
        currentUseWeapon = (Weapon)currentAdventurerAI.returnEquipment(animUseSlot);
        if ((currentUseWeapon == null || !currentUseWeapon.Powered) && attackTimer <= 0)
        {
            attackTimer = 20;
            animator.SetTrigger("Cast");
        }
    }

    public void StartCharge()
    {

        currentUseWeapon = (Weapon)currentAdventurerAI.returnEquipment(animUseSlot);

        if (currentUseWeapon != null)
        {
            currentUseWeapon.StartCharge();
        }
    }

  
}