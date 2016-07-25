using UnityEngine;

namespace BehaviorDesigner.Runtime.Tasks.Movement
{
	[TaskDescription("Set The Value of The Animator Attached To The GameObject.")]
	[TaskCategory("Oceanhorn")]
	[HelpURL("127.0.0.1/wordpress")]
	[TaskIcon("Assets/Behavior Designer Movement/Editor/Icons/{SkinColor}SeekIcon.png")]
	public class AnimatorSetValue : Action
	{
		Animator animator;
		public SharedString parameterName;
		public SharedVariable parameterValue;
		public override void OnStart()
		{
			base.OnStart();
			animator = GetComponent<Animator> ();
			if (parameterValue == null)
				return;
			//Debug.Log (parameterValue.GetValue ()+","+parameterValue.GetValue ().GetType().Name);
			string parameterTypeName = parameterValue.GetValue ().GetType ().FullName;
			switch (parameterTypeName) {
			case "System.Int32":
				animator.SetInteger (parameterName.Value, (int)parameterValue.GetValue ());
				break;
			case "System.Boolean":
				animator.SetBool(parameterName.Value, (bool)parameterValue.GetValue ());
				break;
			case "System.Single":
				animator.SetFloat(parameterName.Value, (float)parameterValue.GetValue ());
				break;
			default:
				break;
			}
		}
		public override TaskStatus OnUpdate()
		{
			return TaskStatus.Success;
		}
		public override void OnReset()
		{
			base.OnReset();
		}
	}
}