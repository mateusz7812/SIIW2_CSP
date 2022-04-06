using System.Collections.Generic;
using System.Linq;

namespace SSIW2_CSP.Domains
{
    class ConstDomain<T> : IDomain<T>
    {

        public IList<T> Values { get; init; }

        public ConstDomain(T value)
        {
            Values = Enumerable.Repeat(value, 1).ToList();
        }
    }
}
