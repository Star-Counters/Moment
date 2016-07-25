using UnityEngine;
public class IdleState : FSMState{
	bool needNear=false;
	bool needFar=false;
	bool needFace=false;
	public IdleState(BaseAI inNPC,bool inNeedNear,bool inNeedFar){
		npc=inNPC;
		stateID = StateID.Idle;
		needNear=inNeedNear;
		needFar=inNeedFar;
		needFace = inNPC is NPCAI;
	}
	public override void DoBeforeEntering ()
	{
		//npc.animator
	}
	public override void Reason()
	{
		if(needNear){
			if(npc.CheckPlayerNear(needFace)){
				npc.SetTransition(Transition.SawPlayerNear);
				return;
			}
		}
		if(needFar){
			if (npc.CheckPlayerFar()) {
				npc.SetTransition(Transition.SawPlayerFar);		
				return;
			}
		}
	}
	public override void Act()
	{
		
	}
}