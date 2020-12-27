using System;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations.Schema;

namespace MLManager.Database
{

    [Table("dataset_permissions")]
    public class DatasetPermission
    {
        public int UserId { get; set; }
        public Guid DatasetId { get; set; }
        //PermissionType 
        public PermissionLevel PermissionLevel { get; set; }
        public DateTime CreateTimestamp { get; set; }

        public static void BuildModel(ModelBuilder modelBuilder)
        {
            var datasetPermissionEntity = modelBuilder.Entity<DatasetPermission>();

            //Primary Account
            datasetPermissionEntity.HasKey(x => new { x.UserId, x.DatasetId });

            //Column Definitions
            datasetPermissionEntity.Property(x => x.UserId)
            .IsRequired()
            .HasColumnName(nameof(DatasetPermission.UserId).ToSnakeCase());

            datasetPermissionEntity.Property(x => x.DatasetId)
            .IsRequired()
            .HasColumnName(nameof(DatasetPermission.DatasetId).ToSnakeCase());

            datasetPermissionEntity.Property(x => x.PermissionLevel)
            .IsRequired()
            .HasColumnName(nameof(DatasetPermission.PermissionLevel).ToSnakeCase());

            datasetPermissionEntity.Property(x => x.CreateTimestamp)
            .IsRequired()
            .HasDefaultValueSql("now() at time zone 'utc'")
            .HasColumnName(nameof(DatasetPermission.CreateTimestamp).ToSnakeCase());

            //Foreign Key;
            datasetPermissionEntity.HasOne<User>().WithMany().HasForeignKey(x => x.UserId);
            datasetPermissionEntity.HasOne<Dataset>().WithMany().HasForeignKey(x => x.DatasetId);

        }
    }
}