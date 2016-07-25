using UnityEngine;
public class AttackState : FSMState{
	bool isAttacking=false;
	float attackTime=0;
	public AttackState(BaseAI inNPC){
		npc=inNPC;
		stateID = StateID.AttackPlayer;
	}
	public override void DoBeforeEntering ()
	{
		attackTime=(npc as MonsterAI).monsterData.attackDelayTime;
		//npc.animator.SetBool ("Attack", true);
	}
	public override void Reason()
	{
		if (!npc.CheckPlayerNear()) {
			npc.SetTransition(Transition.LostPlayerNear);
		}
	}
	public override void Act()
	{
		if(attackTime<(npc as MonsterAI).monsterData.attackDelayTime){
			attackTime+=Time.deltaTime;
			Quaternion q=Quaternion.LookRotation(PlayerController.Instance.transform.position-npc.transform.position,Vector3.up);
			npc.transform.rotation=Quaternion.Euler(new Vector3(0,q.eulerAngles.y,0));
		}
		else{
			attackTime=0f;
			npc.animator.SetBool ("Attack", true);
		}
	}
	public override void DoBeforeLeaving ()
	{
		npc.animator.SetBool ("Attack", false);
		//npc.animator.SetBool ("Run", true);
	}
}