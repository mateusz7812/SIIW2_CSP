using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SSIW2_CSP
{
    class Label<T> where T: struct
    {
        public T? Value { get; set; } = null;
        public IDomain<T> Domain { get; }

        public Label(IDomain<T> domain)
        {
            Domain = domain;
        }
    }
}
