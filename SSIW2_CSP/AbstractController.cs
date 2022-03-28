using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SSIW2_CSP
{
    abstract class AbstractController<T> : IController<T> where T : struct
    {
        private IDomainCrawler<T> crawler { get; set; }
        private List<Label<T> []> Solutions { get; set; }
        private List<Label<T>> Labels { get; set; }
        private IEnumerable<IConstraint> Constraints { get; set; }
        public Label<T> [] [] GetSolutions(IProblem<T> problem)
        {
            while (crawler.HasNext())
            {
                crawler.SetNext(Labels);
                crawler.SetReturn(Labels);
                if (false)
                {
                    //Solutions.Add();
                }
            }
            return null;
        }

        
    }
}
