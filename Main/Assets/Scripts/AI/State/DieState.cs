using UnityEngine;
using System.Collections;

public class DieState : FSMState {
	public DieState(BaseAI inNpc){
		npc=inNpc;
		stateID=StateID.Die;
	}
	public override void DoBeforeEntering ()
	{
		DataManager.Instance.RemoveAIListItem(npc as MonsterAI);
		UnityEngine.Object.Destroy(npc.gameObject);
	}
	public override void Act ()
	{
	}
	public override void Reason ()
	{
	}
	public override void DoBeforeLeaving ()
	{
	}
}