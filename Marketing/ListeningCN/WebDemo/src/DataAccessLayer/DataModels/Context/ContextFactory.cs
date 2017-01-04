// ***********************************************************************
// Assembly         : DataAccessLayer
// Author           : 
// Created          : 07-25-2016
//
// Last Modified By : 
// Last Modified On : 08-26-2016
// ***********************************************************************
// <copyright file="ContextFactory.cs" company="">
//     Copyright ©  2016
// </copyright>
// <summary></summary>
// ***********************************************************************
using System;

namespace DataAccessLayer.DataModels.Context
{
    /// <summary>
    /// Class ContextFactory.
    /// </summary>
    public class ContextFactory
    {
        /// <summary>
        /// Gets the profile context.
        /// </summary>
        /// <returns>ProfileDataContext.</returns>
        public static ProfileDataContext GetProfileContext()
        {
            return new ProfileDataContext();
        }

        /// <summary>
        /// Gets the context.
        /// </summary>
        /// <param name="user">The user.</param>
        /// <returns>DataContextBase.</returns>
        public static DataContextBase GetContext(ClientUserProfile user)
        {
            return GetContext(user.Postfix);
        }

        /// <summary>
        /// Gets the context.
        /// </summary>
        /// <param name="key">The key.</param>
        /// <returns>DataContextBase.</returns>
        public static DataContextBase GetContext(string key)
        {
            Type dynamicType = DynamicContextCreator.CreateMyNewType($"Identifier{key}", "ContextName", typeof(string), typeof(ContextIdentifier));
            var obj = Activator.CreateInstance(dynamicType);
            var genericListType = typeof(DataContext<>);
            var specificListType = genericListType.MakeGenericType(dynamicType);
            var context = Activator.CreateInstance(specificListType, key) as DataContextBase;
            return context;
        }
    }
}