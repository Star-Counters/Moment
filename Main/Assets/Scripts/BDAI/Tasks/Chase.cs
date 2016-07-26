using UnityEngine;

namespace BehaviorDesigner.Runtime.Tasks
{
	[TaskDescription("Seek the target specified using the Unity NavMesh.")]
	[TaskCategory("Oceanhorn")]
	[HelpURL("127.0.0.1/wordpress")]
	[TaskIcon("Assets/Behavior Designer Movement/Editor/Icons/{SkinColor}SeekIcon.png")]
	public class Chase : Action
	{
		//[Tooltip("The agent has arrived when the magnitude is less than this value")]
		//public SharedFloat arriveDistance = 0.1f;
		[Tooltip("The transform that the agent is moving towards")]
		public SharedTransform targetTransform;
        public ChaseTime chaseTimeTask;
        private Vector3 prevPosition;
		MoveUtility moveUtility ;
		
		public override void OnStart()
		{
			base.OnStart();
            chaseTimeTask.curChaseTime = 0;
            if (targetTransform.Value == null)
				targetTransform.Value = PlayerController.Instance.transform;
			moveUtility =  MoveUtility.CreateMoveUtilityInstance (transform);
			moveUtility.targetFollow = targetTransform.Value.position;
			moveUtility.FollowTarget ();
            //navMeshAgent.destination = Target();
        }

        // Seek the destination. Return success once the agent has reached the destination.
        // Return running if the agent hasn't reached the destination yet
        public override TaskStatus OnUpdate()
        {
            //if (moveUtility.isReached) {
            //	return TaskStatus.Success;
            //}
            //else {
            //这里暂时不要返回Success了。。不然整个树就完成了。
            if (prevPosition != targetTransform.Value.position)
            {
                prevPosition = targetTransform.Value.position;
                moveUtility.targetFollow = prevPosition;
                //					navMeshAgent.destination = prevPosition;
            }
            moveUtility.FollowTarget();
            chaseTimeTask.curChaseTime += Time.deltaTime;
            return TaskStatus.Running;
            //}
        }
		
		public override void OnReset()
        {
            base.OnReset();
			//arriveDistance = 0.1f;
			targetTransform = null;
        }
        public override void OnEnd()
        {
            //chaseTimeTask.curChaseTime = 3;
            base.OnEnd();
            //arriveDistance = 0.1f;
            //targetTransform = null;
        }
    }
}