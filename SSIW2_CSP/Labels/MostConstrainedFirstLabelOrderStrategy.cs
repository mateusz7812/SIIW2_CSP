using System.Collections.Generic;
using System.Linq;
using SSIW2_CSP.LabelOrderStrategies;

namespace SSIW2_CSP.Labels
{
    public class MostConstrainedFirstLabelOrderStrategy<T> : ILabelOrderStrategy<T> where T:struct
    {
        public List<ILabel<T>> OrderLabels(List<ILabel<T>> labels)
        {
            return new List<ILabel<T>>(labels).OrderByDescending(label => label.Constraints.Count).ToList();
        }
    }
}