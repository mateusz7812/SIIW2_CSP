using System.Collections.Generic;
using SSIW2_CSP.Constraints;
using SSIW2_CSP.Domains;

namespace SSIW2_CSP.Labels
{
    public class Label<T>: ILabel<T> where T: struct
    {
        public T? Value { get; set; } = null;
        public IDomain<T> Domain { get; }
        public List<T> FreeDomainValues { get; } = new ();

        public List<IConstraint> Constraints { get; } = new List<IConstraint>();
        public Label(IDomain<T> domain)
        {
            Domain = domain;
        }
        
    }
}
