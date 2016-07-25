using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

public class DealEventState : FSMState {
	bool needFace=true;
	public DealEventState(BaseAI inNpc){
		npc=inNpc;
		stateID=StateID.DealEvent;
	}
	public override void DoBeforeEntering ()
	{
		Debug.Log("Deal Event");
		CombatPanel.Instance.MainButtonDown = OnMainButtonDown;
		(npc as NPCAI).StopTalkBubble ();
	}
	public override void Act ()
	{

	}
	public override void Reason ()
	{
		if (!npc.CheckPlayerNear (needFace)) {
			npc.SetTransition(Transition.LostPlayerNear);
		}
	}
	void OnMainButtonDown(){
		npc.SetTransition(Transition.TalkPlayer);
	}
	public override void DoBeforeLeaving ()
	{
		CombatPanel.Instance.MainButtonDown-= OnMainButtonDown;
		CombatPanel.Instance.MainButtonDown+= PlayerController.Instance.OnMainButtonDown;
		(npc as NPCAI).PlayTalkBubble ();
	}
}
