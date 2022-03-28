using System.Collections.Generic;

namespace SSIW2_CSP
{
    interface IDomainCrawler<T> where T : struct
    {
        void SetNext(IEnumerable<Label<T>> Labels);
        bool HasNext();
        void SetReturn(List<Label<T>> labels);
    }
}