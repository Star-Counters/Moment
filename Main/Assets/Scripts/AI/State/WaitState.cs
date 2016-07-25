using UnityEngine;
using System.Collections;

public class WaitState : FSMState {
	public WaitState(BaseAI inNpc){
		npc=inNpc;
		stateID=StateID.Wait;
	}
	public override void DoBeforeEntering ()
	{
	}
	public override void DoBeforeLeaving ()
	{
	}
	public override void Act ()
	{

	}
	public override void Reason ()
	{

	}
}
