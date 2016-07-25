using UnityEngine;

namespace BehaviorDesigner.Runtime
{
    [System.Serializable]
    public class SharedMaterial : SharedVariable<Material>
    {
        public override string ToString() { return mValue.ToString(); }
        public static implicit operator SharedMaterial(Material value) { return new SharedMaterial { mValue = value }; }
    }
}