// ***********************************************************************
// Assembly         : DataAccessLayer
// Author           : 
// Created          : 07-25-2016
//
// Last Modified By : 
// Last Modified On : 08-26-2016
// ***********************************************************************
// <copyright file="database_firewall_rules.cs" company="">
//     Copyright ©  2016
// </copyright>
// <summary></summary>
// ***********************************************************************
namespace DataAccessLayer.DataModels
{
    using System;

    /// <summary>
    /// Class database_firewall_rules.
    /// </summary>
    public class database_firewall_rules
    {
        /// <summary>
        /// Gets or sets the identifier.
        /// </summary>
        /// <value>The identifier.</value>
        public int id { get; set; }

        /// <summary>
        /// Gets or sets the name.
        /// </summary>
        /// <value>The name.</value>
        public string name { get; set; }

        /// <summary>
        /// Gets or sets the start ip address.
        /// </summary>
        /// <value>The start ip address.</value>
        public string start_ip_address { get; set; }

        /// <summary>
        /// Gets or sets the end ip address.
        /// </summary>
        /// <value>The end ip address.</value>
        public string end_ip_address { get; set; }

        /// <summary>
        /// Gets or sets the create date.
        /// </summary>
        /// <value>The create date.</value>
        public DateTime create_date { get; set; }

        /// <summary>
        /// Gets or sets the modify date.
        /// </summary>
        /// <value>The modify date.</value>
        public DateTime modify_date { get; set; }
    }
}