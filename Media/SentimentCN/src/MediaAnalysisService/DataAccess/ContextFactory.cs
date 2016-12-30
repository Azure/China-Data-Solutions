using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DataAccess.Models;
using Microsoft.Azure;

namespace DataAccess
{
    public class ContextFactory
    {
        public static MediaAnalysisContext GetMediaAnalysisContext()
        {
            var setting = CloudConfigurationManager.GetSetting("Database.ConnectionString");
            if (!string.IsNullOrEmpty(setting))
            {
                return new MediaAnalysisContext(setting);
            }
            else
            {
                return new MediaAnalysisContext();
            }
        }
    }
}
