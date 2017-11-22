// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

namespace Microsoft.Azure.ChinaDataSolution.CrdAnalytics.Common.DataAccess.EntityConfigurations
{
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.ModelConfiguration;

    using Entities;

    /// <summary>
    /// Defines the product review entity type configuration.
    /// </summary>
    /// <seealso cref="EntityTypeConfiguration{ProductReviewEntity}" />
    internal sealed class ProductReviewEntityConfiguration
        : EntityTypeConfiguration<ProductReviewEntity>
    {
        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="ProductReviewEntityConfiguration"/> class.
        /// </summary>
        public ProductReviewEntityConfiguration()
        {
            this.ToTable(TableNames.ProductReviews)
                .HasKey(e => e.Id);

            this.Property(e => e.Id)
                .HasColumnName(ColumnNames.Id)
                .HasColumnType(ColumnTypes.BigInt)
                .HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity)
                .IsRequired();

            this.Property(e => e.ProductId)
                .HasColumnName(ColumnNames.ProductId)
                .HasColumnType(ColumnTypes.Int)
                .IsOptional();

            this.Property(e => e.Channel)
                .HasColumnName(ColumnNames.Channel)
                .HasColumnType(ColumnTypes.NVarChar)
                .IsUnicode()
                .IsVariableLength()
                .HasMaxLength(255)
                .IsOptional();

            this.Property(e => e.Comment)
                .HasColumnName(ColumnNames.Comment)
                .HasColumnType(ColumnTypes.NVarChar)
                .IsUnicode()
                .IsVariableLength()
                .IsMaxLength()
                .IsOptional();

            this.Property(e => e.CreatedTime)
                .HasColumnName(ColumnNames.CreatedTime)
                .HasColumnType(ColumnTypes.DateTime)
                .HasDatabaseGeneratedOption(DatabaseGeneratedOption.Computed)
                .IsOptional();
        }

        #endregion
    }
}
