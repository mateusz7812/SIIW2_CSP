using System.Collections.Generic;
using System.Linq;
using SSIW2_CSP.Labels;

namespace SSIW2_CSP.ValueOrderStrategies
{
    public class DomainValueOrderStrategy<T>: IValueOrderStrategy<T> where T:struct
    {
        public T? GetNext(ILabel<T> label)
        {
            return label.Domain.Values.First(val => label.FreeDomainValues.Contains(val));
        }
    }
}