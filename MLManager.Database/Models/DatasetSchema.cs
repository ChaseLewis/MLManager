using System;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace MLManager.Database
{
    [Table("dataset_schemas")]
    public class DatasetSchema
    {
        public Guid DatasetId { get; set; }
        public int VersionId { get; set; }
        public string Schema { get; set; }
        public DateTime CreationTimestamp { get; set; }

        public static void BuildModel(ModelBuilder modelBuilder)
        {
            var datasetSchemaEntity = modelBuilder.Entity<DatasetSchema>();

            //Primary Key
            datasetSchemaEntity.HasKey(u => new { u.DatasetId, u.VersionId });

            //Foreign Keys
            datasetSchemaEntity.HasOne<Dataset>().WithMany().HasForeignKey(u => u.DatasetId);

            //Column Definitions
            datasetSchemaEntity.Property(u => u.DatasetId)
            .IsRequired()
            .HasColumnName(nameof(DatasetSchema.DatasetId).ToSnakeCase());

            datasetSchemaEntity.Property(u => u.VersionId)
            .IsRequired()
            .HasColumnName(nameof(DatasetSchema.VersionId).ToSnakeCase());

            datasetSchemaEntity.Property(u => u.Schema)
            .IsRequired()
            .HasColumnType("jsonb")
            .HasColumnName(nameof(DatasetSchema.Schema).ToSnakeCase());

            datasetSchemaEntity.Property(u => u.CreationTimestamp)
            .IsRequired()
            .HasDefaultValueSql("now() at time zone 'utc'")
            .HasColumnName(nameof(DatasetSchema.CreationTimestamp).ToSnakeCase());
        }
    }
}