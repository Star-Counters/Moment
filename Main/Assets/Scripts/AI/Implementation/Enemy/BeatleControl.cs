using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine;
public sealed class BeatleControl : MonsterAI
{
	public Transform[] paths;
	protected override void Awake ()
	{
		base.Awake ();
		id = 101;
//		Debug.Log ("Beatle AWAKE");
	}
	protected override void Start()
	{
		base.Start ();
		MakeFSM();
	}
	protected void Update()
	{
		fsm.CurrentState.Reason();
		fsm.CurrentState.Act();
	}
	// The NPC has two states: FollowPath and ChasePlayer
	// If it's on the first state and SawPlayer transition is fired, it changes to ChasePlayer
	// If it's on ChasePlayerState and LostPlayer transition is fired, it returns to FollowPath
	private void MakeFSM()
	{
		FollowTargetState follow = new FollowTargetState(this,paths);
		follow.AddTransition(Transition.LostPlayerFar, StateID.Idle);
		follow.AddTransition(Transition.SawPlayerNear, StateID.AttackPlayer);
		//ChasePlayerState chase = new ChasePlayerState(this);
		//chase.AddTransition(Transition.LostPlayer, StateID.FollowingPath);
		IdleState idle=new IdleState(this,true,true);
		idle.AddTransition(Transition.SawPlayerFar,StateID.FollowingPath);
		idle.AddTransition (Transition.SawPlayerNear, StateID.AttackPlayer);
		AttackState attack = new AttackState (this);
		attack.AddTransition (Transition.LostPlayerNear, StateID.FollowingPath);
		fsm.AddState(follow);
		fsm.AddState(idle);
		fsm.AddState(attack);
	}
//	void OnGUI(){
//		GUILayout.Label(fsm.CurrentState.ToString());
//	}
	protected override void InitData ()
	{
		base.InitData ();
	}
	void OnGUI()
	{
		GUILayout.Label (fsm.CurrentState.ToString ());
	}
}