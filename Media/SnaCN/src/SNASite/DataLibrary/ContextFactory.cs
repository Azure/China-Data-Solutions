using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DataLibrary.Models;
using Microsoft.Azure;

namespace DataLibrary
{
    public class ContextFactory
    {
        public static WeiboDataContext GetMediaAnalysisContext()
        {
            var setting = CloudConfigurationManager.GetSetting("Database.ConnectionString");
            if (!string.IsNullOrEmpty(setting))
            {
                return new WeiboDataContext(setting);
            }
            else
            {
                return new WeiboDataContext();
            }
        }
    }
}
