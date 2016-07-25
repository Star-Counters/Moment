namespace BehaviorDesigner.Runtime
{
    [System.Serializable]
    public class SharedFloat : SharedVariable<float>
    {
        public override string ToString() { return Value.ToString(); }
        public static implicit operator SharedFloat(float value) { return new SharedFloat { Value = value }; }
    }
}