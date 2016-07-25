using UnityEngine;
using System.Collections;

namespace BehaviorDesigner.Runtime
{
    [System.Serializable]
    public class SharedGameObject : SharedVariable<GameObject>
    {
        public override string ToString() { return (mValue == null ? "null" : mValue.name); }
        public static implicit operator SharedGameObject(GameObject value) { return new SharedGameObject { mValue = value }; }
    }
}