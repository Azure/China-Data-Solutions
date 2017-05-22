using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DataLibrary;
using DataLibrary.Models;
using EntityFramework.BulkInsert.Extensions;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace DataPrepare
{
    public class DataPrepareWorker
    {
        public void Run()
        {
            ImportWeiboData();
            Logger.Log("Weibo Data Processed");

            ImportWeiboUser();
            Logger.Log("Weibo User Data Processed");

            ImportProbData();
            Logger.Log("Weibo Prob Data Processed");
        }

        private void ImportWeiboData()
        {
            ImportData<Weibo_detailed>(@"data\weibodata.csv");
        }

        private void ImportWeiboUser()
        {
            ImportData<Weibo_user_detailed>(@"data\weibousers.csv");
        }

        private void ImportProbData()
        {
            ImportData<Diffusion_prob>(@"data\diffu_porb.csv");
        }

        private void ImportData<T>(string file)
        {
            try
            {
                var userList = new List<T>();
                using (StreamReader sr = new StreamReader(file))
                {
                    string line = null;
                    do
                    {
                        line = sr.ReadLine();
                        if (!string.IsNullOrEmpty(line))
                        {
                            var obj = JsonConvert.DeserializeObject<T>(line);
                            userList.Add(obj);
                        }
                    }
                    while (!string.IsNullOrEmpty(line));
                }

                var batch = 500;
                var totalBatches = (userList.Count / batch) + 1;
                for (var i = 0; i < totalBatches; i++)
                {
                    var tobeinserted = userList.Skip(i * batch).Take(batch).ToList();
                    using (var context = ContextFactory.GetMediaAnalysisContext())
                    {
                        try
                        {
                            context.BulkInsert(tobeinserted);
                            context.SaveChanges();
                        }
                        catch (Exception e)
                        {
                            Logger.Log(e);
                        }

                    }
                }
            }
            catch (Exception ex)
            {
                Logger.Log(ex);
            }
        }

        private void ExportData()
        {
            using (var context = ContextFactory.GetMediaAnalysisContext())
            {
                var lines = new List<string>();
                foreach (var item in context.Diffusion_prob)
                {
                    var obj = JsonConvert.SerializeObject(item);
                    lines.Add(obj);
                }

                File.WriteAllLines("diffu_porb.csv", lines);
            }
        }
    }
}
