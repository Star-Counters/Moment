using UnityEngine;
using System.Collections;

namespace BehaviorDesigner.Runtime
{
    [System.Serializable]
    public class SharedInt : SharedVariable<int>
    {
        public override string ToString() { return mValue.ToString(); }
        public static implicit operator SharedInt(int value) { return new SharedInt { mValue = value }; }
    }
}