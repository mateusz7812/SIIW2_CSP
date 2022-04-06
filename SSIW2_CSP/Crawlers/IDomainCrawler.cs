using SSIW2_CSP.Constraints;
using SSIW2_CSP.Labels;

namespace SSIW2_CSP.Crawlers
{
    interface IDomainCrawler<T> where T : struct
    {
        void SetNext();
        bool HasNext { get; }
        void SetReturn();
        void Initialize();
        IConstraint Constraint { get; }
        ILabel<T> LastLabel { get; }
    }
}