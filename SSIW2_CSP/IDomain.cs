using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SSIW2_CSP
{
    interface IDomain<T>
    {
        IEnumerable<T> Values { get; }
    }
}
