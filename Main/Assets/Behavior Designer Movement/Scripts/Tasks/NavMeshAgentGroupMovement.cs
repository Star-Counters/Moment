using UnityEngine;

namespace BehaviorDesigner.Runtime.Tasks.Movement
{
    public abstract class NavMeshAgentGroupMovement : Action
    {
        [Tooltip("All of the agents")]
        public SharedGameObject[] agents = null;
        [Tooltip("The speed of the agents")]
        public SharedFloat speed = 10;
        [Tooltip("Angular speed of the agents")]
        public SharedFloat angularSpeed = 120;

        // A cache of the NavMeshAgents
        protected NavMeshAgent[] navMeshAgents;

        public override void OnStart()
        {
            navMeshAgents = new NavMeshAgent[agents.Length];
            for (int i = 0; i < agents.Length; ++i) {
                navMeshAgents[i] = agents[i].Value.GetComponent<NavMeshAgent>();
                navMeshAgents[i].enabled = true;
                navMeshAgents[i].speed = speed.Value;
                navMeshAgents[i].angularSpeed = angularSpeed.Value;
            }
        }

        public override void OnEnd()
        {
            // Disable the nav mesh
            for (int i = 0; i < navMeshAgents.Length; ++i) {
                if (navMeshAgents[i] != null) {
                    navMeshAgents[i].enabled = false;
                }
            }
        }

        // Reset the public variables
        public override void OnReset()
        {
            speed = 10;
            angularSpeed = 120;
            agents = null;
        }
    }
}