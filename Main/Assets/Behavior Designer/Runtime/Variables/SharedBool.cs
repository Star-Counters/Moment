using UnityEngine;
using System.Collections;

namespace BehaviorDesigner.Runtime
{
    [System.Serializable]
    public class SharedBool : SharedVariable<bool>
    {
        public override string ToString() { return mValue.ToString(); }
        public static implicit operator SharedBool(bool value) { return new SharedBool { mValue = value }; }
    }
}