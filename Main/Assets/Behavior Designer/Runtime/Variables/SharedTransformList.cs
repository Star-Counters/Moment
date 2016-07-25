using UnityEngine;
using System.Collections.Generic;

namespace BehaviorDesigner.Runtime
{
    [System.Serializable]
    public class SharedTransformList : SharedVariable<List<Transform>>
    {
        public override string ToString() { return (mValue == null ? "null" : mValue.Count + " Transforms"); }
        public static implicit operator SharedTransformList(List<Transform> value) { return new SharedTransformList { mValue = value }; }
    }
}