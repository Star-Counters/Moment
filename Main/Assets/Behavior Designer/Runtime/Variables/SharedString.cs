using UnityEngine;
using System.Collections;

namespace BehaviorDesigner.Runtime
{
    [System.Serializable]
    public class SharedString : SharedVariable<string>
    {
        public override string ToString() { return string.IsNullOrEmpty(mValue) ? "" : mValue.ToString(); }
        public static implicit operator SharedString(string value) { return new SharedString { mValue = value }; }
    }
}