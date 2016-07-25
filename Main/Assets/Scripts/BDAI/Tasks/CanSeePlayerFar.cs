using UnityEngine;
using System.Collections.Generic;

namespace BehaviorDesigner.Runtime.Tasks
{
	[TaskDescription("Check to see if the player are within sight of the agent.")]
	[TaskCategory("Oceanhorn")]
	[HelpURL("127.0.0.1/wordpress")]
	[TaskIcon("Assets/Behavior Designer Movement/Editor/Icons/{SkinColor}CanSeeObjectIcon.png")]
	public class CanSeePlayerFar : Conditional
	{
		[Tooltip("The object that we are searching for. If this value is null then the objectLayerMask will be used")]
		public SharedTransform targetObject;
		[Tooltip("The distance that the agent can see ")]
		public SharedFloat viewDistance = 5;
		[Tooltip("The offset relative to the pivot position")]
		public SharedVector3 offset;
		public override void OnStart ()
		{
			base.OnStart();
			//Debug.Log ("OnStart");
			if (targetObject==null) {
				targetObject = PlayerController.Instance.transform;
			}
		}
		// Returns success if an object was found otherwise failure
		public override TaskStatus OnUpdate()
		{
			// If the target is not null then determine if that object is within sight
//			objectInSight.Value = MovementUtility.WithinSight(transform, offset.Value, fieldOfViewAngle.Value, viewDistance.Value, targetObject);
//			if (objectInSight.Value != null) {
//				// Return success if an object was found
//				return TaskStatus.Success;
//			}
//			// An object is not within sight so return failure
//			else
//				return TaskStatus.Running;

			
			if (Mathf.Abs (transform.position.y+offset.Value.y - PlayerController.Instance.transform.position.y) < 1.4f) {
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
			viewDistance = 5;
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