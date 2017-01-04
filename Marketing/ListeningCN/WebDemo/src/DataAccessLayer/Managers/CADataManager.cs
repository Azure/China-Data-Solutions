// ***********************************************************************
// Assembly         : DataAccessLayer
// Author           : 
// Created          : 07-25-2016
//
// Last Modified By : 
// Last Modified On : 08-26-2016
// ***********************************************************************
// <copyright file="CADataManager.cs" company="">
//     Copyright ©  2016
// </copyright>
// <summary></summary>
// ***********************************************************************
namespace DataAccessLayer.Managers
{
    using System;
    using System.Data.Entity;
    using System.Linq;

    using DataAccessLayer.BusinessModel;
    using DataAccessLayer.DataModels;
    using DataAccessLayer.DataModels.Context;

    /// <summary>
    /// Class CADataManager.
    /// </summary>
    /// <seealso cref="DataAccessLayer.Managers.ManagerBase" />
    public class CADataManager : ManagerBase
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="CADataManager"/> class.
        /// </summary>
        /// <param name="config">The configuration.</param>
        /// <param name="clientUser">The client user.</param>
        public CADataManager(SysConfig config, ClientUser clientUser)
            : base(config, clientUser)
        {
        }

        /// <summary>
        /// Gets the maximum date.
        /// </summary>
        /// <param name="dataType">Type of the data.</param>
        /// <returns>DateTime.</returns>
        public DateTime GetMaxDate(string dataType)
        {
            if (this.currentClientUser == null) return DateTime.UtcNow.AddDays(-1);
            using (var context = ContextFactory.GetContext(this.currentClientUser.GetProfile().Postfix))
            {
                return
                    context.CAData.Where(i => i.DataType == dataType && i.Company == this.currentClientUser.Name)
                        .Max(x => x.Date) ?? DateTime.UtcNow.AddDays(-1);
            }
        }

        /// <summary>
        /// The get ca data.
        /// </summary>
        /// <param name="dataType">The data type.</param>
        /// <param name="date">The date.</param>
        /// <returns>The <see cref="CAData" />.</returns>
        public CAData GetCaData(string dataType, DateTime date)
        {
            using (var context = this.GetContext())
            {
                return
                    context.CAData.FirstOrDefault(
                        i => i.DataType == dataType && i.Date == date && i.Company == this.currentClientUser.Name);
            }
        }

        /// <summary>
        /// The save ca data.
        /// </summary>
        /// <param name="data">The data.</param>
        public void SaveCaData(CAData data)
        {
            data.Company = this.currentClientUser.Name;
            using (var context = this.GetContext())
            {
                var retrived = this.GetCaData(data.DataType, data.Date ?? DateTime.Now);
                if (retrived == null)
                {
                    context.CAData.Add(data);
                }
                else
                {
                    retrived.Data = data.Data;
                    context.Entry(retrived).State = EntityState.Modified;
                }

                var arow = context.SaveChanges();
            }
        }

        /// <summary>
        /// The refresh ca data.
        /// </summary>
        /// <param name="date">The date.</param>
        /// <param name="newDate">The new date.</param>
        public void RefreshCaData(DateTime date, DateTime newDate)
        {
            using (var context = this.GetContext())
            {
                context.Database.ExecuteSqlCommand(
                    "update cadata set date = {0} where date = {1} and company ={2}",
                    newDate,
                    date,
                    this.config.Company);
            }
        }

        /// <summary>
        /// Get the min date for a kind of data.
        /// </summary>
        /// <param name="eventType">The given event type.</param>
        /// <returns>The min date for the event type.</returns>
        public DateTime GetMinDate(string eventType)
        {
            using (var context = this.GetContext())
            {
                var date =context.CAData.Where(i => i.DataType == eventType && i.Company == this.currentClientUser.Name)
                        .Min(i => i.Date);
                return date ?? DateTime.UtcNow;
            }
        }
    }
}