using System.Collections.Generic;

namespace SSIW2_CSP
{
    public interface IDomain<T>
    {
        IList<T> Values { get; }
    }
}
