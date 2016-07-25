using UnityEngine;
using System.Collections;

public class TalkState : FSMState {
    int chatIndex;
	public TalkState(BaseAI inNpc){
		npc=inNpc;
		stateID=StateID.Talking;
	}
	public override void DoBeforeEntering ()
	{
        chatIndex = 0;
		CombatPanel.Instance.MainButtonDown = OnMainButtonDown;
        ChatPanel.Instance.gameObject.SetActive(true);
		ChatPanel.Instance.ShowChatBox((npc as NPCAI).chatContents[chatIndex++]);
		Messenger.Broadcast<bool>(CombatEventType.Pause,true);
		PlayerController.Instance.ChangeState(PlayerState.Talk);
	}
	public override void Act ()
	{
	}
	public override void Reason ()
	{
		//if (chatIndex < (npc as NPCAI).chatContents.Length-1)
	}
	void OnMainButtonDown(){
		if (chatIndex < (npc as NPCAI).chatContents.Length)
		{
			ChatPanel.Instance.ShowChatBox((npc as NPCAI).chatContents[chatIndex++]);
		}
		else
		{
			npc.SetTransition(Transition.LostPlayerNear);
		}
	}
	public override void DoBeforeLeaving ()
	{
		CombatPanel.Instance.MainButtonDown -= OnMainButtonDown;
		Messenger.Broadcast<bool>(CombatEventType.Pause,false);
		PlayerController.Instance.ChangeState(PlayerState.Idle);
		ChatPanel.Instance.gameObject.SetActive(false);
	}
}
