// ***********************************************************************
// Assembly         : DataAccessLayer
// Author           : 
// Created          : 07-25-2016
//
// Last Modified By : 
// Last Modified On : 08-26-2016
// ***********************************************************************
// <copyright file="HotNewsPredictionMap.cs" company="">
//     Copyright ©  2016
// </copyright>
// <summary></summary>
// ***********************************************************************
namespace DataAccessLayer.DataModels.Mapping
{
    using System.Data.Entity.ModelConfiguration;

    /// <summary>
    /// Class HotNewsPredictionMap.
    /// </summary>
    /// <seealso cref="System.Data.Entity.ModelConfiguration.EntityTypeConfiguration{DataAccessLayer.DataModels.HotNewsPrediction}" />
    public class HotNewsPredictionMap : EntityTypeConfiguration<HotNewsPrediction>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="HotNewsPredictionMap"/> class.
        /// </summary>
        /// <param name="postfix">The postfix.</param>
        public HotNewsPredictionMap(string postfix)
        {
            // Primary Key
            this.HasKey(t => new { t.Date, t.ClusterId0, t.ClusterId4 });

            // Properties
            this.Property(t => t.ClusterId0).IsRequired().HasMaxLength(255);

            this.Property(t => t.ClusterId4).IsRequired().HasMaxLength(255);

            // Table & Column Mappings
            this.ToTable("HotNewsPrediction_" + postfix);
            this.Property(t => t.Date).HasColumnName("Date");
            this.Property(t => t.ClusterId0).HasColumnName("ClusterId0");
            this.Property(t => t.ClusterId4).HasColumnName("ClusterId4");
            this.Property(t => t.PredictionTime).HasColumnName("PredictionTime");
            this.Property(t => t.Probability).HasColumnName("Probability");
            this.Property(t => t.HotRank).HasColumnName("HotRank");
        }
    }
}