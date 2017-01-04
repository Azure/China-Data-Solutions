// ***********************************************************************
// Assembly         : MediaMonitoring
// Author           : 
// Created          : 07-25-2016
//
// Last Modified By : 
// Last Modified On : 07-25-2016
// ***********************************************************************
// <copyright file="RequestHelper.cs" company="">
//     Copyright ©  2016
// </copyright>
// <summary></summary>
// ***********************************************************************
namespace MediaMonitoring.Utility
{
    using System;
    using System.Net.Http;

    /// <summary>
    /// Class RequestExtension.
    /// </summary>
    public static class RequestExtension
    {
        /// <summary>
        /// Gets the request parameter value.
        /// </summary>
        /// <param name="request">The request.</param>
        /// <param name="paraName">Name of the para.</param>
        /// <returns>System.String.</returns>
        public static string GetRequestParamValue(this HttpRequestMessage request, string paraName)
        {
            if (request != null)
            {
                var paras = request.GetQueryNameValuePairs();
                foreach (var item in paras)
                {
                    if (string.Compare(paraName, item.Key, StringComparison.OrdinalIgnoreCase) == 0)
                    {
                        return item.Value;
                    }
                }
            }

            return null;
        }
    }
}