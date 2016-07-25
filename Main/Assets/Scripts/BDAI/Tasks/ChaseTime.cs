using UnityEngine;
using System.Collections.Generic;

namespace BehaviorDesigner.Runtime.Tasks
{
    [TaskDescription("Check to see how long the player are in chasing.")]
    [TaskCategory("Oceanhorn")]
    [HelpURL("127.0.0.1/wordpress")]
    [TaskIcon("Assets/Behavior Designer Movement/Editor/Icons/{SkinColor}CanSeeObjectIcon.png")]
    public class ChaseTime : Conditional
    {
        [Tooltip("The min time to chase the player.\tDon't go back and forward!!!")]
        public SharedFloat minChaseTime;
        public float curChaseTime;
        public override TaskStatus OnUpdate()
        {
            if (curChaseTime < minChaseTime.Value)
            {
                return TaskStatus.Success;
            }
            else
            {
                return TaskStatus.Failure;
            }
        }
        public override void OnEnd()
        {
            base.OnEnd();
        }
        public override void OnStart()
        {
            base.OnStart();
        }
        public override void OnAwake()
        {
            base.OnAwake();
            curChaseTime = minChaseTime.Value + float.Epsilon;
        }
    }
}
