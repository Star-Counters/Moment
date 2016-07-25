using UnityEngine;
using System.Collections;

namespace BehaviorDesigner.Runtime
{
    [System.Serializable]
    public class SharedVector4 : SharedVariable<Vector4>
    {
        public override string ToString() { return mValue.ToString(); }
        public static implicit operator SharedVector4(Vector4 value) { return new SharedVector4 { mValue = value }; }
    }
}