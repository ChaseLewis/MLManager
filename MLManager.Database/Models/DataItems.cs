using System;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations.Schema;

namespace MLManager.Database
{
    [Table("data_items")]
    public class DataItem
    {
        public Guid DatasetId { get; set; }
        public Guid DataItemId { get; set; }
        public int VersionId { get; set; }
        //Might need the accountId for sharding purposes ...
        public string LabelJson { get; set; }
        public DateTime CreationTimestamp { get; set; }

        public static void BuildModel(ModelBuilder modelBuilder)
        {
            var dataItemEntity = modelBuilder.Entity<DataItem>();

            //Primary Key
            dataItemEntity.HasKey(x => new { x.DatasetId, x.DataItemId });

            //Non-Clustered Indexes
            dataItemEntity.HasIndex(x => new { x.DataItemId });

            //Foreign Key
            dataItemEntity.HasOne<Dataset>().WithMany().HasForeignKey(x => x.DatasetId);

            //Column Definitions
            dataItemEntity.Property(x => x.DatasetId)
            .IsRequired()
            .HasColumnName(nameof(DataItem.DatasetId).ToSnakeCase());

            dataItemEntity.Property(x => x.DataItemId)
            .IsRequired()
            .HasDefaultValueSql("uuid_generate_v4()")
            .HasColumnName(nameof(DataItem.DataItemId).ToSnakeCase());

            dataItemEntity.Property(x => x.VersionId)
            .IsRequired()
            .HasColumnName(nameof(DataItem.VersionId).ToSnakeCase());

            dataItemEntity.Property(x => x.LabelJson)
            .IsRequired()
            .HasColumnType("jsonb")
            .HasColumnName(nameof(DataItem.LabelJson).ToSnakeCase());

            dataItemEntity.Property(x => x.CreationTimestamp)
            .IsRequired()
            .HasColumnName(nameof(DataItem.CreationTimestamp).ToSnakeCase())
            .HasDefaultValueSql("now() at time zone 'utc'");
        }
    }
}