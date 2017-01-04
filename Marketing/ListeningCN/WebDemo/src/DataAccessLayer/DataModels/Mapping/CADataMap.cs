// ***********************************************************************
// Assembly         : DataAccessLayer
// Author           : 
// Created          : 07-25-2016
//
// Last Modified By : 
// Last Modified On : 08-26-2016
// ***********************************************************************
// <copyright file="CADataMap.cs" company="">
//     Copyright ©  2016
// </copyright>
// <summary></summary>
// ***********************************************************************
namespace DataAccessLayer.DataModels.Mapping
{
    using System.Data.Entity.ModelConfiguration;

    /// <summary>
    /// Class CADataMap.
    /// </summary>
    /// <seealso cref="System.Data.Entity.ModelConfiguration.EntityTypeConfiguration{DataAccessLayer.DataModels.CAData}" />
    public class CADataMap : EntityTypeConfiguration<CAData>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="CADataMap"/> class.
        /// </summary>
        public CADataMap()
        {
            // Primary Key
            this.HasKey(t => t.Id);

            // Properties
            this.Property(t => t.DataType).HasMaxLength(25);

            // Table & Column Mappings
            this.ToTable("CAData");
            this.Property(t => t.Id).HasColumnName("id");
            this.Property(t => t.DataType).HasColumnName("datatype");
            this.Property(t => t.Date).HasColumnName("Date");
            this.Property(t => t.Data).HasColumnName("Data");
            this.Property(t => t.Company).HasColumnName("Company");
        }
    }
}