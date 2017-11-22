using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataLibrary.Pipeline
{
    public class PipelineResult
    {
        public IDictionary<string, ActivityResult> ActivityResults { get; } = new Dictionary<string, ActivityResult>();

        public Exception Exception { get; set; }

        public bool Succeeded { get; set; }
    }
}
