using UnityEngine;

namespace BehaviorDesigner.Runtime.Tasks
{
	[TaskDescription("Seek the target specified using the Unity NavMesh.")]
	[TaskCategory("Oceanhorn")]
	[HelpURL("127.0.0.1/wordpress")]
	[TaskIcon("Assets/Behavior Designer Movement/Editor/Icons/{SkinColor}SeekIcon.png")]
	public class Attack : Action
	{	
		public override void OnStart()
		{
			base.OnStart();
		}
		
		// Seek the destination. Return success once the agent has reached the destination.
		// Return running if the agent hasn't reached the destination yet
		public override TaskStatus OnUpdate()
		{
			return TaskStatus.Running;
		}
		
		public override void OnReset()
		{
			base.OnReset();
		}
		public void OnAttackBegin(){
			Debug.Log ("AttackTask:OnAttackBegin");
		}
		public void OnAttackEnd(){
			Debug.Log ("AttackTask:OnAttackEnd");
		}
	}
}