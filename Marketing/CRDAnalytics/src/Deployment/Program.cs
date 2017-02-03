// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

namespace Microsoft.Azure.ChinaDataSolution.CrdAnalytics.Deployment
{
    using System;

    using Common.Extensions;

    /// <summary>
    /// Defines the program class.
    /// </summary>
    internal static class Program
    {
        #region Methods

        /// <summary>
        /// The console application entry method.
        /// </summary>
        /// <param name="args">The arguments.</param>
        public static void Main(string[] args)
        {
            try
            {
                // could write test code from here.
            }
            catch (Exception exception)
            {
                Console.WriteLine(exception.GetDetailMessage());
            }

            Console.WriteLine(@"Press any key to continue...");
            Console.ReadKey(true);
        }

        #endregion
    }
}
