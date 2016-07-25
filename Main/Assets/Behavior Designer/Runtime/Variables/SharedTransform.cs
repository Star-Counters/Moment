using UnityEngine;
using System.Collections;

namespace BehaviorDesigner.Runtime
{
    [System.Serializable]
    public class SharedTransform : SharedVariable<Transform>
    {
        public override string ToString() { return (mValue == null ? "null" : mValue.name); }
        public static implicit operator SharedTransform(Transform value) { return new SharedTransform { mValue = value }; }
    }
}