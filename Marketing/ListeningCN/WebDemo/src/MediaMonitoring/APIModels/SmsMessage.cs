// ***********************************************************************
// Assembly         : MediaMonitoring
// Author           : 
// Created          : 07-25-2016
//
// Last Modified By : 
// Last Modified On : 07-25-2016
// ***********************************************************************
// <copyright file="SmsMessage.cs" company="">
//     Copyright ©  2016
// </copyright>
// <summary></summary>
// ***********************************************************************
namespace MediaMonitoring.APIModels
{
    using System.Collections.Generic;

    /// <summary>
    /// Class SmsMessage.
    /// </summary>
    public class SmsMessage
    {
        /// <summary>
        /// Gets to numbers.
        /// </summary>
        /// <value>To numbers.</value>
        public List<string> ToNumbers { get; } = new List<string>();

        /// <summary>
        /// Gets or sets the text body.
        /// </summary>
        /// <value>The text body.</value>
        public string TextBody { get; set; }
    }
}