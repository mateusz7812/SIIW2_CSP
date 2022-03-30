using System.Collections.Generic;

namespace SSIW2_CSP
{
    internal interface IDataLoader
    {
        void LoadData(List<ILabel<int>> labels, List<IConstraint> constraints);
    }
}