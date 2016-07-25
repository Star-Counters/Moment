using UnityEngine;
using System.Collections;

namespace BehaviorDesigner.Runtime
{
    [System.Serializable]
    public class SharedRect : SharedVariable<Rect>
    {
        public override string ToString() { return mValue.ToString(); }
        public static implicit operator SharedRect(Rect value) { return new SharedRect { mValue = value }; }
    }
}