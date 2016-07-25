using UnityEngine;

namespace BehaviorDesigner.Runtime
{
    [System.Serializable]
    public class GenericVariable
    {
        [SerializeField]
        public string type = "SharedString";
        [SerializeField]
        public SharedVariable value;
    }

    [System.Serializable]
    public class SharedGenericVariable : SharedVariable<GenericVariable>
    {
        public override string ToString() { return mValue == null ? "null" : mValue.ToString(); }
        public static implicit operator SharedGenericVariable(GenericVariable value) { return new SharedGenericVariable { mValue = value }; }
    }
}