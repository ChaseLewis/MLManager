using System;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations.Schema;

namespace MLManager.Database
{
    [Table("datasets")]
    public class Dataset
    {
        public Guid DatasetId { get; set; }
        public string DatasetName { get; set; }
        public int AccountId { get; set; }
        public DateTime CreationTimestamp { get; set; }

        public static void BuildModel(ModelBuilder modelBuilder)
        {
            var datasetEntity = modelBuilder.Entity<Dataset>();
            
            //Primary Key
            datasetEntity.HasKey(u => u.DatasetId);
            
            //Foreign Keys
            datasetEntity
            .HasOne<Account>()
            .WithMany()
            .HasForeignKey(u => u.AccountId);

            //NonClustered Indices
            datasetEntity.HasIndex(u => new { u.AccountId, u.DatasetName }).IsUnique();

            //Column Definitions
            datasetEntity.Property(u => u.DatasetId)
            .IsRequired()
            .HasDefaultValueSql("uuid_generate_v4()")
            .HasColumnName(nameof(Dataset.DatasetId).ToSnakeCase());

            datasetEntity.Property(u => u.DatasetName)
            .IsRequired()
            .HasColumnName(nameof(Dataset.DatasetName).ToSnakeCase());

            datasetEntity.Property(u => u.AccountId)
            .IsRequired()
            .HasColumnName(nameof(Dataset.AccountId).ToSnakeCase());

            datasetEntity.Property(u => u.CreationTimestamp)
            .IsRequired()
            .HasDefaultValueSql("now() at time zone 'utc'")
            .HasColumnName(nameof(Dataset.CreationTimestamp).ToSnakeCase());
        }
    }
}