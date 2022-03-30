using System;
using System.Collections;
using System.Collections.Generic;

namespace SSIW2_CSP
{
    class SetDomain<T> : IDomain<T>, IEnumerable<T>
    {
        public SetDomain()
        {
        }

        public SetDomain(IEnumerable<T> items)
        {
            foreach(T item in items)
            {
                Add(item);
            }
        }

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
