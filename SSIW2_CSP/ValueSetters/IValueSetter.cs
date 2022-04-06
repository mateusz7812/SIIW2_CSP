using SSIW2_CSP.Labels;

namespace SSIW2_CSP.ValueSetters
{
    public interface IValueSetter<T> where T:struct
    {
        public void SetNextFreeValue(ILabel<T> label);
    }
}