using System;
using System.Collections.Generic;
using System.Linq;
using SSIW2_CSP.Controllers;
using SSIW2_CSP.Crawlers;
using SSIW2_CSP.DataLoaders;
using SSIW2_CSP.LabelOrderStrategies;
using SSIW2_CSP.Labels;
using SSIW2_CSP.ValueOrderStrategies;
using SSIW2_CSP.ValueSetters;

namespace SSIW2_CSP
{
    class Program
    {

        static void Main(string [] args)
        {
            Problem<int> problem = new Problem<int>(
                6, 
                ProblemType.Futoshiki, 
                CrawlerType.ForwardChecking, 
                LabelOrderStrategyType.MostConstrainedFirst, 
                ValueOrderStrategyType.Random,
                false);
            var controller = ControllerBuilder.Build(problem);
            controller.FindSolutions();
            controller.PrintResult();
        }
    }
}
