namespace Microsoft.Azure.ChinaDataSolution.CrdAnalytics.Common.DataAccess.EntityConfigurations
{
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.ModelConfiguration;

    using Entities;

    internal sealed class ProductReviewSentenceSentimentEntityConfiguration
        : EntityTypeConfiguration<ProductReviewSentenceSentimentEntity>
    {
        public ProductReviewSentenceSentimentEntityConfiguration()
        {
            this.ToTable(TableNames.ProductReviewSentenceSentiments)
                .HasKey(e => e.Id);

            this.Property(e => e.Id)
                .HasColumnName(ColumnNames.Id)
                .HasColumnType(ColumnTypes.BigInt)
                .HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity)
                .IsRequired();

            this.Property(e => e.ReviewId)
                .HasColumnName(ColumnNames.ReviewId)
                .HasColumnType(ColumnTypes.BigInt)
                .IsOptional();

            this.Property(e => e.SentenceIndex)
                .HasColumnName(ColumnNames.SentenceIndex)
                .HasColumnType(ColumnTypes.Int)
                .IsOptional();

            this.Property(e => e.Sentence)
                .HasColumnName(ColumnNames.Sentence)
                .HasColumnType(ColumnTypes.NVarChar)
                .IsUnicode()
                .IsVariableLength()
                .IsMaxLength()
                .IsOptional();

            this.Property(e => e.Polarity)
                .HasColumnName(ColumnNames.Polarity)
                .HasColumnType(ColumnTypes.NVarChar)
                .IsUnicode()
                .IsVariableLength()
                .HasMaxLength(50)
                .IsOptional();

            this.Property(e => e.Sentiment)
                .HasColumnName(ColumnNames.Sentiment)
                .HasColumnType(ColumnTypes.Float)
                .IsOptional();

            this.Property(e => e.CreatedTime)
                .HasColumnName(ColumnNames.CreatedTime)
                .HasColumnType(ColumnTypes.DateTime)
                .HasDatabaseGeneratedOption(DatabaseGeneratedOption.Computed)
                .IsOptional();
        }
    }
}
