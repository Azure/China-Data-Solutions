// ***********************************************************************
// Assembly         : DataAccessLayer
// Author           : 
// Created          : 08-02-2016
//
// Last Modified By : 
// Last Modified On : 08-26-2016
// ***********************************************************************
// <copyright file="ManagerBase.cs" company="">
//     Copyright ©  2016
// </copyright>
// <summary></summary>
// ***********************************************************************
namespace DataAccessLayer.Managers
{
    using DataAccessLayer.BusinessModel;
    using DataAccessLayer.DataModels.Context;

    /// <summary>
    /// Class ManagerBase.
    /// </summary>
    public class ManagerBase
    {
        /// <summary>
        /// The configuration
        /// </summary>
        protected readonly SysConfig config;

        /// <summary>
        /// The current client user
        /// </summary>
        protected readonly ClientUser currentClientUser;

        /// <summary>
        /// Initializes a new instance of the <see cref="ManagerBase"/> class.
        /// </summary>
        /// <param name="config">The configuration.</param>
        /// <param name="clientUser">The client user.</param>
        public ManagerBase(SysConfig config, ClientUser clientUser)
        {
            this.config = config;
            this.currentClientUser = clientUser;
        }

        /// <summary>
        /// Gets the context.
        /// </summary>
        /// <returns>DataContextBase.</returns>
        protected DataContextBase GetContext()
        {
            return ContextFactory.GetContext(this.currentClientUser.GetProfile());
        }
    }
}