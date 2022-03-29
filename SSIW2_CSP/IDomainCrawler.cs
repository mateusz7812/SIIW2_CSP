using System.Collections.Generic;

namespace SSIW2_CSP
{
    interface IDomainCrawler<T> where T : struct
    {
        void SetNext();
        bool HasNext { get; }
        void SetReturn();
        void InitializeLabels(List<ILabel<T>> labels);
    }
}