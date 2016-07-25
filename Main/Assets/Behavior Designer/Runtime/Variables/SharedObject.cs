using UnityEngine;
using System.Collections;

namespace BehaviorDesigner.Runtime
{
    [System.Serializable]
    public class SharedObject : SharedVariable<Object>
    {
        public override string ToString() { return (mValue == null ? "null" : mValue.name); }
        public static explicit operator SharedObject(Object value) { return new SharedObject { mValue = value }; }
    }
}