using System.Collections.Generic;

namespace SSIW2_CSP.Domains
{
    public interface IDomain<T>
    {
        IList<T> Values { get; }
    }
}
