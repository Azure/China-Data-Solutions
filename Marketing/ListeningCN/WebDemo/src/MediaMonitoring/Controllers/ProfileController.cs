// ***********************************************************************
// Assembly         : MediaMonitoring
// Author           : 
// Created          : 08-22-2016
//
// Last Modified By : 
// Last Modified On : 08-26-2016
// ***********************************************************************
// <copyright file="ProfileController.cs" company="">
//     Copyright ©  2016
// </copyright>
// <summary></summary>
// ***********************************************************************
namespace MediaMonitoring.Controllers
{
    using System;
    using System.Web.Http;

    using DataAccessLayer;
    using DataAccessLayer.BusinessModel;
    using DataAccessLayer.DataModels.Filters;
    using DataAccessLayer.Helper;

    using Hangfire;
    using Hangfire.SqlServer;

    using MediaMonitoring.Utility;

    using Newtonsoft.Json;
    using System.Collections.Generic;

    /// <summary>
    /// Class ProfileController.
    /// </summary>
    /// <seealso cref="System.Web.Http.ApiController" />
    public class ProfileController : ApiController
    {
        /// <summary>
        /// Updates the profile.
        /// </summary>
        /// <param name="clientProfile">The client profile.</param>
        /// <returns>IHttpActionResult.</returns>
        [HttpPost]
        public IHttpActionResult UpdateProfile([FromBody] string clientProfile)
        {
            if (string.IsNullOrEmpty(this.User.Identity.Name))
            {
                return this.Ok();
            }

            dynamic obj = JsonConvert.DeserializeObject(clientProfile);
            var clientUser = ProfileHelper.GetClientUser(this.User.Identity.Name);
            clientUser.UserFilter = FilterFromPost(obj.company);
            clientUser.CompetitorFilter.Clear();
            foreach (var item in obj.competitors)
            {                
                if (item.name == ""||item.filters.Count==0) continue;
                var filter = FilterFromPost(item);
                clientUser.CompetitorFilter.Add(filter);
            }
            clientUser.UserFilter.UserName = this.User.Identity.Name;//公司名称不能被修改
            

            if (this.IsValidFilter(clientUser)|| IsValidCompetitorFilter(clientUser))
            {
                ProfileHelper.UpdateClientUser(clientUser);
                clientUser = ProfileHelper.GetClientUser(this.User.Identity.Name);
                BackgroundJob.Enqueue(() => TaskProcessor.InitDataFast(clientUser));
            }

            return this.Ok(clientUser);
        }

        /// <summary>
        /// Gets this instance.
        /// </summary>
        /// <returns>IHttpActionResult.</returns>
        [HttpGet]
        public IHttpActionResult Get()
        {
            var clientUser = ProfileHelper.GetClientUser(this.User.Identity.Name);
            return this.Ok(clientUser);
        }

        /// <summary>
        /// Filters from post.
        /// </summary>
        /// <param name="obj">The object.</param>
        /// <returns>CustomerFilters.</returns>
        private CustomerFilters FilterFromPost(dynamic obj)
        {
            var result = new CustomerFilters();

            result.UserName = obj.name;
            result.UserFilterListCollection.Clear();
            foreach (var filter in obj.filters)
            {
                var list = new FilterList();
                var array = ((string)filter).Split(new[] { '+' }, StringSplitOptions.RemoveEmptyEntries);
                foreach (var val in array)
                {
                    if (!string.IsNullOrEmpty(val))
                    {
                        list.Filters.Add(new Filter { Name = "Keywords", Value = val });
                    }
                }

                if (list.Filters.Count > 0)
                {
                    result.UserFilterListCollection.Add(list);
                }
            }

            return result;
        }

        /// <summary>
        /// Determines whether [is valid filter] [the specified client user].
        /// </summary>
        /// <param name="clientUser">The client user.</param>
        /// <returns><c>true</c> if [is valid filter] [the specified client user]; otherwise, <c>false</c>.</returns>
        private bool IsValidFilter(ClientUser clientUser)
        {
            var result = clientUser.UserFilter != null && !string.IsNullOrEmpty(clientUser.UserFilter.UserName)
                         && clientUser.UserFilter.UserFilterListCollection.Count > 0;

           
            
            return result;
        }

        private bool IsValidCompetitorFilter(ClientUser clientUser)
        {
            var result=true;
            foreach (var com in clientUser.CompetitorFilter)
            {
                result &= com != null && !string.IsNullOrEmpty(com.UserName) && com.UserFilterListCollection.Count > 0;
            }
            return result;
        }
    }
}