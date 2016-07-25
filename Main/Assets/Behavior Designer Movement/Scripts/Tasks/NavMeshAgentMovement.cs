using UnityEngine;

namespace BehaviorDesigner.Runtime.Tasks.Movement
{
    public abstract class NavMeshAgentMovement : Action
    {
        [Tooltip("The speed of the agent")]
        public SharedFloat speed = 10;
        [Tooltip("Angular speed of the agent")]
        public SharedFloat angularSpeed = 120;

        // A cache of the NavMeshAgent
        protected NavMeshAgent navMeshAgent;

        public override void OnAwake()
        {
            // cache for quick lookup
            navMeshAgent = gameObject.GetComponent<NavMeshAgent>();
        }

        public override void OnStart()
        {
            // set the speed and angular speed, enable the NavMeshAgent
            navMeshAgent.speed = speed.Value;
            navMeshAgent.angularSpeed = angularSpeed.Value;
            navMeshAgent.enabled = true;
        }

        public override void OnEnd()
        {
            // Disable the nav mesh
            navMeshAgent.enabled = false;
        }

        // Reset the public variables
        public override void OnReset()
        {
            speed = 10;
            angularSpeed = 120;
        }
    }
}