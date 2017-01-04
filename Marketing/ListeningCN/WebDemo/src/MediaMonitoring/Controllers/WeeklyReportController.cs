// ***********************************************************************
// Assembly         : MediaMonitoring
// Author           : 
// Created          : 07-25-2016
//
// Last Modified By : 
// Last Modified On : 07-25-2016
// ***********************************************************************
// <copyright file="WeeklyReportController.cs" company="">
//     Copyright ©  2016
// </copyright>
// <summary></summary>
// ***********************************************************************
namespace MediaMonitoring.Controllers
{
    using System.Web.Mvc;

    /// <summary>
    /// Class WeeklyReportController.
    /// </summary>
    /// <seealso cref="System.Web.Mvc.Controller" />
    public class WeeklyReportController : Controller
    {
        // GET: WeeklyReport
        /// <summary>
        /// Indexes this instance.
        /// </summary>
        /// <returns>ActionResult.</returns>
        public ActionResult Index()
        {
            return this.View();
        }

        /// <summary>
        /// Selfs the event river.
        /// </summary>
        /// <returns>ActionResult.</returns>
        public ActionResult SelfEventRiver()
        {
            return this.View();
        }
    }
}