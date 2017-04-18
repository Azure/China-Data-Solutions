using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataLibrary.Pipeline
{
    public interface IPipelineActivity
    {

        string Name { get; set; }

        Task<ActivityResult> Run(PipelineContext context);
    }
}
