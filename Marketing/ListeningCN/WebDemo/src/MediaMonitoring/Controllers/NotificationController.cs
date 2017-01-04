// ***********************************************************************
// Assembly         : MediaMonitoring
// Author           : 
// Created          : 07-25-2016
//
// Last Modified By : 
// Last Modified On : 08-09-2016
// ***********************************************************************
// <copyright file="NotificationController.cs" company="">
//     Copyright ©  2016
// </copyright>
// <summary></summary>
// ***********************************************************************
namespace MediaMonitoring.Controllers
{
    using System.Web.Http;

    using MediaMonitoring.APIModels;

    /// <summary>
    /// Class NotificationController.
    /// </summary>
    /// <seealso cref="System.Web.Http.ApiController" />
    public class NotificationController : ApiController
    {
        /// <summary>
        /// Tests this instance.
        /// </summary>
        /// <returns>IHttpActionResult.</returns>
        [HttpGet]
        public IHttpActionResult Test()
        {
            return this.Ok("Hello world");
        }

        /// <summary>
        /// Sends the SMS message.
        /// </summary>
        /// <param name="message">The message.</param>
        /// <returns>IHttpActionResult.</returns>
        [HttpPost]
        public IHttpActionResult SendSmsMessage([FromBody] SmsMessage message)
        {
            return this.Ok();
        }
    }
}