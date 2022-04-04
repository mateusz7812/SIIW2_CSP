using System;
using System.Collections.Generic;

namespace SSIW2_CSP
{
    public interface ILabel<T> where T : struct
    {
        public T? Value { get; set; }
        public IDomain<T> Domain { get; }
        public void SetNextFreeValue();
        public bool HasFreeValues { get; }
        List<T> FreeDomainValues { get; }
        int FreeDomainValuesCount { get; }
        public void RenewFreeDomainValues();

        List<T> RemoveFromDomain(Func<T?, bool> func);
    }
}
