using System.Collections.Generic;
using SSIW2_CSP.Labels;

namespace SSIW2_CSP.LabelOrderStrategies
{
    public interface ILabelOrderStrategy<T> where T:struct
    {
        public List<ILabel<T>> OrderLabels(List<ILabel<T>> labels);
    }
}