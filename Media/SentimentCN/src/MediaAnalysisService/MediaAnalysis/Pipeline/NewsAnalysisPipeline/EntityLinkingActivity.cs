using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NLPLib.Entitylinking;

namespace MediaAnalysis.Pipeline.NewsAnalysisPipeline
{
    public class EntityLinkingActivity : IPipelineActivity
    {
        public ActivityResult Run(PipelineContext context)
        {
            throw new NotImplementedException();
        }

        public string Name { get; set; } = "EntityLinking";
    }
}
