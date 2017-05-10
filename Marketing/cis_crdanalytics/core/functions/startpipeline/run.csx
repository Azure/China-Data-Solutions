#load "..\CiqsHelpers\All.csx"

using System;
using System.Diagnostics;
using System.Net;
using System.Net.Http;

public static async Task<object> Run(HttpRequestMessage req, TraceWriter log)
{
    log.Info("startpipeline function begin");
    try
    {
        var parametersReader = await CiqsInputParametersReader.FromHttpRequestMessage(req);
    
        string pipelineAppUrl = parametersReader.GetParameter<string>("pipelineAppUrl"); 
		
		var requestUrl = $"{pipelineAppUrl}/pipeline/index.jsp";

        using (var client = new HttpClient())
        {
            var response = await client.GetAsync(requestUrl);

            if (!response.IsSuccessStatusCode)
            {
                log.Info(await response.Content.ReadAsStringAsync());
                log.Info(response.Headers.ToString());
            }
            else
            {
                log.Info($"Request {requestUrl} success.");
            }
        }
    }
    catch (Exception ex)
    {
        log.Info(ex.GetType().Name);
        log.Info(ex.Message);
        log.Info(ex.StackTrace);
    }
    
    log.Info("startpipeline function end");
    
    return new object();
}