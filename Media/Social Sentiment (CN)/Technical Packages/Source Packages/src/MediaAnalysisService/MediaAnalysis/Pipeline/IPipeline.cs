using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MediaAnalysis.Pipeline
{
    public interface IPipeline
    {
        string Name { get; }

        IList<IPipelineActivity> Activities { get; }

        PipelineResult Run(PipelineContext context);
    }
}
