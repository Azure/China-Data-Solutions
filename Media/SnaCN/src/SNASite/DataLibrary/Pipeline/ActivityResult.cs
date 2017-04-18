using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataLibrary.Pipeline
{
    public class ActivityResult
    {
        public Type ObjectType { get; set; }

        public object Result { get; set; }

        public DateTime StartTime { get; set; }

        public DateTime EndTime { get; set; }

        public double TotalElapsedMillSeconds { get; set; }
    }
}
