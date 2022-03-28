using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SSIW2_CSP
{
    class SetDomain<T> : IDomain<T>, IEnumerable<T>
    {

        public IEnumerable<T> Values { get; init; } = new List<T>();

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
            ((List<T>) Values).Add(item);
        }
    }
}
