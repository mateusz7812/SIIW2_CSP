using System.Linq;
using SSIW2_CSP.Labels;

namespace SSIW2_CSP.ValueSetters
{
    public class DefaultValueSetter<T>: IValueSetter<T> where T:struct
    {
        public void SetNextFreeValue(ILabel<T> label)
        {
            label.Value = label.FreeDomainValues.First();
            label.FreeDomainValues.Remove((T) label.Value);
        }
    }
}