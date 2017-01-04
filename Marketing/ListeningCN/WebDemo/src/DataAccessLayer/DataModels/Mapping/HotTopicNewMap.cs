// ***********************************************************************
// Assembly         : DataAccessLayer
// Author           : 
// Created          : 07-25-2016
//
// Last Modified By : 
// Last Modified On : 08-26-2016
// ***********************************************************************
// <copyright file="HotTopicNewMap.cs" company="">
//     Copyright ©  2016
// </copyright>
// <summary></summary>
// ***********************************************************************
namespace DataAccessLayer.DataModels.Mapping
{
    using System.Data.Entity.ModelConfiguration;

    /// <summary>
    /// Class HotTopicNewMap.
    /// </summary>
    /// <seealso cref="System.Data.Entity.ModelConfiguration.EntityTypeConfiguration{DataAccessLayer.DataModels.HotTopicNew}" />
    public class HotTopicNewMap : EntityTypeConfiguration<HotTopicNew>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="HotTopicNewMap"/> class.
        /// </summary>
        /// <param name="postfix">The postfix.</param>
        public HotTopicNewMap(string postfix)
        {
            // Primary Key
            this.HasKey(t => t.Id);

            // Properties
            this.Property(t => t.Id).IsRequired().HasMaxLength(255);

            this.Property(t => t.Market).HasMaxLength(255);

            this.Property(t => t.TopicType).HasMaxLength(255);

            this.Property(t => t.Content).IsRequired().HasMaxLength(1024);

            this.Property(t => t.ClusterId0).HasMaxLength(255);

            this.Property(t => t.ClusterId4).HasMaxLength(255);

            // Table & Column Mappings
            this.ToTable("HotTopicNews_" + postfix);
            this.Property(t => t.Id).HasColumnName("Id");
            this.Property(t => t.HotTime).HasColumnName("HotTime");
            this.Property(t => t.Market).HasColumnName("Market");
            this.Property(t => t.TopicType).HasColumnName("TopicType");
            this.Property(t => t.HotRank).HasColumnName("HotRank");
            this.Property(t => t.Content).HasColumnName("Content");
            this.Property(t => t.ClusterId0).HasColumnName("ClusterId0");
            this.Property(t => t.ClusterId4).HasColumnName("ClusterId4");
        }
    }
}