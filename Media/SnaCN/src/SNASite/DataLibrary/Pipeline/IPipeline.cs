using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataLibrary.Pipeline
{
    public interface IPipeline
    {
        IPipelineLog Logger { get; set; }

        string Name { get; }

        IList<IPipelineActivity> Activities { get; }

        Task<PipelineResult> Run(PipelineContext context);
    }
}
