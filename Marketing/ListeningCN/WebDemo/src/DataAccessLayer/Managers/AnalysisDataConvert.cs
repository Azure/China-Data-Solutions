// ***********************************************************************
// Assembly         : DataAccessLayer
// Author           : 
// Created          : 07-25-2016
//
// Last Modified By : 
// Last Modified On : 08-26-2016
// ***********************************************************************
// <copyright file="AnalysisDataConvert.cs" company="">
//     Copyright ©  2016
// </copyright>
// <summary></summary>
// ***********************************************************************
namespace DataAccessLayer.Managers
{
    using System;

    using DataAccessLayer.DataModels;

    using Newtonsoft.Json;

    /// <summary>
    /// Class AnalysisDataConvert.
    /// </summary>
    public class AnalysisDataConvert
    {
        /// <summary>
        /// The to ca data.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="type">The type.</param>
        /// <param name="date">The date.</param>
        /// <param name="obj">The obj.</param>
        /// <returns>The <see cref="CAData" />.</returns>
        public static CAData ToCAData<T>(string type, DateTime date, T obj)
        {
            var data = new CAData { DataType = type, Date = date };
            data.Data = JsonConvert.SerializeObject(obj);
            return data;
        }

        /// <summary>
        /// Froms the ca data.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="data">The data.</param>
        /// <returns>T.</returns>
        public static T FromCAData<T>(CAData data)
        {
            if (!string.IsNullOrEmpty(data.Data))
            {
                return JsonConvert.DeserializeObject<T>(data.Data);
            }

            return default(T);
        }
    }
}