// ***********************************************************************
// Assembly         : DataAccessLayer
// Author           : 
// Created          : 08-02-2016
//
// Last Modified By : 
// Last Modified On : 08-26-2016
// ***********************************************************************
// <copyright file="ProfileManager.cs" company="">
//     Copyright ©  2016
// </copyright>
// <summary></summary>
// ***********************************************************************
namespace DataAccessLayer.Managers
{
    using System;
    using System.Collections.Generic;
    using System.Data.Entity;
    using System.Linq;

    using DataAccessLayer.DataModels;
    using DataAccessLayer.DataModels.Context;

    /// <summary>
    /// Class ProfileManager.
    /// </summary>
    public class ProfileManager
    {
        /// <summary>
        /// Gets the profile by identifier.
        /// </summary>
        /// <param name="Id">The identifier.</param>
        /// <returns>ClientUserProfile.</returns>
        public ClientUserProfile GetProfileById(string Id)
        {
            using (var context = ContextFactory.GetProfileContext())
            {
                return context.Profiles.FirstOrDefault(i => i.Id == Id);
            }
        }

        /// <summary>
        /// Gets the profiles.
        /// </summary>
        /// <returns>List&lt;ClientUserProfile&gt;.</returns>
        public List<ClientUserProfile> GetProfiles()
        {
            using (var context = ContextFactory.GetProfileContext())
            {
                return context.Profiles.ToList();
            }
        }

        /// <summary>
        /// Gets the name of the profile by.
        /// </summary>
        /// <param name="name">The name.</param>
        /// <returns>ClientUserProfile.</returns>
        public ClientUserProfile GetProfileByName(string name)
        {
            using (var context = ContextFactory.GetProfileContext())
            {
                try
                {
                    return context.Profiles.FirstOrDefault(i => i.UserName == name);
                }
                catch
                {
                    return null;
                }
            }
        }

        /// <summary>
        /// Saves the profile.
        /// </summary>
        /// <param name="profile">The profile.</param>
        /// <returns>System.Int32.</returns>
        public int SaveProfile(ClientUserProfile profile)
        {
            using (var context = ContextFactory.GetProfileContext())
            {
                var retrived = this.GetProfileById(profile.Id);
                if (retrived == null)
                {
                    profile.CreatedTime = DateTime.UtcNow;
                    context.Profiles.Add(profile);
                }
                else
                {
                    retrived.LastUpdatedTime = DateTime.UtcNow;
                    retrived.CompetitorFilters = profile.CompetitorFilters;
                    retrived.Filters = profile.Filters;
                    context.Entry(retrived).State = EntityState.Modified;
                }

                var arow = context.SaveChanges();
                return arow;
            }
        }
    }
}