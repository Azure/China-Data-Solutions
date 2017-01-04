// ***********************************************************************
// Assembly         : DataAccessLayer
// Author           : 
// Created          : 07-25-2016
//
// Last Modified By : 
// Last Modified On : 08-26-2016
// ***********************************************************************
// <copyright file="CompanyKeywordManager.cs" company="">
//     Copyright ©  2016
// </copyright>
// <summary></summary>
// ***********************************************************************
namespace DataAccessLayer.Managers
{
    using System.Collections.Generic;
    using System.Linq;

    using DataAccessLayer.BusinessModel;
    using DataAccessLayer.DataModels.Filters;

    /// <summary>
    /// Class CompanyKeywordManager.
    /// </summary>
    /// <seealso cref="DataAccessLayer.Managers.ManagerBase" />
    public class CompanyKeywordManager : ManagerBase
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="CompanyKeywordManager"/> class.
        /// </summary>
        /// <param name="config">The configuration.</param>
        /// <param name="clientUser">The client user.</param>
        public CompanyKeywordManager(SysConfig config, ClientUser clientUser)
            : base(config, clientUser)

        {
        }

        /// <summary>
        /// Gets the company.
        /// </summary>
        /// <returns>System.String.</returns>
        public string GetCompany()
        {
            return currentClientUser.Name;  //UserFilter.UserName;
        }

        /// <summary>
        /// Gets the company keywords.
        /// </summary>
        /// <returns>List&lt;System.String&gt;.</returns>
        public List<string> GetCompanyKeywords()
        {
            var keywords = new List<string>();
            foreach (var list in this.currentClientUser.UserFilter.UserFilterListCollection)
            {
                foreach (var filter in list.Filters)
                {
                    if (filter != null && !string.IsNullOrEmpty(filter.Value))
                    {
                        keywords.Add(filter.Value);
                    }
                }
            }

            return keywords;
        }

        /// <summary>
        /// Gets the companies.
        /// </summary>
        /// <returns>List&lt;System.String&gt;.</returns>
        public List<string> GetCompanies()
        {
            var list = new List<string>();
            list.Add(this.currentClientUser.UserFilter.UserName);
            list.AddRange(this.currentClientUser.CompetitorFilter.Select(i => i.UserName).ToList());

            return list;
        }

        /// <summary>
        /// Gets the filters.
        /// </summary>
        /// <param name="companyName">Name of the company.</param>
        /// <returns>CustomerFilters.</returns>
        public CustomerFilters GetFilters(string companyName)
        {
            if (this.currentClientUser.UserFilter.UserName == companyName)
            {
                return this.currentClientUser.UserFilter;
            }
            return this.currentClientUser.CompetitorFilter.FirstOrDefault(i => i.UserName == companyName);
        }
    }
}