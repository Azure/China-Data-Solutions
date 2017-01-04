// ***********************************************************************
// Assembly         : DataAccessLayer
// Author           : 
// Created          : 08-22-2016
//
// Last Modified By : 
// Last Modified On : 08-26-2016
// ***********************************************************************
// <copyright file="SysConfig.cs" company="">
//     Copyright ©  2016
// </copyright>
// <summary></summary>
// ***********************************************************************
namespace DataAccessLayer
{
    using System.Configuration;

    using DataAccessLayer.Helper;

    /// <summary>
    /// Class SysConfig.
    /// </summary>
    public class SysConfig
    {
        /// <summary>
        /// The date range
        /// </summary>
        private int dateRange;

        /// <summary>
        /// Gets or sets the company.
        /// </summary>
        /// <value>The company.</value>
        public string Company { get; set; }

        private static int defaultInterval = 14400;

        /// <summary>
        /// Gets or sets the date range.
        /// </summary>
        /// <value>The date range.</value>
        public int DateRange
        {
            get
            {
                if (this.dateRange <= 0)
                {
                    var range = 7;
                    int.TryParse(ConfigurationManager.AppSettings["DateRange"], out range);
                    this.dateRange = range;
                }

                return this.dateRange;
            }
            set
            {
                this.dateRange = value;
            }
        }

        /// <summary>
        /// Gets the hot weibo news amount.
        /// </summary>
        /// <value>The hot weibo news amount.</value>
        public int HotWeiboNewsAmount
        {
            get
            {
                return ConfigurationReader.ReadValue<int>("HotNewsNumber");
            }
        }

        /// <summary>
        /// Gets the notification count.
        /// </summary>
        /// <value>The notification count.</value>
        public int NotificationCount
        {
            get
            {
                return ConfigurationReader.ReadValue<int>("NumberOfNotification");
            }
        }

        /// <summary>
        /// Gets the favoriate count.
        /// </summary>
        /// <value>The favoriate count.</value>
        public int FavoriateCount
        {
            get
            {
                return ConfigurationReader.ReadValue<int>("NumberOfFav");
            }
        }

        /// <summary>
        /// Gets the default connection string.
        /// </summary>
        /// <value>The default connection string.</value>
        public static string DefaultConnStr
        {
            get
            {
#if DEBUG
                var connstr = ConfigurationManager.ConnectionStrings["JobContext"].ConnectionString;
                if (!string.IsNullOrEmpty(connstr))
                {
                    return connstr;
                }
#endif
                return ConfigurationManager.ConnectionStrings["DataContext"].ConnectionString;
            }
        }

        /// <summary>
        /// Gets the data synchronize interval.
        /// </summary>
        /// <value>The data synchronize interval.</value>
        public static int DataSyncInterval
        {
            get
            {
                return ParseConfigInt("DataSyncInterval", defaultInterval);
            }
        }

        /// <summary>
        /// Gets the reports interval.
        /// </summary>
        /// <value>The reports interval.</value>
        public static int ReportsInterval
        {
            get
            {
                return ParseConfigInt("ReportsInterval", defaultInterval);
            }
        }

        /// <summary>
        /// Gets the weibo interval.
        /// </summary>
        /// <value>The weibo interval.</value>
        public static int WeiboInterval
        {
            get
            {
                return ParseConfigInt("WeiboInterval", defaultInterval);
            }
        }

        /// <summary>
        /// Gets the word cloud interval.
        /// </summary>
        /// <value>The word cloud interval.</value>
        public static int WordCloudInterval
        {
            get
            {
                return ParseConfigInt("WordCloudInterval", defaultInterval);
            }
        }

        /// <summary>
        /// Gets a value indicating whether [synchronize weibo].
        /// </summary>
        /// <value><c>true</c> if [synchronize weibo]; otherwise, <c>false</c>.</value>
        public static bool SyncWeibo
        {
            get
            {
                return ConfigurationReader.ReadValue<bool>("SyncWeibo");
            }
        }

        /// <summary>
        /// Gets a value indicating whether [use static date].
        /// </summary>
        public static bool UseStaticDate
        {
            get
            {
                return ConfigurationReader.ReadValue<bool>("UseStaticDate");
            }
        }


        public static bool AllowLogin
        {
            get
            {
                return ConfigurationReader.ReadValue<bool>("AllowLogin");
            }
        }


        public static bool AllowDatePick
        {
            get
            {
                return ConfigurationReader.ReadValue<bool>("AllowDatePick");
            }
        }

        public static bool DoWeiboFallback
        {
            get
            {
                return ConfigurationReader.ReadValue<bool>("DoWeiboFallback");
            }
        }

        /// <summary>
        /// Parses the configuration int.
        /// </summary>
        /// <param name="key">The key.</param>
        /// <param name="defaultVal">The default value.</param>
        /// <returns>System.Int32.</returns>
        private static int ParseConfigInt(string key, int defaultVal)
        {
            var result = defaultVal;
            int.TryParse(ConfigurationManager.AppSettings[key], out result);
            return result;
        }
    }
}