using System.Collections.Generic;
using SSIW2_CSP.Labels;

namespace SSIW2_CSP.ValueOrderStrategies
{
    public interface IValueOrderStrategy<T> where T:struct
    {
        T? GetNext(ILabel<T> label);
    }
}