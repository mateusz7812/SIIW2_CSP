using System.Collections.Generic;
using SSIW2_CSP.Constraints;
using SSIW2_CSP.Labels;

namespace SSIW2_CSP.DataLoaders
{
    internal interface IDataLoader
    {
        void LoadData(List<ILabel<int>> labels, List<IConstraint> constraints);
    }
}