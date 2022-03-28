using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SSIW2_CSP
{
    class ConstDomain<T>: IDomain<T>
    {

        public IEnumerable<T> Values { get; init; }

        public ConstDomain(T value)
        {
            Values = Enumerable.Repeat(value, 1);
        }
    }
}
