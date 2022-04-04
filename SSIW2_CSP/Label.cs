using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SSIW2_CSP
{
    class Label<T>: ILabel<T> where T: struct
    {
        public T? Value { get; set; } = null;
        public IDomain<T> Domain { get; }
        public List<T> FreeDomainValues { get; init; } = new ();

        public Label(IDomain<T> domain)
        {
            Domain = domain;
        }
        
        public void SetNextFreeValue()
        {
            Value = FreeDomainValues.First();
            FreeDomainValues.Remove((T) Value);
        }

        public bool HasFreeValues => FreeDomainValues.Any();
        public int FreeDomainValuesCount => FreeDomainValues.Count;

        public void RenewFreeDomainValues()
        {
            FreeDomainValues.Clear();
            FreeDomainValues.AddRange(Domain.Values);
        }

        public List<T> RemoveFromDomain(Func<T?, bool> func)
        {
            var result = FreeDomainValues.Where(v => func(v)).ToList();
            FreeDomainValues.RemoveAll(v => func(v));
            return result;
        }
    }
}
