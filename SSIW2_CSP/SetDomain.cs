using System;
using System.Collections;
using System.Collections.Generic;

namespace SSIW2_CSP
{
    class SetDomain<T> : IDomain<T>, IEnumerable<T>
    {

        public IList<T> Values { get; init; } = new List<T>();

        public IEnumerator<T> GetEnumerator()
        {
            throw new NotImplementedException();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            throw new NotImplementedException();
        }

        public void Add(T item)
        {
            Values.Add(item);
        }
    }
}
