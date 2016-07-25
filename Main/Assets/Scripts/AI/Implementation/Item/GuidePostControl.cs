using UnityEngine;
using System.Collections;
public sealed class  GuidePostControl : NPCAI {
	protected override void Awake ()
	{
		id = 701;
		base.Awake ();
	}
	protected override void Start()
	{
		base.Start ();
		MakeFSM();
	}
	protected void Update()
	{
		//base.Update ();
		//set the speechBubble face to camera.
		fsm.CurrentState.Reason();
		fsm.CurrentState.Act();
	}
	// The NPC has two states: FollowPath and ChasePlayer
	// If it's on the first state and SawPlayer transition is fired, it changes to ChasePlayer
	// If it's on ChasePlayerState and LostPlayer transition is fired, it returns to FollowPath
	private void MakeFSM()
	{
		IdleState idle=new IdleState(this,true,false);
		TalkState talk=new TalkState(this);
		DealEventState dealEvent=new DealEventState(this);
		idle.AddTransition(Transition.SawPlayerNear,StateID.DealEvent);
		talk.AddTransition(Transition.LostPlayerNear,StateID.DealEvent);
		dealEvent.AddTransition(Transition.TalkPlayer,StateID.Talking);
		dealEvent.AddTransition(Transition.LostPlayerNear,StateID.Idle);
		fsm.AddState(idle); 
		fsm.AddState(talk);
		fsm.AddState(dealEvent);
	}
	protected override void InitData ()
	{
		base.InitData ();
	}
	void OnGUI()
	{
		//GUILayout.Label (fsm.CurrentState.ToString ());
		//GUILayout.Label (fsm.CurrentState.ToString ());
	}
}
