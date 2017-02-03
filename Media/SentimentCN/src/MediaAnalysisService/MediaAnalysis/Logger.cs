using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MediaAnalysis
{
    public class Logger
    {
        public static void Log(object obj)
        {
            Console.WriteLine(obj);
        }

        public static void Log(string message, params object[] parameters)
        {
            Console.WriteLine(message, parameters);
        }
    }
}
