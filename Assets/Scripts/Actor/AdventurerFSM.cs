
using System.Collections.Generic;
using UnityEngine;

public class AdventurerFSM : FriendlyAiFSM
{

    protected Weapon currentUseWeapon;

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
        if (WaveManager.Instance.HasMonster && currentState != FSMState.COMBAT)
        {
            Monster mob = WaveManager.Instance.GetClosestMonster(transform.position);
            if (mob != null)
            {
                Vector3 useVector = transform.position;
                useVector.y = eye.position.y;
                Vector3 dir = (mob.transform.position - useVector).normalized;

                Ray ray = new Ray(useVector, dir);
                RaycastHit hit;

                //Debug.DrawRay(head.transform.position - head.right, _direction);
                if (mob.Collider.Raycast(ray, out hit, detectionDistance))
                {
                    ChangeState(FSMState.COMBAT);
                    target = mob;
                }
            }
        }
    }



    [SerializeField]
    protected float attackTimer;

    public override void ChangeState(FSMState state)
    {

        FSMState previousState = currentState;
        currentFriendlyAI.StopAllInteractions();
        if (state != FSMState.COMBAT)
            (currentFriendlyAI as AdventurerAI).UnEquipWeapons();
        base.ChangeState(state);
        if (previousState != currentState)
        {
            switch (state)
            {

                case FSMState.COMBAT:
                    (currentFriendlyAI as AdventurerAI).EquipRandomWeapons();
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

        currentUseWeapon = (Weapon)currentFriendlyAI.returnEquipment(animUseSlot);
        if ((currentUseWeapon == null || !currentUseWeapon.Powered) && attackTimer <= 0)
        {
            attackTimer = 20;
            animator.SetTrigger("Cast");
        }
    }

    public void StartCharge()
    {

        currentUseWeapon = (Weapon)currentFriendlyAI.returnEquipment(animUseSlot);

        if (currentUseWeapon != null)
        {
            currentUseWeapon.StartCharge();
        }
    }


}