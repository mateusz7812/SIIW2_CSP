using System.Collections.Generic;
using SSIW2_CSP.Constraints;
using SSIW2_CSP.Domains;

namespace SSIW2_CSP.Labels
{
    public interface ILabel<T> where T : struct
    {
        public T? Value { get; set; }
        public IDomain<T> Domain { get; }
        List<T> FreeDomainValues { get; }
        public List<IConstraint> Constraints { get; }
    }
}
