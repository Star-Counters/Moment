using UnityEngine;
using System.Collections.Generic;

namespace BehaviorDesigner.Runtime.Tasks
{
	[TaskDescription("Check to see if the player are within sight of the agent.")]
	[TaskCategory("Oceanhorn")]
	[HelpURL("127.0.0.1/wordpress")]
	[TaskIcon("Assets/Behavior Designer Movement/Editor/Icons/{SkinColor}CanSeeObjectIcon.png")]
	public class CanSeePlayerNear : Conditional
	{
		[Tooltip("The object that we are searching for. If this value is null then the objectLayerMask will be used")]
		public SharedTransform targetObject;
		[Tooltip("The distance that the agent can see ")]
		public SharedFloat viewDistance = 0.5f;
		[Tooltip("The offset relative to the pivot position")]
		public SharedVector3 offset;
		public override void OnStart ()
		{
			base.OnStart();
			if (targetObject==null) {
				targetObject = PlayerController.Instance.transform;
			}
		}
		// Returns success if an object was found otherwise failure
		public override TaskStatus OnUpdate()
		{
			if (Mathf.Abs (transform.position.y+offset.Value.y - PlayerController.Instance.transform.position.y) < 0.9f) {
				//float xDistance=Mathf.Pow((transform.position.z-PlayerController.Instance.transform.position.z),2f);
				//float zDistance=Mathf.Pow((transform.position.x-PlayerController.Instance.transform.position.x),2f);
				if(Mathf.Pow((transform.position.x+offset.Value.x-PlayerController.Instance.transform.position.x),2f)
				   +Mathf.Pow((transform.position.z+offset.Value.z-PlayerController.Instance.transform.position.z),2f)
				   <Mathf.Pow(viewDistance.Value,2f)){
					return TaskStatus.Success;
				}
			}
			return TaskStatus.Failure;
		}
		
		// Reset the public variables
		public override void OnReset()
		{
			base.OnReset();
			Debug.Log ("OnReset");
			viewDistance = 0.5f;
			offset = Vector3.zero;
		}
		// Draw the line of sight representation within the scene window
		public override void OnDrawGizmos()
		{
			UnityEditor.Handles.color = new Color(0,0.3f,0,0.1f);
			UnityEditor.Handles.DrawSolidDisc(Owner.transform.position, Vector3.up, viewDistance.Value);
		}
	}
}