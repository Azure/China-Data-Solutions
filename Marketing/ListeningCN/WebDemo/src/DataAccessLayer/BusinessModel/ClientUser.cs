// ***********************************************************************
// Assembly         : DataAccessLayer
// Author           : 
// Created          : 08-02-2016
//
// Last Modified By : 
// Last Modified On : 08-26-2016
// ***********************************************************************
// <copyright file="ClientUser.cs" company="">
//     Copyright ©  2016
// </copyright>
// <summary></summary>
// ***********************************************************************
namespace DataAccessLayer.BusinessModel
{
    using System.Collections.Generic;

    using DataAccessLayer.DataModels;
    using DataAccessLayer.DataModels.Filters;

    using Newtonsoft.Json;

    /// <summary>
    /// Class ClientUser.
    /// </summary>
    public class ClientUser
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ClientUser"/> class.
        /// </summary>
        public ClientUser()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ClientUser"/> class.
        /// </summary>
        /// <param name="profile">The profile.</param>
        public ClientUser(ClientUserProfile profile)
        {
            this.Id = profile.Id;
            this.Name = profile.UserName;
            this.InitProfile(profile);
        }

        /// <summary>
        /// Gets or sets the name.
        /// </summary>
        /// <value>The name.</value>
        public string Name { get; set; }

        /// <summary>
        /// Gets or sets the identifier.
        /// </summary>
        /// <value>The identifier.</value>
        public string Id { get; set; }

        /// <summary>
        /// Gets or sets the postfix.
        /// </summary>
        /// <value>The postfix.</value>
        public string Postfix { get; set; }

        /// <summary>
        /// Gets or sets the user filter.
        /// </summary>
        /// <value>The user filter.</value>
        public CustomerFilters UserFilter { get; set; }

        /// <summary>
        /// Gets or sets the competitor filter.
        /// </summary>
        /// <value>The competitor filter.</value>
        public List<CustomerFilters> CompetitorFilter { get; set; }

        /// <summary>
        /// Gets the profile.
        /// </summary>
        /// <returns>ClientUserProfile.</returns>
        public ClientUserProfile GetProfile()
        {
            var profile = new ClientUserProfile
                              {
                                  Id = this.Id,
                                  UserName = this.Name,
                                  Postfix = this.Postfix,
                                  Filters = JsonConvert.SerializeObject(this.UserFilter),
                                  CompetitorFilters = JsonConvert.SerializeObject(this.CompetitorFilter)
                              };
            return profile;
        }

        /// <summary>
        /// Initializes the profile.
        /// </summary>
        /// <param name="value">The value.</param>
        public void InitProfile(ClientUserProfile value)
        {
            if (value != null)
            {
                this.Postfix = value.Postfix;
                this.UserFilter = !string.IsNullOrEmpty(value.Filters)
                                      ? JsonConvert.DeserializeObject<CustomerFilters>(value.Filters)
                                      : new CustomerFilters();
                List<CustomerFilters> list= !string.IsNullOrEmpty(value.Filters)
                                            ? JsonConvert.DeserializeObject<List<CustomerFilters>>(
                                                value.CompetitorFilters)
                                            : new List<CustomerFilters>();
                if(list.Count == 1)
                {
                    if (list[0].UserName == "")
                        list = new List<CustomerFilters>();
                }
                this.CompetitorFilter = list;
            }
            else
            {
                this.UserFilter = new CustomerFilters();
                this.CompetitorFilter = new List<CustomerFilters>();
            }
        }
    }
}