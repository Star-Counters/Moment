using UnityEngine;
using System.Collections;

namespace BehaviorDesigner.Runtime
{
    [System.Serializable]
    public class SharedColor : SharedVariable<Color>
    {
        public override string ToString() { return mValue.ToString(); }
        public static implicit operator SharedColor(Color value) { return new SharedColor { mValue = value }; }
    }
}