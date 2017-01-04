// ***********************************************************************
// Assembly         : DataAccessLayer
// Author           : 
// Created          : 07-25-2016
//
// Last Modified By : 
// Last Modified On : 08-26-2016
// ***********************************************************************
// <copyright file="LoadHistoryMap.cs" company="">
//     Copyright ©  2016
// </copyright>
// <summary></summary>
// ***********************************************************************
namespace DataAccessLayer.DataModels.Mapping
{
    using System.Data.Entity.ModelConfiguration;

    /// <summary>
    /// Class LoadHistoryMap.
    /// </summary>
    /// <seealso cref="System.Data.Entity.ModelConfiguration.EntityTypeConfiguration{DataAccessLayer.DataModels.LoadHistory}" />
    public class LoadHistoryMap : EntityTypeConfiguration<LoadHistory>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="LoadHistoryMap"/> class.
        /// </summary>
        public LoadHistoryMap()
        {
            // Primary Key
            this.HasKey(t => t.ID);

            // Properties
            // Table & Column Mappings
            this.ToTable("LoadHistory");
            this.Property(t => t.ID).HasColumnName("ID");
            this.Property(t => t.Date).HasColumnName("Date");
            this.Property(t => t.HostVisitCount).HasColumnName("HostVisitCount");
            this.Property(t => t.NewsSentiments).HasColumnName("NewsSentiments");
            this.Property(t => t.NewsStream).HasColumnName("NewsStream");
            this.Property(t => t.LocationAndUserDemo).HasColumnName("LocationAndUserDemo");
            this.Property(t => t.HostVisitCount7Days).HasColumnName("HostVisitCount7Days");
            this.Property(t => t.NewsSentiments7Days).HasColumnName("NewsSentiments7Days");
            this.Property(t => t.NewsStream7Days).HasColumnName("NewsStream7Days");
            this.Property(t => t.LocationAndUserDemo7Days).HasColumnName("LocationAndUserDemo7Days");
            this.Property(t => t.HotNewsPrediction7Days).HasColumnName("HotNewsPrediction7Days");
        }
    }
}