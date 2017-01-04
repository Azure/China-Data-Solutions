// ***********************************************************************
// Assembly         : DataAccessLayer
// Author           : 
// Created          : 08-04-2016
//
// Last Modified By : 
// Last Modified On : 08-26-2016
// ***********************************************************************
// <copyright file="ProfileHelper.cs" company="">
//     Copyright ©  2016
// </copyright>
// <summary></summary>
// ***********************************************************************
namespace DataAccessLayer.Helper
{
    using System;
    using System.Collections.Generic;
    using System.Runtime.Caching;

    using DataAccessLayer.BusinessModel;
    using DataAccessLayer.DataModels;
    using DataAccessLayer.DataModels.Filters;
    using DataAccessLayer.Managers;

    using Newtonsoft.Json;

    /// <summary>
    /// Class ProfileHelper.
    /// </summary>
    public class ProfileHelper
    {
        /// <summary>
        /// The cache
        /// </summary>
        private static readonly MemoryCache cache;

        /// <summary>
        /// The profile manager
        /// </summary>
        private static readonly ProfileManager profileManager;

        /// <summary>
        /// Initializes static members of the <see cref="ProfileHelper"/> class.
        /// </summary>
        static ProfileHelper()
        {
            cache = MemoryCache.Default;
            profileManager = new ProfileManager();
        }

        /// <summary>
        /// Gets the client user.
        /// </summary>
        /// <param name="name">The name.</param>
        /// <returns>ClientUser.</returns>
        public static ClientUser GetClientUser(string name)
        {
            if (string.IsNullOrEmpty(name))
            {
                name = "Microsoft";
            }

           // if (cache.Contains(name))
           // {
           //     return cache[name] as ClientUser;
           // }
            var profile = profileManager.GetProfileByName(name);
            if (profile != null)
            {
                var user = new ClientUser(profile);
                cache[name] = user;
                return user;
            }
            return null;
        }

        /// <summary>
        /// Updates the client user.
        /// </summary>
        /// <param name="user">The user.</param>
        /// <returns>System.Int32.</returns>
        public static int UpdateClientUser(ClientUser user)
        {
            var result = profileManager.SaveProfile(user.GetProfile());
            cache.Remove(user.Name);
            return result;
        }

        /// <summary>
        /// Gets all profiles.
        /// </summary>
        /// <returns>List&lt;ClientUserProfile&gt;.</returns>
        public static List<ClientUserProfile> GetAllProfiles()
        {
            return profileManager.GetProfiles();
        }

        /// <summary>
        /// Initializes the client user.
        /// </summary>
        public static void InitClientUser()
        {
            var user = GetClientUser("Microsoft");

            if (user == null)
            {
                var profile = new ClientUserProfile();
                profile.Id = Guid.Empty.ToString();
                profile.UserName = "Microsoft";
                profile.Postfix = "BAT";
                var filter = InitCustomerFilter("微软", "微软");
                profile.Filters = JsonConvert.SerializeObject(filter);

                var competitors = new List<CustomerFilters>();
                competitors.Add(InitCustomerFilter("百度", "百度"));
                competitors.Add(InitCustomerFilter("阿里", "阿里"));
                competitors.Add(InitCustomerFilter("腾讯", "腾讯"));

                profile.CompetitorFilters = JsonConvert.SerializeObject(competitors);

                profileManager.SaveProfile(profile);
            }
        }

        /// <summary>
        /// Initializes the customer filter.
        /// </summary>
        /// <param name="userName">Name of the user.</param>
        /// <param name="value">The value.</param>
        /// <returns>CustomerFilters.</returns>
        internal static CustomerFilters InitCustomerFilter(string userName, string value)
        {
            var filter = new CustomerFilters { UserName = userName, UserFilterListCollection = new List<FilterList>() };
            var list = new FilterList();
            list.Filters.Add(new Filter { Name = "Keywords", Value = value });
            filter.UserFilterListCollection.Add(list);

            return filter;
        }
    }
}