using SSIW2_CSP.Labels;
using SSIW2_CSP.ValueOrderStrategies;

namespace SSIW2_CSP.ValueSetters
{
    public class OrderedValueSetter<T>: IValueSetter<T> where T:struct
    {
        public OrderedValueSetter(IValueOrderStrategy<T> valueOrderStrategy)
        {
            ValueOrderStrategy = valueOrderStrategy;
        }

        public void SetNextFreeValue(ILabel<T> label)
        {
            label.Value = ValueOrderStrategy.GetNext(label);
            label.FreeDomainValues.Remove(label.Value.Value);
        }

        public IValueOrderStrategy<T> ValueOrderStrategy { get; }
    }
}