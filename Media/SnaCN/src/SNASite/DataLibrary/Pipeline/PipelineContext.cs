using System.Collections.Generic;

namespace DataLibrary.Pipeline
{
    public class PipelineContext
    {
        private readonly IDictionary<string, object> paras = new Dictionary<string, object>();
        public PipelineResult Result { get; set; }

        public IPipeline Pipeline { get; set; }

        public object this[string name]
        {
            get
            {
                if (paras.ContainsKey(name))
                    return paras[name];

                return null;
            }

            set { paras[name] = value; }
        }
    }
}