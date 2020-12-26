using System;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations.Schema;

namespace MLManager.Database
{
    public enum PermissionType
    {
        Users = 1,
        DataItems = 2,
        Datasets = 3
    }

    [Table("permission_type")]
    public class PermissionTypeEntity
    {
        public PermissionType PermissionTypeId { get; set; }
        public string Name { get; set; }

        public static void BuildModel(ModelBuilder modelBuilder)
        {
            var permissionTypeEntity = modelBuilder.Entity<PermissionTypeEntity>();
            
            //Primary Key
            permissionTypeEntity.HasKey(x => x.PermissionTypeId);

            //Constraint
            permissionTypeEntity.HasIndex(x => x.Name).IsUnique();

            //Column Definitions
            permissionTypeEntity.Property(x => x.PermissionTypeId)
            .IsRequired()
            .HasColumnName(nameof(PermissionTypeEntity.PermissionTypeId).ToSnakeCase());

            permissionTypeEntity.Property(x => x.Name)
            .IsRequired()
            .HasColumnName(nameof(PermissionTypeEntity.Name).ToSnakeCase());

            //Data
            permissionTypeEntity.HasData(new PermissionTypeEntity[]
            {
                new PermissionTypeEntity { PermissionTypeId = PermissionType.Users, Name = "Users" },
                new PermissionTypeEntity { PermissionTypeId = PermissionType.Datasets, Name = "Datasets" },
                new PermissionTypeEntity { PermissionTypeId = PermissionType.DataItems, Name = "DataItems" }
            });
        }
    }
}