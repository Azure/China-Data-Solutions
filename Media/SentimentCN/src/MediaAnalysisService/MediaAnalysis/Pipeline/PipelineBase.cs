using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MediaAnalysis.Pipeline
{
    public abstract class PipelineBase : IPipeline
    {

        public virtual string Name { get; }

        public IPipelineLog Logger { get; set; }

        public IList<IPipelineActivity> Activities { get; } = new List<IPipelineActivity>();

        public abstract void Init();

        public PipelineResult Run(PipelineContext context)
        {
            PipelineResult result = new PipelineResult();
            context.Result = result;
            context.Pipeline = this;

            try
            {
                foreach (var activity in this.Activities)
                {
                    var start = DateTime.UtcNow;
                    var aResult = activity.Run(context);
                    var end = DateTime.UtcNow;
                    aResult.StartTime = start;
                    aResult.EndTime = end;
                    aResult.TotalElapsedMillSeconds = (end - start).TotalMilliseconds;
                    result.ActivityResults[activity.Name] = aResult;
                }

                result.Succeeded = true;
            }
            catch (Exception ex)
            {
                result.Exception = ex;
            }

            return result;
        }
    }
}
