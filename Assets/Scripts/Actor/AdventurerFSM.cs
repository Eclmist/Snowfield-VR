
using System.Collections.Generic;
using UnityEngine;

public class AdventurerFSM : FriendlyAIFSM
{

	protected Weapon currentUseWeapon;

	public virtual void EndAttack()
	{
		if (currentUseWeapon != null)
		{
			currentUseWeapon.EndCharge();
		}
	}



	protected override bool CheckForTargets()
	{
		bool hasTarget = base.CheckForTargets();
		if (!hasTarget && WaveManager.Instance.HasMonster)
		{
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
						target = mob;
						hasTarget = true;
					}
				}
			}

		}
		return hasTarget;
	}


	[SerializeField]
	protected float attackTimer;

	public override void ChangeState(FSMState state)
	{

		currentFriendlyAI.StopAllInteractions();
		FSMState previousState = currentState;
		base.ChangeState(state);
		if (previousState != currentState)
		{
			if (currentState == FSMState.COMBAT)
			{
				(currentFriendlyAI as AdventurerAI).EquipStrongestWeapon();
				attackTimer = 20;
			}
			else
			{
				(currentFriendlyAI as AdventurerAI).DestroyHandsWeapon();
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