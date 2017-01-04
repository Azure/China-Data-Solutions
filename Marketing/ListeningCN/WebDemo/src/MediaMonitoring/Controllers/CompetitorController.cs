// ***********************************************************************
// Assembly         : MediaMonitoring
// Author           : 
// Created          : 07-25-2016
//
// Last Modified By : 
// Last Modified On : 07-25-2016
// ***********************************************************************
// <copyright file="CompetitorController.cs" company="">
//     Copyright ©  2016
// </copyright>
// <summary></summary>
// ***********************************************************************
namespace MediaMonitoring.Controllers
{
    using System.Web.Mvc;

    /// <summary>
    /// Class CompetitorController.
    /// </summary>
    /// <seealso cref="System.Web.Mvc.Controller" />
    public class CompetitorController : Controller
    {
        // GET: Competitor
        /// <summary>
        /// Indexes this instance.
        /// </summary>
        /// <returns>ActionResult.</returns>
        public ActionResult Index()
        {
            return this.View();
        }

        /// <summary>
        /// Events the river.
        /// </summary>
        /// <returns>ActionResult.</returns>
        public ActionResult EventRiver()
        {
            return this.View();
        }

        /// <summary>
        /// Maps the graph.
        /// </summary>
        /// <returns>ActionResult.</returns>
        public ActionResult MapGraph()
        {
            return this.View();
        }
    }
}