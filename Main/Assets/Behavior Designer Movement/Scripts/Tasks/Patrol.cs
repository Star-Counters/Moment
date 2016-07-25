using UnityEngine;

namespace BehaviorDesigner.Runtime.Tasks.Movement
{
    [TaskDescription("Patrol around the specified waypoints using the Unity NavMesh.")]
    [TaskCategory("Movement")]
    [HelpURL("http://www.opsive.com/assets/BehaviorDesigner/Movement/documentation.php?id=7")]
    [TaskIcon("Assets/Behavior Designer Movement/Editor/Icons/{SkinColor}PatrolIcon.png")]
    public class TheirPatrol : NavMeshAgentMovement
    {
        [Tooltip("The agent has arrived when the square magnitude is less than this value")]
        public SharedFloat arriveDistance = 0.1f;
        [Tooltip("Should the agent patrol the waypoints randomly?")]
        public SharedBool randomPatrol = false;
        [Tooltip("The length of time that the agent should pause when arriving at a waypoint")]
        public SharedFloat waypointPauseDuration = 0;
        [Tooltip("The waypoints to move to")]
        public SharedTransformList waypoints;

        // The current index that we are heading towards within the waypoints array
        private int waypointIndex;
        private float waypointReachedTime;

        public override void OnStart()
        {
            base.OnStart();

            // initially move towards the closest waypoint
            float distance = Mathf.Infinity;
            float localDistance;
            for (int i = 0; i < waypoints.Value.Count; ++i) {
                if ((localDistance = Vector3.Magnitude(transform.position - waypoints.Value[i].position)) < distance) {
                    distance = localDistance;
                    waypointIndex = i;
                }
            }
            waypointReachedTime = -waypointPauseDuration.Value;
            navMeshAgent.destination = Target();
        }

        // Patrol around the different waypoints specified in the waypoint array. Always return a task status of running. 
        public override TaskStatus OnUpdate()
        {
            if (!navMeshAgent.pathPending) {
                var thisPosition = transform.position;
                thisPosition.y = navMeshAgent.destination.y; // ignore y
                if (Vector3.SqrMagnitude(thisPosition - navMeshAgent.destination) < arriveDistance.Value) {
                    if (waypointReachedTime == -waypointPauseDuration.Value) {
                        waypointReachedTime = Time.time;
                    }
                    // wait the required duration before switching waypoints.
                    if (waypointReachedTime + waypointPauseDuration.Value <= Time.time) {
                        if (randomPatrol.Value) {
                            waypointIndex = Random.Range(0, waypoints.Value.Count);
                        } else {
                            waypointIndex = (waypointIndex + 1) % waypoints.Value.Count;
                        }
                        navMeshAgent.destination = Target();
                        waypointReachedTime = -waypointPauseDuration.Value;
                    } 
                }
            }

            return TaskStatus.Running;
        }

        // Return the current waypoint index position
        private Vector3 Target()
        {
            return waypoints.Value[waypointIndex].position;
        }

        // Reset the public variables
        public override void OnReset()
        {
            base.OnReset();

            arriveDistance = 0.1f;
            randomPatrol = false;
            waypointPauseDuration = 0;
            waypoints = null;
        }

        // Draw a gizmo indicating a patrol 
        public override void OnDrawGizmos()
        {
#if UNITY_EDITOR
            if (waypoints == null) {
                return;
            }
            var oldColor = UnityEditor.Handles.color;
            UnityEditor.Handles.color = Color.yellow;
            for (int i = 0; i < waypoints.Value.Count; ++i) {
                if (waypoints.Value[i] != null) {
                    UnityEditor.Handles.SphereCap(0, waypoints.Value[i].position, waypoints.Value[i].rotation, 1);
                }
            }
            UnityEditor.Handles.color = oldColor;
#endif
        }
    }
}