using UnityEngine;
using System.Collections;

namespace BehaviorDesigner.Runtime
{
    [System.Serializable]
    public class SharedVector2 : SharedVariable<Vector2>
    {
        public override string ToString() { return mValue.ToString(); }
        public static implicit operator SharedVector2(Vector2 value) { return new SharedVector2 { mValue = value }; }
    }
}