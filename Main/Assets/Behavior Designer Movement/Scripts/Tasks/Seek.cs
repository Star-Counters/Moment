using UnityEngine;

namespace BehaviorDesigner.Runtime.Tasks.Movement
{
    [TaskDescription("Seek the target specified using the Unity NavMesh.")]
    [TaskCategory("Movement")]
    [HelpURL("http://www.opsive.com/assets/BehaviorDesigner/Movement/documentation.php?id=3")]
    [TaskIcon("Assets/Behavior Designer Movement/Editor/Icons/{SkinColor}SeekIcon.png")]
    public class TheirSeek : NavMeshAgentMovement
    {
        [Tooltip("The agent has arrived when the magnitude is less than this value")]
        public SharedFloat arriveDistance = 0.1f;
        [Tooltip("The transform that the agent is moving towards")]
        public SharedTransform targetTransform;
        [Tooltip("If target is null then use the target position")]
        public SharedVector3 targetPosition;

        private Vector3 prevPosition;

        public override void OnStart()
        {
            base.OnStart();

            navMeshAgent.destination = Target();
        }

        // Seek the destination. Return success once the agent has reached the destination.
        // Return running if the agent hasn't reached the destination yet
        public override TaskStatus OnUpdate()
        {
            if (!navMeshAgent.pathPending && navMeshAgent.remainingDistance < arriveDistance.Value) {
                return TaskStatus.Success;
            }

            if (prevPosition != Target()) {
                prevPosition = Target();
                navMeshAgent.destination = prevPosition;
            }

            return TaskStatus.Running;
        }

        // Return targetPosition if targetTransform is null
        private Vector3 Target()
        {
            if (targetTransform.Value != null) {
                return targetTransform.Value.position;
            }
            return targetPosition.Value;
        }

        public override void OnReset()
        {
            base.OnReset();
            arriveDistance = 0.1f;
            targetTransform = null;
            targetPosition = Vector3.zero;
        }
    }
}