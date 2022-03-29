using System.Collections.Generic;

namespace SSIW2_CSP
{
    interface IDomain<T>
    {
        IList<T> Values { get; }
    }
}
